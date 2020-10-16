using UnityEngine;

namespace TwitchFX {
	
	public class CustomBeatmapEventData : BeatmapEventData {
		
		public readonly Color color;
		
		public CustomBeatmapEventData(float time, BeatmapEventType type, int value, Color color) : base(time, type, value) {
			
			this.color = color;
			
		}
		
	}
	
}