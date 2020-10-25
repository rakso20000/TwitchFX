using UnityEngine;

namespace TwitchFX.Lights {
	
	public class CustomBeatmapEventData : BeatmapEventData {
		
		public readonly Color? color;
		public readonly ColorGradient? colorGradient;
		public readonly float? rotationSpeed;
		public readonly bool? rotationLockPosition;
		public readonly float? rotationDirection;
		public readonly float? rotationStartPosition;
		
		public CustomBeatmapEventData(
			float time,
			BeatmapEventType type,
			int value,
			Color? color,
			ColorGradient? colorGradient,
			float? rotationSpeed,
			bool? rotationLockPosition,
			float? rotationDirection,
			float? rotationStartPosition
		) : base(time, type, value) {
			
			this.color = color;
			this.colorGradient = colorGradient;
			this.rotationSpeed = rotationSpeed;
			this.rotationLockPosition = rotationLockPosition;
			this.rotationDirection = rotationDirection;
			this.rotationStartPosition = rotationStartPosition;
			
		}
		
	}
	
}