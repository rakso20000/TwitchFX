using TwitchFX.Colors;
using UnityEngine;
using MonoBehavior = UnityEngine.MonoBehaviour;

namespace TwitchFX.Lights {
	
	public class LightEffectController : MonoBehavior {
		
		private const float FADE_SPEED = 2f;
		
		private const float NORMAL_ALPHA = 0.5490196f;
		private const float HIGHLIGHT_ALPHA = 0.7529412f;
		
		private static readonly Color offColor = new Color(0f, 0f, 0f, 0f);
		
		public static LightEffectController CreateLightEffectController(
			LightWithIdManagerWrapper lightManager,
			LightMode activeOnMode,
			LightSwitchEventEffect baseLight
		) {
			
			int id = Helper.GetValue<int>(baseLight, "_lightsID");
			BeatmapEventType eventTypeForThisLight = Helper.GetValue<BeatmapEventType>(baseLight, "_event");
			
			int lastEventValue = Helper.GetValue<int>(baseLight, "_prevLightSwitchBeatmapEventDataValue");
			
			LightEffectController controller = new GameObject("TwitchFXLightEffectController").AddComponent<LightEffectController>();
			
			controller.lightManager = lightManager;
			controller.activeOnMode = activeOnMode;
			controller.id = id;
			controller.eventTypeForThisLight = eventTypeForThisLight;
			
			controller.lastEventData = new BeatmapEventData(0f, eventTypeForThisLight, lastEventValue);
			
			controller.enabled = baseLight.enabled;
			
			if (baseLight.enabled) {
				
				float highlightValue = Helper.GetValue<float>(baseLight, "_highlightValue");
				
				controller.transitionValue = highlightValue;
				
				controller.startColor = offColor;
				controller.endColor = offColor;
				
			}
			
			return controller;
			
		}
		
		private LightWithIdManagerWrapper lightManager;
		
		private LightMode activeOnMode;
		private int id;
		private BeatmapEventType eventTypeForThisLight;
		
		private LightMode mode = LightMode.Default;
		
		private Color colorLeft;
		private Color colorRight;
		private Color highlightcolorLeft;
		private Color highlightcolorRight;
		
		private bool rainbowLeft = false;
		private bool rainbowRight = false;
		
		private Color startColor;
		private Color endColor;
		
		private BeatmapEventData lastEventData;
		private float transitionValue;
		
		private bool isLeftActive;
		
		public void Awake() {
			
			enabled = false;
			
		}
		
		public void OnEvent(BeatmapEventData eventData) {
			
			if (eventData.type == eventTypeForThisLight)
				HandleEvent(eventData, true);
			
		}
		
		public void SetColors(Color leftColor, Color rightColor) {
			
			if (Helper.IsRainbow(leftColor))
				rainbowLeft = true;
			
			if (Helper.IsRainbow(rightColor))
				rainbowRight = true;
			
			colorLeft = leftColor.ColorWithAlpha(NORMAL_ALPHA);
			colorRight = rightColor.ColorWithAlpha(NORMAL_ALPHA);
			
			highlightcolorLeft = leftColor.ColorWithAlpha(HIGHLIGHT_ALPHA);
			highlightcolorRight = rightColor.ColorWithAlpha(HIGHLIGHT_ALPHA);
			
			if (rainbowLeft || rainbowRight)
				enabled = true;
			
		}
		
		public void UpdateLightMode(LightMode mode) {
			
			this.mode = mode;
			
			HandleEvent(lastEventData, false);
			
		}
		
		private void HandleEvent(BeatmapEventData eventData, bool executeEvent) {
			
			switch (eventData.value) {
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
				
				SetColor(GetColorForEvent(eventData, false));
				
				break;
			//flash
			case 2:
			case 6:
				
				if (executeEvent) {
					
					transitionValue = 1f;
					enabled = true;
					
				}
				
				startColor = GetColorForEvent(eventData, true);
				endColor = GetColorForEvent(eventData, false);
				
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
				
				startColor = GetColorForEvent(eventData, true);
				endColor = startColor.ColorWithAlpha(0f);
				
				SetColor(Color.Lerp(endColor, startColor, transitionValue));
				
				break;
			}
			
			lastEventData = eventData;
			
			isLeftActive = eventData.value >= 4;
			
			if (rainbowLeft || rainbowRight)
				enabled = true;
			
		}
		
		public void Update() {
			
			Color color;
			
			if (transitionValue > 0f) {
				
				color = Color.Lerp(endColor, startColor, transitionValue);
				
				transitionValue = Mathf.Lerp(transitionValue, 0f, Time.deltaTime * FADE_SPEED);
				
				if (transitionValue < 0.0001f) {
					
					transitionValue = 0f;
					enabled = false;
					
					color = endColor;
					
				}
				
			} else {
				
				color = colorLeft;
				
			}
			
			if ((isLeftActive && rainbowLeft) || (!isLeftActive && rainbowRight)) {
				
				float alpha = color.a;
				
				if (isLeftActive)
					color = RainbowController.instance.GetLeftColor();
				else
					color = RainbowController.instance.GetRightColor();
				
				color = color.ColorWithAlpha(alpha);
				
			}
			
			SetColor(color);
			
			if (
				transitionValue == 0f &&
				!rainbowLeft &&
				!rainbowRight
			)
				enabled = false;
			
		}
		
		private void SetColor(Color color) {
			
			if (mode != activeOnMode)
				return;
			
			lightManager.SetCustomColorForId(id, color);
			
		}
		
		private Color GetColorForEvent(BeatmapEventData eventData, bool highlight) {
			
			if (eventData is CustomBeatmapEventData customEventData)
				return customEventData.color;
			
			if (eventData.value >= 4 && rainbowLeft)
				return RainbowController.instance.GetLeftColor().ColorWithAlpha(highlight ? HIGHLIGHT_ALPHA : NORMAL_ALPHA);
			
			if (eventData.value < 4 && rainbowRight)
				return RainbowController.instance.GetRightColor().ColorWithAlpha(highlight ? HIGHLIGHT_ALPHA : NORMAL_ALPHA);
			
			if (highlight)
				return eventData.value >= 4 ? highlightcolorLeft : highlightcolorRight;
			
			return eventData.value >= 4 ? colorLeft : colorRight;
			
		}
		
	}
	
}