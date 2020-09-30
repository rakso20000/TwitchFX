using UnityEngine;

namespace TwitchFX {
	
	public class LightController : MonoBehaviour {
		
		public static LightController instance { get; private set; }
		
		public ColorMode mode { get; private set; } = ColorMode.Default;
		public bool boostColors { get; private set; } = false;
		
		private float disableOn = -1f;
		private float disableBoostOn = -1f;
		
		private LightWithIdManagerWrapper managerWrapper;
		private BeatEffectSpawner beatEffectSpawner;
		
		private float defaultBeatEffectDuration;
		
		private LightSwitchEventEffect[] defaultLights;
		private LightEffectController[] customLights;
		
		public void Awake() {
			
			if (instance != null) {
				
				Logger.log.Warn("Instance of LightController already exists, destroying.");
				
				Destroy(this);
				
				return;
				
			}
			
			instance = this;
			
		}
		
		public void Start() {
			
			beatEffectSpawner = Resources.FindObjectsOfTypeAll<BeatEffectSpawner>()[0];
			defaultBeatEffectDuration = Helper.GetValue<float>(beatEffectSpawner, "_effectDuration");
			
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
			
			enabled = false;
			
		}
		
		public void DisableIn(float duration) {
			
			disableOn = Time.time + duration;
			
			enabled = true;
			
		}
		
		public void CancelDisable() {
			
			disableOn = -1f;
			
			if (disableBoostOn == -1f)
				enabled = false;
			
		}
		
		public void BoostLights(float duration) {
			
			boostColors = true;
			
			disableBoostOn = Time.time + duration;
			
			UpdateLights(mode);
			
			enabled = true;
			
		}
		
		public void SetColors(Color leftColor, Color rightColor) {
			
			foreach (LightEffectController light in customLights)
				light.SetColors(leftColor, rightColor);
			
		}
		
		public void UpdateLights(ColorMode mode) {
			
			ColorMode prevMode = this.mode;
			this.mode = mode;
			
			foreach (LightEffectController light in customLights)
				light.UpdateColors(mode);
			
			if (mode == prevMode)
				return;
			
			if (mode == ColorMode.Default) {
				
				foreach (LightSwitchEventEffect light in defaultLights) {
					
					int prevEventData = Helper.GetValue<int>(light, "_prevLightSwitchBeatmapEventDataValue");
					
					light.ProcessLightSwitchEvent(prevEventData, true);
					
				}
				
			} else if (mode == ColorMode.Disabled) {
				
				Helper.SetValue<float>(beatEffectSpawner, "_effectDuration", 0f);
				
			}
			
			if (prevMode == ColorMode.Disabled) {
				
				Logger.log.Info("Setting duration to " + defaultBeatEffectDuration);
				
				Helper.SetValue<float>(beatEffectSpawner, "_effectDuration", defaultBeatEffectDuration);
				
			}
			
		}
		
		public void Update() {
			
			if (disableOn != -1f && Time.time > disableOn) {
				
				UpdateLights(ColorMode.Default);
				
				disableOn = -1f;
				
				if (disableBoostOn == -1f)
					enabled = false;
				
			}
			
			if (disableBoostOn != -1f && Time.time > disableBoostOn) {
				
				boostColors = false;
				
				disableBoostOn = -1f;
				
				UpdateLights(mode);
				
				if (disableOn == -1f)
					enabled = false;
				
			}
			
		}
		
		public void OnDestroy() {
			
			if (instance == this)
				instance = null;
			
		}
		
	}
	
}