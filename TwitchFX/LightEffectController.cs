using UnityEngine;

namespace TwitchFX {
	
	public class LightEffectController : MonoBehaviour {
		
		private const float FADE_SPEED = 2f;
		
		private static readonly Color offColor = new Color(0f, 0f, 0f, 0f);
		
		public static LightEffectController CreateLightEffectController(
			LightWithIdManagerWrapper lightManager,
			int id,
			BeatmapEventType eventTypeForThisLight,
			BeatmapObjectCallbackController beatmapObjectCallbackController
		) {
			
			LightEffectController controller = new GameObject("TwitchFXLightEffectController").AddComponent<LightEffectController>();
			
			controller.lightManager = lightManager;
			controller.id = id;
			controller.eventTypeForThisLight = eventTypeForThisLight;
			controller.beatmapObjectCallbackController = beatmapObjectCallbackController;
			
			return controller;
			
		}
		
		private LightWithIdManagerWrapper lightManager;
		
		private int id;
		private BeatmapEventType eventTypeForThisLight;
		private BeatmapObjectCallbackController beatmapObjectCallbackController;
		
		private ColorMode mode = ColorMode.Default;
		
		private Color colorLeft;
		private Color colorRight;
		private Color highlightcolorLeft;
		private Color highlightcolorRight;
		
		private Color startColor;
		private Color endColor;
		
		private int lastEventValue;
		private float transitionValue;
		
		public void Start() {
			
			beatmapObjectCallbackController.beatmapEventDidTriggerEvent += OnEvent;
			
			enabled = false;
			
		}
		
		public void OnDestroy() {
			
			beatmapObjectCallbackController.beatmapEventDidTriggerEvent -= OnEvent;
			
		}
		
		private void OnEvent(BeatmapEventData eventData) {
			
			if (eventData.type == eventTypeForThisLight)
				HandleEvent(eventData.value, true);
			
		}
		
		public void SetColors(Color leftColor, Color rightColor) {
			
			colorLeft = leftColor.ColorWithAlpha(0.5490196f);
			colorRight = rightColor.ColorWithAlpha(0.5490196f);
			
			highlightcolorLeft = leftColor.ColorWithAlpha(0.7529412f);
			highlightcolorRight = rightColor.ColorWithAlpha(0.7529412f);
			
		}
		
		public void UpdateColors(ColorMode mode) {
			
			this.mode = mode;
			
			HandleEvent(lastEventValue, false);
			
		}
		
		private void HandleEvent(int eventValue, bool executeEvent) {
			
			switch (eventValue) {
			//off
			case 0:
				
				transitionValue = 0f;
				enabled = false;
				
				SetColor(offColor);
				
				break;
			//on
			case 1:
			case 5:
				
				transitionValue = 0f;
				enabled = false;
				
				SetColor(GetColorForEvent(eventValue, false));
				
				break;
			//flash
			case 2:
			case 6:
				
				if (executeEvent) {
					
					transitionValue = 1f;
					enabled = true;
					
				}
				
				startColor = GetColorForEvent(eventValue, true);
				endColor = GetColorForEvent(eventValue, false);
				
				SetColor(Color.Lerp(endColor, startColor, transitionValue));
				
				break;
			//fade
			case 3:
			case 7:
			case -1:
				
				if (executeEvent) {
					
					transitionValue = 1f;
					enabled = true;
					
				}
				
				startColor = GetColorForEvent(eventValue, true);
				endColor = startColor.ColorWithAlpha(0f);
				
				SetColor(Color.Lerp(endColor, startColor, transitionValue));
				
				break;
			}
			
			lastEventValue = eventValue;
			
		}
		
		public void Update() {
			
			SetColor(Color.Lerp(endColor, startColor, transitionValue));
			
			transitionValue = Mathf.Lerp(transitionValue, 0f, Time.deltaTime * FADE_SPEED);
			
			if (transitionValue < 0.0001f) {
				
				transitionValue = 0f;
				enabled = false;
				
				SetColor(endColor);
				
			}
			
		}
		
		private void SetColor(Color color) {
			
			switch (mode) {
			case ColorMode.Custom:
				lightManager.SetCustomColorForId(id, color);
				break;
			case ColorMode.Disabled:
				lightManager.SetCustomColorForId(id, offColor);
				break;
			}
			
		}
		
		private Color GetColorForEvent(int eventValue, bool highlight) {
			
			if (eventValue >= 4)
				return highlight ? highlightcolorLeft : colorLeft;
			else
				return highlight ? highlightcolorRight : colorRight;
			
		}
		
	}
	
}