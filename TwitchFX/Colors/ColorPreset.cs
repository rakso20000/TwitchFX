using ChatCore.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchFX.Colors {
	
	public class ColorPreset {
		
		private static readonly Dictionary<string, ColorPreset> presets = new Dictionary<string, ColorPreset>();
		
		public static ColorPreset LoadColorPresetFromJSON(JSONNode rootJSON) {
			
			//just use the SimpleJSON library available in ChatCore
			
			if (!rootJSON.IsObject)
				throw new InvalidJSONException("Root is not a JSON object");
			
			bool leftLightColorExists = rootJSON.TryGetKey("leftLightColor", out JSONNode leftLightColorJSON);
			bool rightLightColorExists = rootJSON.TryGetKey("rightLightColor", out JSONNode rightLightColorJSON);
			bool leftNoteColorExists = rootJSON.TryGetKey("leftNoteColor", out JSONNode leftNoteColorJSON);
			bool rightNoteColorExists = rootJSON.TryGetKey("rightNoteColor", out JSONNode rightNoteColorJSON);
			bool leftSaberColorExists = rootJSON.TryGetKey("leftSaberColor", out JSONNode leftSaberColorJSON);
			bool rightSaberColorExists = rootJSON.TryGetKey("rightSaberColor", out JSONNode rightSaberColorJSON);
			bool wallColorExists = rootJSON.TryGetKey("wallColor", out JSONNode wallColorJSON);
			
			Color? leftLightColor = null;
			Color? rightLightColor = null;
			Color? leftNoteColor = null;
			Color? rightNoteColor = null;
			Color? leftSaberColor = null;
			Color? rightSaberColor = null;
			Color? wallColor = null;
			
			if (leftLightColorExists && (!leftLightColorJSON.IsString || !TryParseColor(((JSONString) leftLightColorJSON).Value, out leftLightColor)))
				throw new InvalidJSONException("Left light color is invalid");
			
			if (rightLightColorExists && (!rightLightColorJSON.IsString || !TryParseColor(((JSONString) rightLightColorJSON).Value, out rightLightColor)))
				throw new InvalidJSONException("Right light color is invalid");
			
			if (leftNoteColorExists && (!leftNoteColorJSON.IsString || !TryParseColor(((JSONString) leftNoteColorJSON).Value, out leftNoteColor)))
				throw new InvalidJSONException("Left note color is invalid");
			
			if (rightNoteColorExists && (!rightNoteColorJSON.IsString || !TryParseColor(((JSONString) rightNoteColorJSON).Value, out rightNoteColor)))
				throw new InvalidJSONException("Right note color is invalid");
			
			if (leftSaberColorExists && (!leftSaberColorJSON.IsString || !TryParseColor(((JSONString) leftSaberColorJSON).Value, out leftSaberColor)))
				throw new InvalidJSONException("Left saber color is invalid");
			
			if (rightSaberColorExists && (!rightSaberColorJSON.IsString || !TryParseColor(((JSONString) rightSaberColorJSON).Value, out rightSaberColor)))
				throw new InvalidJSONException("Right saber color is invalid");
			
			if (wallColorExists && (!wallColorJSON.IsString || !TryParseColor(((JSONString) wallColorJSON).Value, out wallColor)))
				throw new InvalidJSONException("Wall color is invalid");
			
			ColorPreset preset = new ColorPreset(
				leftLightColor,
				rightLightColor,
				leftNoteColor,
				rightNoteColor,
				leftSaberColor,
				rightSaberColor,
				wallColor
			);
			
			return preset;
			
		}
		
		private static bool TryParseColor(string colorStr, out Color? color) {
			
			bool result = Helper.TryParseColor(colorStr, out Color c);
			color = c;
			
			return result;
			
		}
		
		public static void SetColorPreset(string name, ColorPreset colorPreset) {
			
			presets.Add(name.ToLower(), colorPreset);
			
		}
		
		public static ColorPreset GetColorPreset(string name) {
			
			return presets.ContainsKey(name.ToLower()) ? presets[name.ToLower()] : null;
			
		}
		
		public static int GetColorPresetCount() {
			
			return presets.Count;
			
		}
		
		public static void ResetColorPresets() {
			
			presets.Clear();
			
		}
		
		public readonly Color? leftLightColor;
		public readonly Color? rightLightColor;
		public readonly Color? leftNoteColor;
		public readonly Color? rightNoteColor;
		public readonly Color? leftSaberColor;
		public readonly Color? rightSaberColor;
		public readonly Color? wallColor;
		
		private ColorPreset(
			Color? leftLightColor,
			Color? rightLightColor,
			Color? leftNoteColor,
			Color? rightNoteColor,
			Color? leftSaberColor,
			Color? rightSaberColor,
			Color? wallColor
		) {
			
			this.leftLightColor = leftLightColor;
			this.rightLightColor = rightLightColor;
			this.leftNoteColor = leftNoteColor;
			this.rightNoteColor = rightNoteColor;
			this.leftSaberColor = leftSaberColor;
			this.rightSaberColor = rightSaberColor;
			this.wallColor = wallColor;
			
		}
		
	}
	
}