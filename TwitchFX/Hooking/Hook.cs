using System;

namespace TwitchFX.Hooking {
	
	public abstract class Hook<Hooked> : HookBase {
		
		public static Type type2 {
			
			get {
				
				return typeof(Hooked);
				
			}
			
		}
		
		public Type type;
		
		public Hook() {
			
			type = typeof(Hooked);
			
		}
		
	}
	
}