using UnityEngine;
using Zenject;

namespace TwitchFX.Colors {
	
	public class RainbowController : LazyController<RainbowController> {
		
		private IAudioTimeSource timeSource;
		
		[Inject]
		public void Inject(IAudioTimeSource timeSource) {
			
			this.timeSource = timeSource;
			
		}
		
		protected override void Init() {
			
			//not much
			
		}
		
		public Color GetLeftColor() {
			
			float hue = timeSource.songTime / 5f % 1f;
			
			return Color.HSVToRGB(hue, 1f, 1f);
			
		}
		
		public Color GetRightColor() {
			
			float hue = (timeSource.songTime / 5f + 0.5f) % 1f;
			
			return Color.HSVToRGB(hue, 1f, 1f);
			
		}
		
		public Color GetWallColor() {
			
			float hue = (timeSource.songTime / 5f + 0.25f) % 1f;
			
			return Color.HSVToRGB(hue, 1f, 1f);
			
		}
		
	}
	
}