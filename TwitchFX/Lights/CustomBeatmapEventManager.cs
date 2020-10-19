using System;

namespace TwitchFX.Lights {
	
	public class CustomBeatmapEventManager {
		
		public static event Action<BeatmapEventData> onCustomBeatmapEvent;
		
		public static void RaiseCustomBeatmapEvent(BeatmapEventData eventData) {
			
			onCustomBeatmapEvent?.Invoke(eventData);
			
		}
		
	}
	
}