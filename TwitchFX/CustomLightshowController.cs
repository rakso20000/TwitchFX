using UnityEngine;

namespace TwitchFX {
	
	public class CustomLightshowController : MonoBehaviour {
		
		public static CustomLightshowController instance;
		
		public static CustomLightshowController CreateCustomLightshowController(
			CustomLightshowData lightshowData,
			IAudioTimeSource timeSource,
			ColorMode prevMode
		) {
			
			CustomLightshowController controller = new GameObject("TwitchFXCustomLightshowController").AddComponent<CustomLightshowController>();
			
			controller.lightshowData = lightshowData;
			controller.timeSource = timeSource;
			controller.prevMode = prevMode;
			
			instance = controller;
			
			return controller;
			
		}
		
		private CustomLightshowData lightshowData;
		private IAudioTimeSource timeSource;
		private ColorMode prevMode;
		
		private float startTime;
		private bool initialized = false;
		
		private int eventIndex = 0;
		
		public void Start() {
			
			if (prevMode == ColorMode.Default || prevMode == ColorMode.Disabled) {
				
				Color colorLeft = Helper.GetValue<SimpleColorSO>(LightController.instance.colorManager, "_environmentColor0").color;
				Color colorRight = Helper.GetValue<SimpleColorSO>(LightController.instance.colorManager, "_environmentColor1").color;
				
				LightController.instance.SetColors(colorLeft, colorRight);
				
			}
			
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
				Destroy();
			
		}
		
		public void Destroy() {
			
			LightController.instance.UpdateLights(prevMode);
			
			instance = null;
			
			Destroy(this);
			
		}
		
	}
	
}