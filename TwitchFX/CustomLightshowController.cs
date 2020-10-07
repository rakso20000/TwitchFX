using UnityEngine;

namespace TwitchFX {
	
	public class CustomLightshowController : MonoBehaviour {
		
		public static CustomLightshowController CreateCustomLightshowController(
			CustomLightshowData lightshowData,
			IAudioTimeSource timeSource,
			ColorMode restoreMode,
			float restoreDefaultLightsAfter
		) {
			
			CustomLightshowController controller = new GameObject("TwitchFXCustomLightshowController").AddComponent<CustomLightshowController>();
			
			controller.lightshowData = lightshowData;
			controller.timeSource = timeSource;
			controller.restoreMode = restoreMode;
			controller.restoreDefaultLightsAfter = restoreDefaultLightsAfter;
			
			return controller;
			
		}
		
		private CustomLightshowData lightshowData;
		private IAudioTimeSource timeSource;
		private ColorMode restoreMode;
		private float restoreDefaultLightsAfter;
		
		private float startTime;
		private bool initialized = false;
		
		private int eventIndex = 0;
		
		public void RestoreTo(ColorMode? mode, float disableOn) {
			
			if (mode.HasValue)
				restoreMode = mode.Value;
			
			restoreDefaultLightsAfter = disableOn;
			
		}
		
		public void LateUpdate() {
			
			if (!initialized) {
				
				startTime = timeSource.songTime;
				
				initialized = true;
				
			}
			
			for (; eventIndex < lightshowData.Length; eventIndex++) {
				
				BeatmapEventData eventData = lightshowData[eventIndex];
				
				if (eventData.time > timeSource.songTime - startTime)
					break;
				
				LightController.instance.HandleCustomEvent(eventData);
				
			}
			
			if (eventIndex >= lightshowData.Length)
				Destroy(null);
			
		}
		
		public void Destroy(CustomLightshowController nextLightshowController) {
			
			if (LightController.instance.lightshowController == this)
				LightController.instance.lightshowController = null;
			
			if (nextLightshowController == null) {
				
				if (restoreDefaultLightsAfter != -1f && Time.time > restoreDefaultLightsAfter) {
					
					LightController.instance.SetColorMode(ColorMode.Default);
					
				} else {
					
					LightController.instance.SetColorMode(restoreMode);
					
					if (restoreDefaultLightsAfter != -1f)
						LightController.instance.DisableIn(restoreDefaultLightsAfter - Time.time);
					
				}
				
			} else {
				
				nextLightshowController.RestoreTo(restoreMode, restoreDefaultLightsAfter);
				
			}
			
			Object.Destroy(this);
			
		}
		
	}
	
}