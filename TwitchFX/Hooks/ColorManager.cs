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
	
}