using UnityEngine;
using TwitchFX.Colors;

namespace TwitchFX.Hooking {
	
	public class HookObstacleController : Hook<ObstacleController> {
		
		[Postfix]
		public static void Init(ObstacleController __instance) {
			
			if (ColorController.isNull || !ColorController.instance.useCustomWallColor)
				return;
			
			Color color = ColorController.instance.customWallColor;
			
			StretchableObstacle stretchable = Helper.GetValue<StretchableObstacle>(__instance, "_stretchableObstacle");
			
			ParametricBoxFrameController frame = Helper.GetValue<ParametricBoxFrameController>(stretchable, "_obstacleFrame");
			
			stretchable.SetSizeAndColor(frame.width, frame.height, frame.length, color);
			
		}
		
	}
	
}