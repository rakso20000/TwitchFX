using UnityEngine;

namespace TwitchFX.Lights {
	
	public class CustomBeatmapEventData : BeatmapEventData {
		
		public readonly Color? color;
		public readonly float? rotationSpeed;
		public readonly bool? rotationLockPosition;
		public readonly float? rotationDirection;
		
		public CustomBeatmapEventData(
			float time,
			BeatmapEventType type,
			int value,
			Color? color,
			float? rotationSpeed,
			bool? rotationLockPosition,
			float? rotationDirection
		) : base(time, type, value) {
			
			this.color = color;
			this.rotationSpeed = rotationSpeed;
			this.rotationLockPosition = rotationLockPosition;
			this.rotationDirection = rotationDirection;
			
		}
		
	}
	
}