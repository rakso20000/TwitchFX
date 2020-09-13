using System;
using UnityEngine;

namespace TwitchFX {
	
	class LightController : MonoBehaviour {
		
		public static LightController instance { get; private set; }
		
		public bool overrideLights { get; private set; }
		
		public float disableOn = -1f;
		
		private LightWithIdManagerWrapper managerWrapper;
		
		private LightSwitchEventEffect[] defaultLights;
		private LightEffectController[] customLights;
		
		private void Awake() {
			
			if (instance != null) {
				
				Logger.log.Warn("Instance of LightController already exists, destroying.");
				
				Destroy(this);
				
				return;
				
			}
			
			instance = this;
			
		}
		
		private void Start() {
			
			defaultLights = Resources.FindObjectsOfTypeAll<LightSwitchEventEffect>();
			customLights = new LightEffectController[defaultLights.Length];
			
			LightSwitchEventEffect ligt = defaultLights[0];
			
			managerWrapper = new LightWithIdManagerWrapper(Helper.GetValue<LightWithIdManager>(ligt, "_lightManager"));
			
			BeatmapObjectCallbackController beatmapObjectCallbackController;
			beatmapObjectCallbackController = Helper.GetValue<BeatmapObjectCallbackController>(ligt, "_beatmapObjectCallbackController");
			
			for (int i = 0; i < defaultLights.Length; i++) {
				
				LightSwitchEventEffect light = defaultLights[i];
				
				Helper.SetValue<LightWithIdManagerWrapper>(light, "_lightManager", managerWrapper);
				
				int id = Helper.GetValue<int>(light, "_lightsID");
				BeatmapEventType eventTypeForThisLight = Helper.GetValue<BeatmapEventType>(light, "_event");
				
				customLights[i] = LightEffectController.CreateLightEffectController(managerWrapper, id, eventTypeForThisLight, beatmapObjectCallbackController);
				
			}
			
		}
		
		public void SetColors(Color leftColor, Color rightColor) {
			
			foreach (LightEffectController light in customLights)
				light.SetColors(leftColor, rightColor);
			
		}
		
		public void UpdateLights(ColorMode mode) {
			
			foreach (LightEffectController light in customLights)
				light.UpdateColors(mode);
			
			if (mode == ColorMode.Default) {
				
				foreach (LightSwitchEventEffect light in defaultLights) {
					
					int prevEventData = Helper.GetValue<int>(light, "_prevLightSwitchBeatmapEventDataValue");
					
					light.ProcessLightSwitchEvent(prevEventData, true);
					
				}
				
			}
			
			overrideLights = mode != ColorMode.Default;
			
		}
		
		private void Update() {
			
			if (disableOn != -1f && Time.time > disableOn) {
				
				UpdateLights(ColorMode.Default);
				
				disableOn = -1f;
				
			}
			
		}
		
		private void OnDestroy() {
			
			if (instance == this)
				instance = null;
			
		}
		
	}
	
}