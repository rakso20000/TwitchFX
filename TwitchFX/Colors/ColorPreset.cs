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
			
			if (!leftLightColorExists)
				throw new InvalidJSONException("Left light color is missing");
			
			if (!rightLightColorExists)
				throw new InvalidJSONException("Right light color is missing");
			
			if (!leftNoteColorExists)
				throw new InvalidJSONException("Left note color is missing");
			
			if (!rightNoteColorExists)
				throw new InvalidJSONException("Right note color is missing");
			
			if (!leftSaberColorExists)
				throw new InvalidJSONException("Left saber color is missing");
			
			if (!rightSaberColorExists)
				throw new InvalidJSONException("Right saber color is missing");
			
			if (!wallColorExists)
				throw new InvalidJSONException("Wall color is missing");
			
			Color leftLightColor;
			Color rightLightColor;
			Color leftNoteColor;
			Color rightNoteColor;
			Color leftSaberColor;
			Color rightSaberColor;
			Color wallColor;
			
			if (!leftLightColorJSON.IsString || !Helper.TryParseColor(((JSONString) leftLightColorJSON).Value, out leftLightColor))
				throw new InvalidJSONException("Left light color is invalid");
			
			if (!rightLightColorJSON.IsString || !Helper.TryParseColor(((JSONString) rightLightColorJSON).Value, out rightLightColor))
				throw new InvalidJSONException("Right light color is invalid");
			
			if (!leftNoteColorJSON.IsString || !Helper.TryParseColor(((JSONString) leftNoteColorJSON).Value, out leftNoteColor))
				throw new InvalidJSONException("Left note color is invalid");
			
			if (!rightNoteColorJSON.IsString || !Helper.TryParseColor(((JSONString) rightNoteColorJSON).Value, out rightNoteColor))
				throw new InvalidJSONException("Right note color is invalid");
			
			if (!leftSaberColorJSON.IsString || !Helper.TryParseColor(((JSONString) leftSaberColorJSON).Value, out leftSaberColor))
				throw new InvalidJSONException("Left saber color is invalid");
			
			if (!rightSaberColorJSON.IsString || !Helper.TryParseColor(((JSONString) rightSaberColorJSON).Value, out rightSaberColor))
				throw new InvalidJSONException("Right saber color is invalid");
			
			if (!wallColorJSON.IsString || !Helper.TryParseColor(((JSONString) wallColorJSON).Value, out wallColor))
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
		
		public static void SetColorPreset(string name, ColorPreset colorPreset) {
			
			presets.Add(name.ToLower(), colorPreset);
			
		}
		
		public static ColorPreset GetColorPreset(string name) {
			
			return presets.ContainsKey(name.ToLower()) ? presets[name.ToLower()] : null;
			
		}
		
		public static int GetColorPresetCount() {
			
			return presets.Count;
			
		}
		
		public readonly Color leftLightColor;
		public readonly Color rightLightColor;
		public readonly Color leftNoteColor;
		public readonly Color rightNoteColor;
		public readonly Color leftSaberColor;
		public readonly Color rightSaberColor;
		public readonly Color wallColor;
		
		private ColorPreset(
			Color leftLightColor,
			Color rightLightColor,
			Color leftNoteColor,
			Color rightNoteColor,
			Color leftSaberColor,
			Color rightSaberColor,
			Color wallColor
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