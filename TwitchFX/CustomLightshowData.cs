using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChatCore.SimpleJSON;

namespace TwitchFX {
	
	public class CustomLightshowData {
		
		private static readonly Dictionary<string, CustomLightshowData> lightshows = new Dictionary<string, CustomLightshowData>();
		
		public static CustomLightshowData LoadLightshowDataFromFile(string path) {
			
			string json = File.ReadAllText(path);
			
			//just use the SimpleJSON library available in ChatCore
			
			JSONNode jsonRoot;
			
			try {
				
				jsonRoot = JSON.Parse(json);
				
			} catch (Exception) {
				
				return null;
				
			}
			
			if (!jsonRoot.IsObject)
				return null;
			
			bool eventsExists = jsonRoot.TryGetKey("_events", out JSONNode jsonEventsNode);
			
			if (!eventsExists || !jsonEventsNode.IsArray)
				return null;
			
			JSONArray jsonEvents = jsonEventsNode.AsArray;
			
			List<BeatmapEventData> eventsList = new List<BeatmapEventData>();
			
			foreach (JSONNode jsonEvent in jsonEvents) {
				
				bool timeExists = jsonEvent.TryGetKey("_time", out JSONNode jsonTime);
				bool typeExists = jsonEvent.TryGetKey("_type", out JSONNode jsonType);
				bool valueExists = jsonEvent.TryGetKey("_value", out JSONNode jsonValue);
				
				if (!timeExists || !typeExists || !valueExists)
					continue;
				
				if (!jsonTime.IsNumber || !jsonType.IsNumber || !jsonValue.IsNumber)
					continue;
				
				float time = jsonTime.AsFloat;
				int type = jsonType.AsInt;
				int value = jsonValue.AsInt;
				
				if (type < -1 || type > 15)
					continue;
				
				eventsList.Add(new BeatmapEventData(time, (BeatmapEventType) type, value));
				
			}
			
			BeatmapEventData[] events = eventsList.OrderBy(beatmapEvent => beatmapEvent.time).ToArray();
			
			return new CustomLightshowData(eventsList.ToArray());
			
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