using TwitchFX.Colors;
using UnityEngine;
using MonoBehavior = UnityEngine.MonoBehaviour;

namespace TwitchFX.Lights {
	
	public class CustomLightshowController : MonoBehavior {
		
		public static CustomLightshowController CreateCustomLightshowController(
			CustomLightshowData lightshowData,
			LightEffectController[] lights,
			ColorScheme colorScheme,
			IAudioTimeSource timeSource,
			LightMode restoreMode,
			float restoreDefaultLightsAfter
		) {
			
			if (restoreMode == LightMode.CustomLightshow) {
				
				restoreMode = LightController.instance.lightshowController.restoreMode;
				restoreDefaultLightsAfter = LightController.instance.lightshowController.restoreDefaultLightsAfter;
				
				LightController.instance.lightshowController.skipRestore = true;
				
				Destroy(LightController.instance.lightshowController);
				LightController.instance.lightshowController = null;
				
			}
			
			CustomLightshowController controller = new GameObject("TwitchFXCustomLightshowController").AddComponent<CustomLightshowController>();
			
			controller.lightshowData = lightshowData;
			controller.lights = lights;
			controller.colorScheme = colorScheme;
			controller.timeSource = timeSource;
			controller.restoreMode = restoreMode;
			controller.restoreDefaultLightsAfter = restoreDefaultLightsAfter;
			
			return controller;
			
		}
		
		private CustomLightshowData lightshowData;
		private LightEffectController[] lights;
		private ColorScheme colorScheme;
		private IAudioTimeSource timeSource;
		private LightMode restoreMode;
		private float restoreDefaultLightsAfter;
		
		private float startTime;
		private bool initialized = false;
		
		private bool skipRestore = false;
		
		private int eventIndex = 0;
		
		public void Start() {
			
			if (lightshowData.colorPreset != null) {
				
				ColorController.instance.SetNoteColors(lightshowData.colorPreset.leftNoteColor, lightshowData.colorPreset.rightNoteColor);
				ColorController.instance.SetSaberColors(lightshowData.colorPreset.leftSaberColor, lightshowData.colorPreset.rightSaberColor);
				ColorController.instance.SetWallColor(lightshowData.colorPreset.wallColor);
				
				foreach (LightEffectController light in lights)
					light.SetColors(lightshowData.colorPreset.leftLightColor, lightshowData.colorPreset.rightLightColor);
				
			} else if (restoreMode == LightMode.Custom) {
				
				foreach (LightEffectController light in lights)
					light.SetColors(LightController.instance.customColorLeft, LightController.instance.customColorRight);
				
			} else {
				
				foreach (LightEffectController light in lights)
					light.SetColors(colorScheme.environmentColor0, colorScheme.environmentColor1);
				
			}
			
		}
		
		public void OnInterceptedMode(LightMode mode, float disableOn) {
			
			restoreMode = mode;
			restoreDefaultLightsAfter = disableOn;
			
			if (mode == LightMode.Custom) {
				
				foreach (LightEffectController light in lights)
					light.SetColors(LightController.instance.customColorLeft, LightController.instance.customColorRight);
				
			} else {
				
				foreach (LightEffectController light in lights)
					light.SetColors(colorScheme.environmentColor0, colorScheme.environmentColor1);
				
			}
			
			LightController.instance.UpdateLightMode();
			
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
				Destroy(this);
			
		}
		
		public void OnDestroy() {
			
			if (LightController.instance.lightshowController == this)
				LightController.instance.lightshowController = null;
			
			if (!skipRestore) {
				
				if (lightshowData.colorPreset != null) {
					
					ColorController.instance.DisableNoteColors();
					ColorController.instance.DisableSaberColors();
					ColorController.instance.DisableWallColor();
					
				}
				
				if (restoreDefaultLightsAfter != -1f && Time.time > restoreDefaultLightsAfter) {
					
					LightController.instance.SetLightMode(LightMode.Default);
					
				} else {
					
					LightController.instance.SetLightMode(restoreMode, restoreDefaultLightsAfter == -1f ? (float?) null : restoreDefaultLightsAfter - Time.time);
					
				}
				
			}
			
		}
		
	}
	
}