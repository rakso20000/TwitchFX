using TwitchFX.Colors;
using UnityEngine;
using MonoBehavior = UnityEngine.MonoBehaviour;

namespace TwitchFX.Lights {
	
	public class LightEffectController : MonoBehavior {
		
		private const float FADE_SPEED = 2f;
		
		private const float NORMAL_ALPHA = 0.5490196f;
		private const float HIGHLIGHT_ALPHA = 0.7529412f;
		
		private static readonly Color offColor = new Color(0f, 0f, 0f, 0f);
		
		public static LightEffectController Create(
			LightWithIdManagerWrapper lightManager,
			LightMode activeOnMode,
			LightSwitchEventEffect baseLight,
			IAudioTimeSource timeSource
		) {
			
			int id = Helper.GetValue<int>(baseLight, "_lightsID");
			BeatmapEventType eventTypeForThisLight = Helper.GetValue<BeatmapEventType>(baseLight, "_event");
			
			int lastEventValue = Helper.GetValue<int>(baseLight, "_prevLightSwitchBeatmapEventDataValue");
			
			LightEffectController controller = new GameObject("TwitchFXLightEffectController").AddComponent<LightEffectController>();
			
			controller.timeSource = timeSource;
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
		
		private IAudioTimeSource timeSource;
		
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
		
		private float gradientStartTime;
		private ColorGradient? gradient = null;
		
		private BeatmapEventData lastEventData;
		private float transitionValue;
		
		private bool isLeftActive;
		private bool isRightActive;
		
		public void Awake() {
			
			enabled = false;
			
		}
		
		public void Reset() {
			
			transitionValue = 0f;
			gradient = null;
			
			enabled = false;
			
		}
		
		public void OnEvent(BeatmapEventData eventData) {
			
			if (eventData.type == eventTypeForThisLight)
				HandleEvent(eventData, true);
			
		}
		
		public void SetColors(Color leftColor, Color rightColor) {
			
			rainbowLeft = Helper.IsRainbow(leftColor);
			rainbowRight = Helper.IsRainbow(rightColor);
			
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
			
			if (
				executeEvent &&
				eventData is CustomBeatmapEventData customEventData
			) {
				
				gradientStartTime = timeSource.songTime;
				gradient = customEventData.colorGradient;
				
				isLeftActive = false;
				isRightActive = false;
				
			} else {
				
				isLeftActive = eventData.value >= 4;
				isRightActive = !isLeftActive;
				
			}
			
			if (gradient.HasValue || rainbowLeft || rainbowRight)
				enabled = true;
			
		}
		
		public void Update() {
			
			Color color;
			
			if (gradient.HasValue) {
				
				ColorGradient grad = gradient.Value;
				
				color = Color.Lerp(grad.endColor, grad.startColor, (timeSource.songTime - gradientStartTime) / grad.duration);
				
				if (timeSource.songTime - gradientStartTime >= grad.duration)
					gradient = null;
				
			} else {
				
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
				
				if ((isLeftActive && rainbowLeft) || (isRightActive && rainbowRight)) {
					
					float alpha = color.a;
					
					if (isLeftActive)
						color = RainbowController.instance.GetLeftColor();
					else
						color = RainbowController.instance.GetRightColor();
					
					color = color.ColorWithAlpha(alpha);
					
				}
				
			}
			
			SetColor(color);
			
			if (
				transitionValue == 0f &&
				!gradient.HasValue &&
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
			
			if (eventData is CustomBeatmapEventData customEventData && customEventData.color.HasValue)
				return customEventData.color.Value;
			
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