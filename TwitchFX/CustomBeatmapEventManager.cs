using System;

namespace TwitchFX {
	
	public class CustomBeatmapEventManager {
		
		public static event Action<BeatmapEventData> onCustomBeatmapEvent;
		
		public static void RaiseCustomBeatmapEvent(BeatmapEventData eventData) {
			
			onCustomBeatmapEvent?.Invoke(eventData);
			
		}
		
	}
	
}