using TwitchFX.Colors;
using UnityEngine;

namespace TwitchFX.Hooking {
	
	public class HookSaberModelController : Hook<SaberModelController> {
		
		[Prefix]
		public static bool Init(SaberModelController __instance, Transform parent, Saber saber) {
			
			ColorController.sabers[saber.saberType == SaberType.SaberA ? 0 : 1] = __instance;
			
			return true;
			
		}
		
	}
	
}