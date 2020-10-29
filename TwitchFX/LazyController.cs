using System;
using System.Runtime.ExceptionServices;
using UnityEngine;
using MonoBehavior = UnityEngine.MonoBehaviour;

namespace TwitchFX {
	
	public abstract class LazyController<Controller> : MonoBehavior where Controller : LazyController<Controller> {
		
		public static Controller instance {
			
			get {
				
				lock (typeof(Controller)) {
					
					if (controller != null)
						return controller;
					
					//for now LazyControllers only work in GameCore
					if (Injector.instance == null)
						throw new InvalidOperationException("Can't create " + typeof(Controller).Name + " as Injector is not available");
					
					string name = "TwitchFX" + typeof(Controller).Name;
					
					controller = new GameObject(name).AddComponent<Controller>();
					
					try {
						
						Injector.instance.Inject(controller);
						
						controller.Init();
						
					} catch (Exception exception) {
						
						Destroy(controller);
						
						controller = null;
						
						ExceptionDispatchInfo.Capture(exception).Throw();
						
					}
					
				}
				
				return controller;
				
			}
			
		}
		
		public static bool isNull {
			
			get {
				
				return controller == null;
				
			}
			
		}
		
		//gets reset to null on scene change by Unity
		private static Controller controller = null;
		
		protected abstract void Init();
		
	}
	
}