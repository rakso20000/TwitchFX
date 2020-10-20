using HarmonyLib;
using TwitchFX.Colors;
using UnityEngine;

namespace TwitchFX.Hooking {
	
	[HarmonyPatch(typeof(SaberModelController))]
	[HarmonyPatch("Init")]
	public class SaberModelController_Init {
		
		public static bool Prefix(SaberModelController __instance, Transform parent, Saber saber) {
			
			ColorController.sabers[saber.saberType == SaberType.SaberA ? 0 : 1] = __instance;
			
			return true;
			
		}
		
	}
	
}