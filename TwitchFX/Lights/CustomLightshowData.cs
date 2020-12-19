using ChatCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchFX.Colors;
using UnityEngine;

namespace TwitchFX.Lights {
	
	public class CustomLightshowData {
		
		private static readonly Dictionary<string, CustomLightshowData> lightshows = new Dictionary<string, CustomLightshowData>();
		
		public static CustomLightshowData LoadLightshowDataFromJSON(JSONNode rootJSON, string name) {
			
			//just use the SimpleJSON library available in ChatCore
			
			if (!rootJSON.IsObject)
				throw new InvalidJSONException("Root is not a JSON object");
			
			float bpm = 120f;
			
			if (
				rootJSON.TryGetKey("_bpm", out JSONNode bpmJSON) &&
				bpmJSON is JSONNumber bpmJSONNumber &&
				bpmJSONNumber.AsFloat > 0f
			)
				bpmJSON = bpmJSONNumber.AsFloat;
			
			ColorPreset colorPreset = null;
			
			if (rootJSON.TryGetKey("colorPreset", out JSONNode colorPresetJSON)) {
				
				if (colorPresetJSON is JSONString colorPresetJSONString) {
					
					colorPreset = ColorPreset.GetColorPreset(colorPresetJSONString.Value);
					
					if (colorPreset == null)
						Logger.log.Warn("Couldn't find color preset for lightshow: " + name);
					
				} else {
					
					try {
						
						colorPreset = ColorPreset.LoadColorPresetFromJSON(colorPresetJSON);
						
					} catch (InvalidJSONException e) {
						
						Logger.log.Warn("Failed loading color preset for lightshow: " + name);
						Logger.log.Warn("\tInvalid JSON data: " + e.errorMessage);
						
					}
					
				}
				
			}
			
			bool eventsExists = rootJSON.TryGetKey("_events", out JSONNode eventsNodeJSON);
			
			if (!eventsExists || !eventsNodeJSON.IsArray)
				throw new InvalidJSONException("No events JSON array found in root");
			
			JSONArray eventsJSON = eventsNodeJSON.AsArray;
			
			List<BeatmapEventData> eventsList = new List<BeatmapEventData>();
			
			foreach (JSONNode eventJSON in eventsJSON) {
				
				bool timeExists = eventJSON.TryGetKey("_time", out JSONNode timeJSON);
				bool typeExists = eventJSON.TryGetKey("_type", out JSONNode typeJSON);
				bool valueExists = eventJSON.TryGetKey("_value", out JSONNode valueJSON);
				
				if (!timeExists || !typeExists || !valueExists)
					continue;
				
				if (!timeJSON.IsNumber || !typeJSON.IsNumber || !valueJSON.IsNumber)
					continue;
				
				float time = timeJSON.AsFloat;
				int type = typeJSON.AsInt;
				int value = valueJSON.AsInt;
				
				if (type < -1 || type > 15)
					continue;
				
				if (
					eventJSON.TryGetKey("_customData", out JSONNode customDataJSON) &&
					customDataJSON is JSONObject customDataJSONObject
				) {
					
					Color? color = null;
					ColorGradient? colorGradient = null;
					float? direction = null;
					float? rotationSpeed = null;
					bool? rotationLockPosition = null;
					float? rotationStartPosition = null;
					string ringsNameFilter = null;
					bool? ringsCounterSpin = null;
					bool? ringsReset = null;
					float? ringsStep = null;
					float? ringsPropagationSpeed = null;
					float? ringsFlexySpeed = null;
					float? ringsStepMultiplier = null;
					float? ringsPropagationSpeedMultiplier = null;
					float? ringsFlexySpeedMultiplier = null;
					
					if (
						customDataJSONObject.TryGetKey("_color", out JSONNode colorJSON) &&
						TryParseColor(colorJSON, out Color col)
					)
						color = col;
					
					if (
						customDataJSON.TryGetKey("_lightGradient", out JSONNode colorGradientJSON) &&
						colorGradientJSON is JSONObject colorGradientJSONObject &&
						colorGradientJSONObject.TryGetKey("_startColor", out JSONNode startColorJSON) &&
						colorGradientJSONObject.TryGetKey("_endColor", out JSONNode endColorJSON) &&
						colorGradientJSONObject.TryGetKey("_duration", out JSONNode durationJSON) &&
						TryParseColor(startColorJSON, out Color startColor) &&
						TryParseColor(endColorJSON, out Color endColor) &&
						durationJSON is JSONNumber durationJSONNumber &&
						durationJSONNumber.AsFloat > 0f
					) {
						
						float currentBPM = bpm;
						
						if (
							colorGradientJSONObject.TryGetKey("_bpm", out JSONNode currentBPMJSON) &&
							currentBPMJSON is JSONNumber currentBPMJSONNumber &&
							currentBPMJSONNumber.AsFloat > 0f
						)
							currentBPM = currentBPMJSONNumber.AsFloat;
						
						EasingFunction.Ease? easing = null;
						
						if (
							colorGradientJSONObject.TryGetKey("_easing", out JSONNode easingJSON) &&
							easingJSON is JSONString easingJSONString &&
							Enum.TryParse(easingJSONString.Value, out EasingFunction.Ease ease)
						)
							easing = ease;
						
						colorGradient = new ColorGradient(
							startColor,
							endColor,
							durationJSONNumber.AsFloat * 60f / currentBPM,
							easing ?? EasingFunction.Ease.Linear
						);
						
					}
					
					if (
						customDataJSONObject.TryGetKey("_direction", out JSONNode directionJSON) &&
						directionJSON is JSONNumber directionJSONNumber &&
						(directionJSONNumber.AsInt == 0 || directionJSONNumber.AsInt == 1)
					)
						direction = directionJSONNumber.AsInt == 0 ? -1f : 1f;
					
					if (
						customDataJSONObject.TryGetKey("_preciseSpeed", out JSONNode rotationSpeedJSON) &&
						rotationSpeedJSON is JSONNumber rotationSpeedJSONNumber
					)
						rotationSpeed = rotationSpeedJSONNumber.AsFloat;
					
					if (
						customDataJSONObject.TryGetKey("_lockPosition", out JSONNode rotationLockPositionJSON) &&
						rotationLockPositionJSON is JSONBool rotationLockPositionJSONBool
					)
						rotationLockPosition = rotationLockPositionJSONBool.AsBool;
					
					if (
						customDataJSONObject.TryGetKey("_startPosition", out JSONNode rotationStartPositionJSON) &&
						rotationStartPositionJSON is JSONNumber rotationStartPositionJSONNumber
					)
						rotationStartPosition = rotationStartPositionJSONNumber.AsFloat;
					
					if (
						customDataJSONObject.TryGetKey("_nameFilter", out JSONNode ringsNameFilterJSON) &&
						ringsNameFilterJSON is JSONString ringsNameFilterJSONString
					)
						ringsNameFilter = ringsNameFilterJSONString.Value;
					
					if (
						customDataJSONObject.TryGetKey("_counterSpin", out JSONNode ringsCounterSpinJSON) &&
						ringsCounterSpinJSON is JSONBool ringsCounterSpinJSONBool
					)
						ringsCounterSpin = ringsCounterSpinJSONBool.AsBool;
					
					if (
						customDataJSONObject.TryGetKey("_reset", out JSONNode ringsResetJSON) &&
						ringsResetJSON is JSONBool ringsResetJSONBool
					)
						ringsReset = ringsResetJSONBool.AsBool;
					
					if (
						customDataJSONObject.TryGetKey("_step", out JSONNode ringsStepJSON) &&
						ringsStepJSON is JSONNumber ringsStepJSONNumber
					)
						ringsStep = ringsStepJSONNumber.AsFloat;
					
					if (
						customDataJSONObject.TryGetKey("_prop", out JSONNode ringsPropagationSpeedJSON) &&
						ringsPropagationSpeedJSON is JSONNumber ringsPropagationSpeedJSONNumber
					)
						ringsPropagationSpeed = ringsPropagationSpeedJSONNumber.AsFloat;
					
					if (
						customDataJSONObject.TryGetKey("_speed", out JSONNode ringsFlexySpeedJSON) &&
						ringsFlexySpeedJSON is JSONNumber ringsFlexySpeedJSONNumber
					)
						ringsFlexySpeed = ringsFlexySpeedJSONNumber.AsFloat;
					
					if (
						customDataJSONObject.TryGetKey("_stepMult", out JSONNode ringsStepMultiplierJSON) &&
						ringsStepMultiplierJSON is JSONNumber ringsStepMultiplierJSONNumber
					)
						ringsStepMultiplier = ringsStepMultiplierJSONNumber.AsFloat;
					
					if (
						customDataJSONObject.TryGetKey("_propMult", out JSONNode ringsPropagationSpeedMultiplierJSON) &&
						ringsPropagationSpeedMultiplierJSON is JSONNumber ringsPropagationSpeedMultiplierJSONNumber
					)
						ringsPropagationSpeedMultiplier = ringsPropagationSpeedMultiplierJSONNumber.AsFloat;
					
					if (
						customDataJSONObject.TryGetKey("_speedMult", out JSONNode ringsFlexySpeedMultiplierJSON) &&
						ringsFlexySpeedMultiplierJSON is JSONNumber ringsFlexySpeedMultiplierJSONNumber
					)
						ringsFlexySpeedMultiplier = ringsFlexySpeedMultiplierJSONNumber.AsFloat;
					
					CustomBeatmapEventData customEventData = new CustomBeatmapEventData(
						time,
						(BeatmapEventType) type,
						value,
						color,
						colorGradient,
						direction,
						rotationSpeed,
						rotationLockPosition,
						rotationStartPosition,
						ringsNameFilter,
						ringsCounterSpin,
						ringsReset,
						ringsStep,
						ringsPropagationSpeed,
						ringsFlexySpeed,
						ringsStepMultiplier,
						ringsPropagationSpeedMultiplier,
						ringsFlexySpeedMultiplier
					);
					
					eventsList.Add(customEventData);
					
					continue;
					
				}
				
				eventsList.Add(new BeatmapEventData(time, (BeatmapEventType) type, value));
				
			}
			
			if (eventsList.Count == 0)
				throw new InvalidJSONException("No valid events found in events JSON array");
			
			BeatmapEventData[] events = eventsList.OrderBy(beatmapEvent => beatmapEvent.time).ToArray();
			
			return new CustomLightshowData(events, colorPreset);
			
		}
		
		public static void SetLightshowData(string name, CustomLightshowData lightshowData) {
			
			lightshows.Add(name.ToLower(), lightshowData);
			
		}
		
		public static CustomLightshowData GetLightshowData(string name) {
			
			return lightshows.ContainsKey(name.ToLower()) ? lightshows[name.ToLower()] : null;
			
		}
		
		public static int GetLightshowDataCount() {
			
			return lightshows.Count;
			
		}
		
		public static void ResetLightshows() {
			
			lightshows.Clear();
			
		}
		
		private static bool TryParseColor(JSONNode colorJSON, out Color color) {
			
			if (
				colorJSON is JSONArray colorJSONArray &&
				colorJSONArray.List.Count >= 3 &&
				colorJSONArray[0].IsNumber &&
				colorJSONArray[1].IsNumber &&
				colorJSONArray[2].IsNumber &&
				(colorJSONArray[3].IsNumber || colorJSONArray.List.Count == 3)
			) {
				
				float r =colorJSONArray[0].AsFloat;
				float g = colorJSONArray[1].AsFloat;
				float b = colorJSONArray[2].AsFloat;
				float a = colorJSONArray.List.Count >= 4 ? colorJSONArray[3].AsFloat : 1f;
				
				color = new Color(r, g, b, a);
				
				return true;
				
			}
			
			color = Color.black;
			
			return false;
			
		}
		
		public int Length {
			
			get {
				
				return beatmapEvents.Length;
				
			}
			
		}
		
		public BeatmapEventData this[int index] {
			
			get {
				
				return beatmapEvents[index];
				
			}
			
		}
		
		public readonly ColorPreset colorPreset;
		
		private BeatmapEventData[] beatmapEvents;
		
		private CustomLightshowData(BeatmapEventData[] beatmapEvents, ColorPreset colorPreset) {
			
			this.beatmapEvents = beatmapEvents;
			this.colorPreset = colorPreset;
			
		}
		
	}
	
}