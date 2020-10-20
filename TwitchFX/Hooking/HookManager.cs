using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using TwitchFX.Lights;

namespace TwitchFX.Hooking {
	
	public class HookManager {
		
		public static HookManager instance {
			
			get {
				
				if (hookManager == null)
					hookManager = new HookManager();
				
				return hookManager;
				
			}
			
		}
		
		private static HookManager hookManager;
		
		public void HookAll(Assembly assembly) {
			
			Harmony harmony = new Harmony("com.rakso20000.beatsaber.twitchfx");
			
			foreach (Type type in assembly.GetTypes()) {
				
				if (!type.IsSubclassOf(typeof(HookBase)))
					continue;
				
				if (type.IsAbstract)
					continue;
				
				//type is Hook<Hooked>
				object hook = Activator.CreateInstance(type);
				
				Type hookedType = (Type) type.GetField("type").GetValue(hook);
				
				foreach (MethodInfo method in type.GetMethods()) {
					
					if (!method.IsStatic)
						continue;
					
					bool isPrefix = method.GetCustomAttribute<Prefix>() != null;
					bool isPostfix = method.GetCustomAttribute<Postfix>() != null;
					
					if (!isPrefix && !isPostfix)
						continue;
					
					MethodInfo hookedMethod;
					
					try {
						
						hookedMethod = hookedType.GetMethod(method.Name);
						
					} catch (AmbiguousMatchException) {
						
						Logger.log.Error("Error in HookManager: " + hookedType.Name + "." + method.Name + " is ambiguous");
						
						continue;
						
					}
					
					if (hookedMethod == null) {
						
						Logger.log.Error("Error in HookManager: " + hookedType.Name + "." + method.Name + " not found");
						
						continue;
						
					}
					
					List<string> beforeList = new List<string>();
					
					foreach (Before before in method.GetCustomAttributes<Before>())
						beforeList.Add(before.id);
					
					HarmonyMethod harmonyMethod = new HarmonyMethod(method);
					harmonyMethod.before = beforeList.ToArray();
					
					if (isPrefix)
						harmony.Patch(hookedMethod, prefix: harmonyMethod);
					else
						harmony.Patch(hookedMethod, postfix: harmonyMethod);
					
				}
				
				
			}
			
		}
		
	}
	
}