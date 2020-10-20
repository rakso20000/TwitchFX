using UnityEngine;
using TwitchFX.Colors;

namespace TwitchFX.Hooking {
	
	public class HookColorManager : Hook<ColorManager> {
		
		[Prefix]
		[Before("com.noodle.BeatSaber.ChromaCore")]
		public static bool ColorForType(ColorType type, ref Color __result) {
			
			if (ColorController.isNull || !ColorController.instance.useCustomNoteColors)
				return true;
			
			switch (type) {
			case ColorType.ColorA:
				__result = ColorController.instance.noteColorLeft;
				break;
			case ColorType.ColorB:
				__result = ColorController.instance.noteColorRight;
				break;
			default:
				return true;
			}
			
			return false;
			
		}
		
		[Prefix]
		[Before("com.noodle.BeatSaber.ChromaCore")]
		public static bool ColorForSaberType(SaberType type, ref Color __result) {
			
			if (ColorController.isNull || !ColorController.instance.useCustomSaberColors)
				return true;
			
			switch (type) {
			case SaberType.SaberA:
				__result = ColorController.instance.saberColorLeft;
				break;
			case SaberType.SaberB:
				__result = ColorController.instance.saberColorRight;
				break;
			default:
				return true;
			}
			
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
				break;
			case SaberType.SaberB:
				color = ColorController.instance.saberColorRight;
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
			
			Color color = ColorController.instance.customWallColor;
			
			Color.RGBToHSV(color, out float h, out float s, out float v);
			
			__result = Color.HSVToRGB(h, s, 1f);
			
			return false;
			
		}
		
	}
	
}