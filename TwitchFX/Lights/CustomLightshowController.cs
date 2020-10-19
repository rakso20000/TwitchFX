using UnityEngine;
using MonoBehavior = UnityEngine.MonoBehaviour;

namespace TwitchFX.Lights {
	
	public class CustomLightshowController : MonoBehavior {
		
		public static CustomLightshowController CreateCustomLightshowController(
			CustomLightshowData lightshowData,
			IAudioTimeSource timeSource,
			LightMode restoreMode,
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
		private LightMode restoreMode;
		private float restoreDefaultLightsAfter;
		
		private float startTime;
		private bool initialized = false;
		
		private int eventIndex = 0;
		
		public void RestoreTo(LightMode? mode, float disableOn) {
			
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
				
				CustomBeatmapEventManager.RaiseCustomBeatmapEvent(eventData);
				
			}
			
			if (eventIndex >= lightshowData.Length)
				Destroy(null);
			
		}
		
		public void Destroy(CustomLightshowController nextLightshowController) {
			
			if (LightController.instance.lightshowController == this)
				LightController.instance.lightshowController = null;
			
			if (nextLightshowController == null) {
				
				if (restoreDefaultLightsAfter != -1f && Time.time > restoreDefaultLightsAfter) {
					
					LightController.instance.SetLightMode(LightMode.Default);
					
				} else {
					
					LightController.instance.SetLightMode(restoreMode);
					
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