using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using MonoBehavior = UnityEngine.MonoBehaviour;

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
		
		private Harmony harmony;
		
		public HookManager() {
			
			harmony = new Harmony("com.rakso20000.beatsaber.twitchfx");
			
		}
		
		public void HookAll(Assembly assembly) {
			
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
		
		public void BindOnCreation<Behavior>(bool allowMultiple = false, string methodName = null) where Behavior : MonoBehavior {
			
			Type type = typeof(Behavior);
			
			MethodInfo method;
			
			if (methodName != null) {
				
				method = type.GetMethod(
					methodName,
					BindingFlags.NonPublic |
					BindingFlags.Public |
					BindingFlags.Instance
				);
				
			} else {
				
				method = type.GetMethod(
					"Awake",
					BindingFlags.NonPublic |
					BindingFlags.Public |
					BindingFlags.Instance
				);
				
				if (method == null) {
					
					method = type.GetMethod(
						"Start",
						BindingFlags.NonPublic |
						BindingFlags.Public |
						BindingFlags.Instance
					);
					
				}
				
			}
			
			if (method == null) {
				
				Logger.log.Error("Couldn't hook into creation of " + type.Name + ". Instance might not get bound to container.");
				
				return;
				
			}
			
			HarmonyMethod hookHarmonyMethod = new HarmonyMethod(typeof(OnCreationHandler), allowMultiple ? "OnMultiBehaviorCreated" : "OnSingleBehaviorCreated");
			
			harmony.Patch(method, prefix: hookHarmonyMethod);
			
		}
		
		private class OnCreationHandler {
			
			private static readonly MethodInfo onBehaviorCreated = typeof(OnCreationHandler).GetMethod("OnBehaviorCreated", BindingFlags.Static | BindingFlags.Public);
			
			public static bool OnSingleBehaviorCreated(object __instance) {
				
				onBehaviorCreated.MakeGenericMethod(__instance.GetType()).Invoke(null, new object[] { __instance, true });
				
				return true;
				
			}
			
			public static bool OnMultiBehaviorCreated(object __instance) {
				
				onBehaviorCreated.MakeGenericMethod(__instance.GetType()).Invoke(null, new object[] { __instance, false });
				
				return true;
				
			}
			
			public static void OnBehaviorCreated<Type>(Type instance, bool single) {
				
				if (Injector.instance == null) {
					
					Logger.log.Error("Injector is not available. Could not bind " + typeof(Type).Name + ".");
					
					return;
					
				}
				
				if (single && Injector.instance.HasBinding<Type>()) {
					
					Logger.log.Warn(typeof(Type).Name + " is already bound. Not binding again.");
					
					return;
					
				}
				
				Injector.instance.BindInstance<Type>(instance);
				
				Logger.log.Debug("Bound instance of type " + typeof(Type).Name + ".");
				
			}
			
		}
		
	}
	
}