using UnityEngine;
using HarmonyLib;

namespace TwitchFX.Hooks {
	
	[HarmonyPatch(typeof(ObstacleController))]
	[HarmonyPatch("Init")]
	public class ColorController_Init {
		
		public static void Postfix(ObstacleController __instance) {
			
			if (ColorController.instance == null || !ColorController.instance.useCustomWallColor)
				return;
			
			Color color = ColorController.instance.customWallColor;
			
			StretchableObstacle stretchable = Helper.GetValue<StretchableObstacle>(__instance, "_stretchableObstacle");
			
			ParametricBoxFrameController frame = Helper.GetValue<ParametricBoxFrameController>(stretchable, "_obstacleFrame");
			
			stretchable.SetSizeAndColor(frame.width, frame.height, frame.length, color);
			
		}
		
	}
	
}