using UnityEngine;

namespace TwitchFX.Lights {
	
	public struct ColorGradient {
		
		public readonly Color startColor;
		public readonly Color endColor;
		public readonly float duration;
		public readonly EasingFunction.Ease easing;
		
		public ColorGradient(
			Color startColor,
			Color endColor,
			float duration,
			EasingFunction.Ease easing
		) {
			
			this.startColor = startColor;
			this.endColor = endColor;
			this.duration = duration;
			this.easing = easing;
			
		}
		
	}
	
}