using System;
using UnityEngine;
using MonoBehavior = UnityEngine.MonoBehaviour;

namespace TwitchFX {
	
	public abstract class LazyController<Controller> : MonoBehavior where Controller : LazyController<Controller> {
		
		public static Controller instance {
			
			get {
				
				if (controller != null)
					return controller;

				//for now LazyControllers only work in GameCore
				if (Injector.instance == null)
					throw new InvalidOperationException("Can't create " + typeof(Controller).Name + " as Injector is not available");
				
				string name = "TwitchFX" + typeof(Controller).Name;
				
				controller = new GameObject(name).AddComponent<Controller>();
				
				Injector.instance.Inject(controller);
				
				controller.Init();
				
				return controller;
				
			}
			
		}
		
		public static bool isNull {
			
			get {
				
				return controller == null;
				
			}
			
		}
		
		//gets reset to null on destroy by Unity
		private static Controller controller = null;
		
		protected abstract void Init();
		
	}
	
}