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
			
			LightRotationEventEffect rotatinEffect = rotationEffects[0];
			
			bocc = Helper.GetValue<BeatmapObjectCallbackController>(rotatinEffect, "_beatmapObjectCallbackController");
			
			bocc.beatmapEventDidTriggerEvent += OnBeatmapEvent;
			CustomBeatmapEventManager.onCustomBeatmapEvent += OnCustomBeatmapEvent;
			
			foreach (LightRotationEventEffect rotationEffect in rotationEffects) {
				
				onPipedOrCustomBeatmapEvent += rotationEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				bocc.beatmapEventDidTriggerEvent -= rotationEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				
			}
			
			foreach (LightPairRotationEventEffect pairRotationEffect in pairRotationEffects) {
				
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
		
	}
	
}