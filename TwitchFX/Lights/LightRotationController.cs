using System;
using UnityEngine;
using Zenject;

namespace TwitchFX.Lights {
	
	public class LightRotationController : LazyController<LightRotationController> {
		
		private event Action<BeatmapEventData> onPipedOrCustomBeatmapEvent;
		
		private BeatmapObjectCallbackController bocc;
		
		private LightRotationEventEffect[] rotationEffects;
		private LightPairRotationEventEffect[] pairRotationEffects;
		
		private bool disablePipedBeatmapEvents = false;
		
		[Inject]
		public void Inject(
			BeatmapObjectCallbackController bocc,
			LightRotationEventEffect[] rotationEffects,
			LightPairRotationEventEffect[] pairRotationEffects
		) {
			
			this.bocc = bocc;
			this.rotationEffects = rotationEffects;
			this.pairRotationEffects = pairRotationEffects;
			
		}
		
		protected override void Init() {
			
			if (rotationEffects.Length == 0 && pairRotationEffects.Length == 0)
				return;
			
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