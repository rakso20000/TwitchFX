using UnityEngine;

namespace TwitchFX {
	
	class LightController : MonoBehaviour {
		
		public static LightController instance { get; private set; }
		
		public bool overrideLights { get; private set; }
		
		public float disableOn = -1f;
		
		private LightWithIdManagerWrapper managerWrapper;
		
		private LightSwitchEventEffect[] lights;
		
		private void Awake() {
			
			if (instance != null) {
				
				Logger.log.Warn("Instance of LightController already exists, destroying.");
				
				Destroy(this);
				
				return;
				
			}
			
			instance = this;
			
		}
		
		private void Start() {
			
			lights = Resources.FindObjectsOfTypeAll<LightSwitchEventEffect>();
			
			LightSwitchEventEffect ligt = lights[0];
			managerWrapper = new LightWithIdManagerWrapper(Helper.GetValue<LightWithIdManager>(ligt, "_lightManager"));
			
			foreach(LightSwitchEventEffect light in lights) {
				
				Helper.SetValue<LightWithIdManagerWrapper>(light, "_lightManager", managerWrapper);
				
			}
			
		}
		
		public void SetLeftColor(Color color) {
			
		}
		
		public void SetRightColor(Color color) {
			
		}
		
		public void UpdateLights(ColorMode mode) {
			
			overrideLights = mode != ColorMode.Default;
			
		}
		
		private void Update() {
			
			if (disableOn != -1f && Time.time > disableOn) {
				
				disableOn = -1f;
				
			}
			
		}
		
		private void OnDestroy() {
			
			if (instance == this)
				instance = null;
			
		}
		
	}
	
}