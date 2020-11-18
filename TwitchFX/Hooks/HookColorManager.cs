using UnityEngine;
using TwitchFX.Colors;

namespace TwitchFX.Hooking {
	
	public class HookColorManager : Hook<ColorManager> {
		
		[Prefix]
		[Before("com.noodle.BeatSaber.ChromaCore")]
		public static bool ColorForType(ColorType type, ref Color __result) {
			
			if (ColorController.isNull || !ColorController.instance.useCustomNoteColors)
				return true;
			
			Color color;
			
			switch (type) {
			case ColorType.ColorA:
				
				color = ColorController.instance.noteColorLeft;
				
				if (Helper.IsRainbow(color))
					color = RainbowController.instance.GetLeftColor();
				
				break;
			case ColorType.ColorB:
				
				color = ColorController.instance.noteColorRight;
				
				if (Helper.IsRainbow(color))
					color = RainbowController.instance.GetRightColor();
				
				break;
			default:
				return true;
			}
			
			__result = color;
			
			return false;
			
		}
		
		[Prefix]
		[Before("com.noodle.BeatSaber.ChromaCore")]
		public static bool ColorForSaberType(SaberType type, ref Color __result) {
			
			if (ColorController.isNull || !ColorController.instance.useCustomSaberColors)
				return true;
			
			Color color;
			
			switch (type) {
			case SaberType.SaberA:
				
				color = ColorController.instance.saberColorLeft;
				
				if (Helper.IsRainbow(color))
					color = RainbowController.instance.GetLeftColor();
				
				break;
			case SaberType.SaberB:
				
				color = ColorController.instance.saberColorRight;
				
				if (Helper.IsRainbow(color))
					color = RainbowController.instance.GetRightColor();
				
				break;
			default:
				return true;
			}
			
			__result = color;
			
			return false;
			
		}
		
		[Prefix]
		[Before("com.noodle.BeatSaber.ChromaCore")]
		public static bool EffectsColorForSaberType(SaberType type, ref Color __result) {
			
			if (ColorController.isNull || !ColorController.instance.useCustomSaberColors)
				return true;
			
			Color color;
			
			switch (type) {
			case SaberType.SaberA:
				
				color = ColorController.instance.saberColorLeft;
				
				if (Helper.IsRainbow(color))
					color = RainbowController.instance.GetLeftColor();
				
				break;
			case SaberType.SaberB:
				
				color = ColorController.instance.saberColorRight;
				
				if (Helper.IsRainbow(color))
					color = RainbowController.instance.GetRightColor();
				
				break;
			default:
				return true;
			}
			
			Color.RGBToHSV(color, out float h, out float s, out float v);
			
			__result = Color.HSVToRGB(h, s, 1f);
			
			return false;
			
		}
		
		[Prefix]
		public static bool GetObstacleEffectColor(ref Color __result) {
			
			if (ColorController.isNull || !ColorController.instance.useCustomWallColor)
				return true;
			
			Color color = ColorController.instance.wallColor;
			
			if (Helper.IsRainbow(color))
				color = RainbowController.instance.GetWallColor();
			
			Color.RGBToHSV(color, out float h, out float s, out float v);
			
			__result = Color.HSVToRGB(h, s, 1f);
			
			return false;
			
		}
		
	}
	
}