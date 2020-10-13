using HarmonyLib;
using UnityEngine;

namespace TwitchFX.Hooks {
	
	[HarmonyPatch(typeof(SaberModelController))]
	[HarmonyPatch("Init")]
	public class SaberModelController_Init {
		
		public static bool Prefix(SaberModelController __instance, Transform parent, Saber saber) {
			
			ColorController.instance.sabers[saber.saberType == SaberType.SaberA ? 0 : 1] = __instance;
			
			return true;
			
		}
		
	}
	
}