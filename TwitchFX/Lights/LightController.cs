using System;
using UnityEngine;
using Zenject;

namespace TwitchFX.Lights {
	
	public class LightController : LazyController<LightController> {
		
		public CustomLightshowController lightshowController;
		
		public LightMode mode { get; private set; } = LightMode.Default;
		public bool boostColors { get; private set; } = false;
		
		public Color customColorLeft { get; private set; }
		public Color customColorRight { get; private set; }
		
		private event Action<LightMode> onLightModeUpdated;
		
		private float disableOn = -1f;
		private float disableBoostOn = -1f;
		
		private ColorScheme colorScheme;
		private LightWithIdManager lightWithIdManager;
		private LightWithIdManagerWrapper managerWrapper;
		private BeatmapObjectCallbackController bocc;
		private BeatEffectSpawner beatEffectSpawner;
		private IAudioTimeSource timeSource;
		
		private float defaultBeatEffectDuration;
		
		private LightSwitchEventEffect[] defaultLights;
		private LightEffectController[] customLights;
		private LightEffectController[] lightshowLights;
		
		[Inject]
		public void Inject(
			ColorScheme colorScheme,
			LightWithIdManager lightWithIdManager,
			BeatmapObjectCallbackController bocc,
			BeatEffectSpawner beatEffectSpawner,
			IAudioTimeSource timeSource,
			LightSwitchEventEffect[] defaultLights
		) {
			
			this.colorScheme = colorScheme;
			this.lightWithIdManager = lightWithIdManager;
			this.bocc = bocc;
			this.beatEffectSpawner = beatEffectSpawner;
			this.timeSource = timeSource;
			this.defaultLights = defaultLights;
			
		}
		
		protected override void Init() {
			
			defaultBeatEffectDuration = Helper.GetValue<float>(beatEffectSpawner, "_effectDuration");
			
			customLights = new LightEffectController[defaultLights.Length];
			lightshowLights = new LightEffectController[defaultLights.Length];
			
			managerWrapper = new LightWithIdManagerWrapper(lightWithIdManager);
			
			for (int i = 0; i < defaultLights.Length; i++) {
				
				LightSwitchEventEffect light = defaultLights[i];
				
				Helper.SetValue<LightWithIdManagerWrapper>(light, "_lightManager", managerWrapper);
				
				LightEffectController customLight = LightEffectController.Create(managerWrapper, LightMode.Custom, light, timeSource);
				LightEffectController lightshowLight = LightEffectController.Create(managerWrapper, LightMode.CustomLightshow, light, timeSource);
				
				bocc.beatmapEventDidTriggerEvent += customLight.OnEvent;
				onLightModeUpdated += customLight.UpdateLightMode;
				
				CustomBeatmapEventManager.onCustomBeatmapEvent += lightshowLight.OnEvent;
				onLightModeUpdated += lightshowLight.UpdateLightMode;
				
				customLights[i] = customLight;
				lightshowLights[i] = lightshowLight;
				
			}
			
			enabled = false;
			
		}
		
		public void BoostLights(float duration) {
			
			boostColors = true;
			
			disableBoostOn = Time.time + duration;
			
			UpdateLightMode();
			
			enabled = true;
			
		}
		
		public void SetColors(Color leftColor, Color rightColor) {
			
			customColorLeft = leftColor;
			customColorRight = rightColor;
			
			foreach (LightEffectController light in customLights)
				light.SetColors(leftColor, rightColor);
			
		}
		
		public bool ShowCustomLightshow(string name) {
			
			CustomLightshowData lightshowData = CustomLightshowData.GetLightshowData(name);
			
			if (lightshowData == null)
				return false;
			
			foreach (LightEffectController light in lightshowLights)
				light.Reset();
			
			CustomLightshowController lightshowController = CustomLightshowController.Create(
				lightshowData,
				lightshowLights,
				colorScheme,
				timeSource,
				mode,
				disableOn
			);
			
			SetLightMode(LightMode.CustomLightshow);
			
			this.lightshowController = lightshowController;
			
			return true;
			
		}
		
		public void SetLightMode(LightMode mode, float? duration = null) {
			
			float disableOn = duration.HasValue ? Time.time + duration.Value : -1f;
			
			LightMode prevMode = this.mode;
			
			if (prevMode == LightMode.CustomLightshow && lightshowController != null) {
				
				lightshowController.OnInterceptedMode(mode, disableOn);
				
				return;
				
			}
			
			this.disableOn = disableOn;
			
			enabled = disableOn != -1f || disableBoostOn != -1f;
			
			this.mode = mode;
			
			UpdateLightMode();
			
			if (mode == LightMode.Disabled || mode == LightMode.CustomLightshow)
				managerWrapper.ClearLights();
			
			if (mode == prevMode)
				return;
			
			if (mode == LightMode.Disabled) {
				
				Helper.SetValue<float>(beatEffectSpawner, "_effectDuration", 0f);
				
			} else if (mode == LightMode.CustomLightshow) {
				
				LightRotationController.instance.DisableDefaultLightRotationEvents();
				RingController.instance.DisableDefaultRingEvents();
				
				LightRotationController.instance.ResetCustomLightRotation();
				RingController.instance.ResetCustomRingRotation();
				
			}
			
			if (prevMode == LightMode.Disabled) {
				
				Helper.SetValue<float>(beatEffectSpawner, "_effectDuration", defaultBeatEffectDuration);
				
			} else if (prevMode == LightMode.CustomLightshow) {
				
				LightRotationController.instance.EnableDefaultLightRotationEvents();
				RingController.instance.EnableDefaultRingEvents();
				
			}
			
		}
		
		public void UpdateLightMode() {
			
			onLightModeUpdated?.Invoke(mode);
			
			if (mode == LightMode.Default) {
				
				foreach (LightSwitchEventEffect light in defaultLights) {
					
					int prevEventData = Helper.GetValue<int>(light, "_prevLightSwitchBeatmapEventDataValue");
					
					light.ProcessLightSwitchEvent(prevEventData, true);
					
				}
				
			}
			
		}
		
		public void Update() {
			
			if (disableOn != -1f && Time.time > disableOn) {
				
				SetLightMode(LightMode.Default);
				
				disableOn = -1f;
				
			}
			
			if (disableBoostOn != -1f && Time.time > disableBoostOn) {
				
				boostColors = false;
				
				UpdateLightMode();
				
				disableBoostOn = -1f;
				
			}
			
			if (disableOn == -1f && disableBoostOn == -1f)
				enabled = false;
			
		}
		
		public void OnDestroy() {
			
			foreach (LightEffectController light in customLights)
				bocc.beatmapEventDidTriggerEvent -= light.OnEvent;
			
			foreach (LightEffectController light in lightshowLights)
				CustomBeatmapEventManager.onCustomBeatmapEvent -= light.OnEvent;
			
		}
		
	}
	
}