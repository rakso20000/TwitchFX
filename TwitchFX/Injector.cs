using Zenject;
using MonoBehavior = UnityEngine.MonoBehaviour;

namespace TwitchFX {
	
	public class Injector : MonoBehavior {
		
		public static Injector instance = null;
		
		private DiContainer container;
		
		[Inject]
		public void ReceiveContainer(DiContainer container) {
			
			this.container = container;
			
			if (instance != null) {
				
				Logger.log.Warn("Multiple instances of Injector exist");
				
				Destroy(instance);
				
			}
			
			instance = this;
			
			enabled = false;
			
		}
		
		public void Inject(object injectee) {
			
			container.Inject(injectee);
			
		}
		
		public Type Resolve<Type>() {
			
			return container.Resolve<Type>();
			
		}
		
		public bool HasBinding<Type>() {
			
			return container.HasBinding<Type>();
			
		}
		
		public void BindInstance<Type>(Type instance) {
			
			container.BindInstance(instance);
			
		}
		
		public void OnDestroy() {
			
			if (instance == this)
				instance = null;
			
		}
		
	}
	
}