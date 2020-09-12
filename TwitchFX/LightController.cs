using UnityEngine;

namespace TwitchFX {
	
	class LightController : MonoBehaviour {
		
		public static LightController instance { get; private set; }
		
		public float disableOn = -1f;
		
		private ConfigurableColorSO colorLeft;
		private ConfigurableColorSO colorRight;
		private ConfigurableColorSO highlightcolorLeft;
		private ConfigurableColorSO highlightcolorRight;
		
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
			
			colorRight = ScriptableObject.CreateInstance<ConfigurableColorSO>();
			colorLeft = ScriptableObject.CreateInstance<ConfigurableColorSO>();
			highlightcolorRight = ScriptableObject.CreateInstance<ConfigurableColorSO>();
			highlightcolorLeft = ScriptableObject.CreateInstance<ConfigurableColorSO>();
			
			Color offColor = Helper.GetValue<Color>(ligt, "_offColor");
			
			colorRight.Init(Helper.GetValue<ColorSO>(ligt, "_lightColor0"), offColor);
			colorLeft.Init(Helper.GetValue<ColorSO>(ligt, "_lightColor1"), offColor);
			highlightcolorRight.Init(Helper.GetValue<ColorSO>(ligt, "_highlightColor0"), offColor);
			highlightcolorLeft.Init(Helper.GetValue<ColorSO>(ligt, "_highlightColor1"), offColor);
			
			foreach(LightSwitchEventEffect light in lights) {
				
				Helper.SetValue<ColorSO>(light, "_lightColor0", colorRight);
				Helper.SetValue<ColorSO>(light, "_lightColor1", colorLeft);
				Helper.SetValue<ColorSO>(light, "_highlightColor0", highlightcolorRight);
				Helper.SetValue<ColorSO>(light, "_highlightColor1", highlightcolorLeft);
				
			}
			
		}
		
		public void SetLeftColor(Color color) {
			
			colorLeft.SetColor(color);
			highlightcolorLeft.SetColor(color);
			
		}
		
		public void SetRightColor(Color color) {
			
			colorRight.SetColor(color);
			highlightcolorRight.SetColor(color);
			
		}
		
		public void UpdateLights(ColorMode mode) {
			
			colorLeft.SetMode(mode);
			colorRight.SetMode(mode);
			highlightcolorLeft.SetMode(mode);
			highlightcolorRight.SetMode(mode);
			
			foreach (LightSwitchEventEffect light in lights) {
				
				int oldEventData = Helper.GetValue<int>(light, "_prevLightSwitchBeatmapEventDataValue");
				int newEventData;
				
				switch (oldEventData) {
				case 0: //off
					return;
				case 1: //on
				case 5: //on
					newEventData = oldEventData;
					break;
				case 2: //flash
				case 6: //flash
					newEventData = oldEventData - 1;
					break;
				case 3: //fade
				case 7: //fade
				case -1: //fade
					newEventData = 0;
					break;
				default:
					return;
				}
				
				light.ProcessLightSwitchEvent(newEventData, false);
				
			}
			
		}
		
		private void Update() {
			
			if (disableOn != -1f && Time.time > disableOn) {
				
				disableOn = -1f;
				
				colorLeft.SetMode(ColorMode.Default);
				colorRight.SetMode(ColorMode.Default);
				highlightcolorLeft.SetMode(ColorMode.Default);
				highlightcolorRight.SetMode(ColorMode.Default);
				
			}
			
		}
		
		private void OnDestroy() {
			
			if (instance == this)
				instance = null;
			
		}
		
	}
	
}