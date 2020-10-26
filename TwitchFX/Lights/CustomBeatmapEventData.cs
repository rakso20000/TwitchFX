using UnityEngine;

namespace TwitchFX.Lights {
	
	public class CustomBeatmapEventData : BeatmapEventData {
		
		public readonly Color? color;
		public readonly ColorGradient? colorGradient;
		public readonly float? direction;
		public readonly float? rotationSpeed;
		public readonly bool? rotationLockPosition;
		public readonly float? rotationStartPosition;
		public readonly string ringsNameFilter;
		public readonly bool? ringsCounterSpin;
		public readonly bool? ringsReset;
		public readonly float? ringsStep;
		public readonly float? ringsPropagationSpeed;
		public readonly float? ringsFlexySpeed;
		public readonly float? ringsStepMultiplier;
		public readonly float? ringsPropagationSpeedMultiplier;
		public readonly float? ringsFlexySpeedMultiplier;
		
		public CustomBeatmapEventData(
			float time,
			BeatmapEventType type,
			int value,
			Color? color,
			ColorGradient? colorGradient,
			float? direction,
			float? rotationSpeed,
			bool? rotationLockPosition,
			float? rotationStartPosition,
			string ringsNameFilter,

			bool? ringsCounterSpin,
			bool? ringsReset,
			float? ringsStep,
			float? ringsPropagationSpeed,
			float? ringsFlexySpeed,
			float? ringsStepMultiplier,
			float? ringsPropagationSpeedMultiplier,
			float? ringsFlexySpeedMultiplier
		) : base(time, type, value) {
			
			this.color = color;
			this.colorGradient = colorGradient;
			this.direction = direction;
			this.rotationSpeed = rotationSpeed;
			this.rotationLockPosition = rotationLockPosition;
			this.rotationStartPosition = rotationStartPosition;
			this.ringsNameFilter = ringsNameFilter;
			this.ringsCounterSpin = ringsCounterSpin;
			this.ringsReset = ringsReset;
			this.ringsStep = ringsStep;
			this.ringsPropagationSpeed = ringsPropagationSpeed;
			this.ringsFlexySpeed = ringsFlexySpeed;
			this.ringsStepMultiplier = ringsStepMultiplier;
			this.ringsPropagationSpeedMultiplier = ringsPropagationSpeedMultiplier;
			this.ringsFlexySpeedMultiplier = ringsFlexySpeedMultiplier;
			
		}
		
	}
	
}