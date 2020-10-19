using System;
using UnityEngine;
using MonoBehavior = UnityEngine.MonoBehaviour;

namespace TwitchFX {
	
	public abstract class LazyController<Controller> : MonoBehavior where Controller : LazyController<Controller> {
		
		public static Controller instance {
			
			get {
				
				if (controller != null)
					return controller;
				
				string name = "TwitchFX" + typeof(Controller).Name;
				
				controller = new GameObject(name).AddComponent<Controller>();
				
				try {
					
					controller.Init();
					
				} catch (Exception exception) {
					
					Destroy(controller);
					
					controller = null;
					
					throw exception;
					
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