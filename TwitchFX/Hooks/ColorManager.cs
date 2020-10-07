using UnityEngine;
using HarmonyLib;

namespace TwitchFX.Hooks {
	
	[HarmonyPatch(typeof(ColorManager))]
	[HarmonyPatch("ColorForNoteType")]
	public class ColorManager_ColorForNoteType {
		
		[HarmonyBefore(new string[] { "com.noodle.BeatSaber.ChromaCore" })]
		public static bool Prefix(NoteType type, ref Color __result) {
			
			if (ColorController.instance == null || !ColorController.instance.useCustomNoteColors)
				return true;
			
			switch (type) {
			case NoteType.NoteA:
				__result = ColorController.instance.noteColorLeft;
				break;
			case NoteType.NoteB:
				__result = ColorController.instance.noteColorRight;
				break;
			default:
				return true;
			}
			
			return false;
			
		}
		
	}
	
	[HarmonyPatch(typeof(ColorManager))]
	[HarmonyPatch("ColorForSaberType")]
	public class ColorManager_ColorForSaberType {
		
		[HarmonyBefore(new string[] { "com.noodle.BeatSaber.ChromaCore" })]
		public static bool Prefix(SaberType type, ref Color __result) {
			
			if (ColorController.instance == null || !ColorController.instance.useCustomSaberColors)
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
		
	}
	
	[HarmonyPatch(typeof(ColorManager))]
	[HarmonyPatch("EffectsColorForSaberType")]
	public class ColorManager_EffectsColorForSaberType {
		
		[HarmonyBefore(new string[] { "com.noodle.BeatSaber.ChromaCore" })]
		public static bool Prefix(SaberType type, ref Color __result) {
			
			if (ColorController.instance == null || !ColorController.instance.useCustomSaberColors)
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
		
	}
	
}