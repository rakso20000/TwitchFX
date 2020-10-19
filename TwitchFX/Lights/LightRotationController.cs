using System;
using UnityEngine;

namespace TwitchFX.Lights {
	
	public class LightRotationController : LazyController<LightRotationController> {
		
		private event Action<BeatmapEventData> onPipedOrCustomBeatmapEvent;
		
		private BeatmapObjectCallbackController bocc;
		
		private bool disablePipedBeatmapEvents = false;
		
		protected override void Init() {
			
			LightRotationEventEffect[] rotationEffects = Resources.FindObjectsOfTypeAll<LightRotationEventEffect>();
			LightPairRotationEventEffect[] pairRotationEffects = Resources.FindObjectsOfTypeAll<LightPairRotationEventEffect>();
			
			object someEffect = rotationEffects.Length != 0 ? (object) rotationEffects[0] : pairRotationEffects.Length != 0 ? pairRotationEffects[0] : null;
			
			if (someEffect == null)
				return;
			
			bocc = Helper.GetValue<BeatmapObjectCallbackController>(someEffect, "_beatmapObjectCallbackController");
			
			bocc.beatmapEventDidTriggerEvent += OnBeatmapEvent;
			CustomBeatmapEventManager.onCustomBeatmapEvent += OnCustomBeatmapEvent;
			
			foreach (LightRotationEventEffect rotationEffect in rotationEffects) {
				
				onPipedOrCustomBeatmapEvent += rotationEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				bocc.beatmapEventDidTriggerEvent -= rotationEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				
			}
			
			foreach (LightPairRotationEventEffect pairRotationEffect in pairRotationEffects) {
				
				object rotationDataL = Helper.GetValue<object>(pairRotationEffect, "_rotationDataL");
				object rotationDataR = Helper.GetValue<object>(pairRotationEffect, "_rotationDataR");
				
				if (rotationDataL == null || rotationDataR == null)
					continue;
				
				onPipedOrCustomBeatmapEvent += pairRotationEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				bocc.beatmapEventDidTriggerEvent -= pairRotationEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				
			}
			
		}
		
		public void EnableDefaultLightRotationEvents() {
			
			disablePipedBeatmapEvents = false;
			
		}
		
		public void DisableDefaultLightRotationEvents() {
			
			disablePipedBeatmapEvents = true;
			
		}
		
		public void ResetLightRotation() {
			
			onPipedOrCustomBeatmapEvent?.Invoke(new BeatmapEventData(0f, BeatmapEventType.Event12, 0));
			onPipedOrCustomBeatmapEvent?.Invoke(new BeatmapEventData(0f, BeatmapEventType.Event13, 0));
			
		}
		
		private void OnBeatmapEvent(BeatmapEventData eventData) {
			
			if (!disablePipedBeatmapEvents)
				onPipedOrCustomBeatmapEvent?.Invoke(eventData);
			
		}
		
		private void OnCustomBeatmapEvent(BeatmapEventData eventData) {
			
			onPipedOrCustomBeatmapEvent?.Invoke(eventData);
			
		}
		
		public void OnDestroy() {
			
			if (bocc != null)
				bocc.beatmapEventDidTriggerEvent -= OnBeatmapEvent;
			
			CustomBeatmapEventManager.onCustomBeatmapEvent -= OnCustomBeatmapEvent;
			
		}
		
	}
	
}