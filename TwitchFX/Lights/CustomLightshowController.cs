using TwitchFX.Colors;
using UnityEngine;
using MonoBehavior = UnityEngine.MonoBehaviour;

namespace TwitchFX.Lights {
	
	public class CustomLightshowController : MonoBehavior {
		
		public static CustomLightshowController Create(
			CustomLightshowData lightshowData,
			LightEffectController[] lights,
			ColorScheme colorScheme,
			IAudioTimeSource timeSource,
			LightMode restoreMode,
			float restoreDefaultLightsAfter
		) {
			
			CustomLightshowController controller = new GameObject("TwitchFXCustomLightshowController").AddComponent<CustomLightshowController>();
			
			if (restoreMode == LightMode.CustomLightshow) {
				
				CustomLightshowController prevLightshow = LightController.instance.lightshowController;
				
				restoreMode = prevLightshow.restoreMode;
				restoreDefaultLightsAfter = prevLightshow.restoreDefaultLightsAfter;
				
				prevLightshow.skipRestoreMode = true;
				
				if (lightshowData.colorPreset != null) {
					
					controller.restoreSaberColorLeft = prevLightshow.restoreSaberColorLeft;
					controller.restoreSaberColorRight = prevLightshow.restoreSaberColorRight;
					controller.restoreNoteColorLeft = prevLightshow.restoreNoteColorLeft;
					controller.restoreNoteColorRight = prevLightshow.restoreNoteColorRight;
					controller.restoreWallColor = prevLightshow.restoreWallColor;
					controller.restoreSaberColorsAfter = prevLightshow.restoreSaberColorsAfter;
					controller.restoreNoteColorsAfter = prevLightshow.restoreNoteColorsAfter;
					controller.restoreWallColorAfter = prevLightshow.restoreWallColorAfter;
					controller.skipSetRestore = true;
					
					prevLightshow.skipRestoreColors = true;
					
				}
				
				Destroy(prevLightshow);
				LightController.instance.lightshowController = null;
				
			}
			
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
		private Color? restoreSaberColorLeft = null;
		private Color? restoreSaberColorRight = null;
		private Color? restoreNoteColorLeft = null;
		private Color? restoreNoteColorRight = null;
		private Color? restoreWallColor = null;
		private float restoreDefaultLightsAfter;
		private float restoreSaberColorsAfter = -1f;
		private float restoreNoteColorsAfter = -1f;
		private float restoreWallColorAfter = -1f;
		
		private bool skipSetRestore = false;
		
		private float startTime;
		private bool initialized = false;
		
		private bool skipRestoreMode = false;
		private bool skipRestoreColors = false;
		
		private int eventIndex = 0;
		
		public void Start() {
			
			if (lightshowData.colorPreset != null) {
				
				if (!skipSetRestore)
					ColorController.instance.SetRestoreValues(this);
				
				ColorController.instance.StopIntercept();
				
				ColorController.instance.SetNoteColors(lightshowData.colorPreset.leftNoteColor, lightshowData.colorPreset.rightNoteColor);
				ColorController.instance.SetSaberColors(lightshowData.colorPreset.leftSaberColor, lightshowData.colorPreset.rightSaberColor);
				ColorController.instance.SetWallColor(lightshowData.colorPreset.wallColor);
				
				ColorController.instance.StartIntercept(this);
				
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
		
		public void OnInterceptedSaberColors(Color? leftColor, Color? rightColor, float disableOn) {
			
			restoreSaberColorLeft = leftColor;
			restoreSaberColorRight = rightColor;
			restoreSaberColorsAfter = disableOn;
			
		}
		
		public void OnInterceptedNoteColors(Color? leftColor, Color? rightColor, float disableOn) {
			
			restoreNoteColorLeft = leftColor;
			restoreNoteColorRight = rightColor;
			restoreNoteColorsAfter = disableOn;
			
		}
		
		public void OnInterceptedWallColor(Color? color, float disableOn) {
			
			restoreWallColor = color;
			restoreWallColorAfter = disableOn;
			
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
			
			if (!skipRestoreColors && lightshowData.colorPreset != null) {
				
				ColorController.instance.StopIntercept();
				
				if (restoreSaberColorLeft.HasValue) {
					
					ColorController.instance.SetSaberColors(
						restoreSaberColorLeft.Value,
						restoreSaberColorRight.Value,
						restoreSaberColorsAfter == -1f ? (float?) null : restoreSaberColorsAfter - Time.time
					);
					
				} else {
					
					ColorController.instance.DisableSaberColors();
					
				}
				
				if (restoreNoteColorLeft.HasValue) {
					
					ColorController.instance.SetNoteColors(
						restoreNoteColorLeft.Value,
						restoreNoteColorRight.Value,
						restoreNoteColorsAfter == -1f ? (float?) null : restoreNoteColorsAfter - Time.time
					);
					
				} else {
					
					ColorController.instance.DisableNoteColors();
					
				}
				
				if (restoreWallColor.HasValue) {
					
					ColorController.instance.SetWallColor(
						restoreWallColor.Value,
						restoreWallColorAfter == -1f ? (float?) null : restoreWallColorAfter - Time.time
					);
					
				} else {
					
					ColorController.instance.DisableWallColor();
					
				}
				
			}
			
			if (!skipRestoreMode) {
				
				if (restoreDefaultLightsAfter != -1f && Time.time > restoreDefaultLightsAfter) {
					
					LightController.instance.SetLightMode(LightMode.Default);
					
				} else {
					
					LightController.instance.SetLightMode(restoreMode, restoreDefaultLightsAfter == -1f ? (float?) null : restoreDefaultLightsAfter - Time.time);
					
				}
				
			}
			
		}
		
	}
	
}