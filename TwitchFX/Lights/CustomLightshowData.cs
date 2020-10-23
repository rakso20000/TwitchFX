using ChatCore.SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TwitchFX.Lights {
	
	public class CustomLightshowData {
		
		private static readonly Dictionary<string, CustomLightshowData> lightshows = new Dictionary<string, CustomLightshowData>();
		
		public static CustomLightshowData LoadLightshowDataFromJSON(JSONNode rootJSON) {
			
			//just use the SimpleJSON library available in ChatCore
			
			if (!rootJSON.IsObject)
				throw new InvalidJSONException("Root is not a JSON object");
			
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
					float? rotationSpeed = null;
					bool? rotationLockPosition = null;
					float? rotationDirection = null;
					float? rotationStartPosition = null;
					
					if (
						customDataJSONObject.TryGetKey("_color", out JSONNode colorJSON) &&
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
						
					}
					
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
						customDataJSONObject.TryGetKey("_direction", out JSONNode rotationDirectionJSON) &&
						rotationDirectionJSON is JSONNumber rotationDirectionJSONNumber &&
						(rotationDirectionJSONNumber.AsInt == 0 || rotationDirectionJSONNumber.AsInt == 1)
					)
						rotationDirection = rotationDirectionJSONNumber.AsInt == 0 ? -1f : 1f;
					
					if (
						customDataJSONObject.TryGetKey("_startPosition", out JSONNode rotationStartPositionJSON) &&
						rotationStartPositionJSON is JSONNumber rotationStartPositionJSONNumber
					)
						rotationStartPosition = rotationStartPositionJSONNumber.AsFloat;
					
					CustomBeatmapEventData customEventData = new CustomBeatmapEventData(
						time,
						(BeatmapEventType) type,
						value,
						color,
						rotationSpeed,
						rotationLockPosition,
						rotationDirection,
						rotationStartPosition
					);
					
					eventsList.Add(customEventData);
					
					continue;
					
				}
				
				eventsList.Add(new BeatmapEventData(time, (BeatmapEventType) type, value));
				
			}
			
			if (eventsList.Count == 0)
				throw new InvalidJSONException("No valid events found in events JSON array");
			
			BeatmapEventData[] events = eventsList.OrderBy(beatmapEvent => beatmapEvent.time).ToArray();
			
			return new CustomLightshowData(events);
			
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
		
		private BeatmapEventData[] beatmapEvents;
		
		private CustomLightshowData(BeatmapEventData[] beatmapEvents) {
			
			this.beatmapEvents = beatmapEvents;
			
		}
		
	}
	
}