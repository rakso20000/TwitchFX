using System;
using UnityEngine;
using Zenject;

namespace TwitchFX.Lights {
	
	public class LightRotationController : LazyController<LightRotationController> {
		
		private event Action<BeatmapEventData> onPipedBeatmapEvent;
		
		private BeatmapObjectCallbackController bocc;
		
		private LightRotationEventEffect[] rotationEffects;
		private LightPairRotationEventEffect[] pairRotationEffects;
		
		private LightRotationEffectController[] rotationEffectControllers;
		private LightPairRotationEffectController[] pairRotationEffectControllers;

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
			
			rotationEffectControllers = new LightRotationEffectController[rotationEffects.Length];
			pairRotationEffectControllers = new LightPairRotationEffectController[pairRotationEffects.Length];
			
			for (int i = 0; i < rotationEffects.Length; i++) {
				
				LightRotationEventEffect rotationEffect = rotationEffects[i];
				
				onPipedBeatmapEvent += rotationEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				bocc.beatmapEventDidTriggerEvent -= rotationEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				
				LightRotationEffectController rotationEffectController = LightRotationEffectController.Create(rotationEffect);
				CustomBeatmapEventManager.onCustomBeatmapEvent += rotationEffectController.OnEvent;
				
				rotationEffectControllers[i] = rotationEffectController;
				
			}
			
			for (int i = 0; i < pairRotationEffects.Length; i++) {
				
				LightPairRotationEventEffect pairRotationEffect = pairRotationEffects[i];
				
				onPipedBeatmapEvent += pairRotationEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				bocc.beatmapEventDidTriggerEvent -= pairRotationEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				
				LightPairRotationEffectController pairRotationEffectController = LightPairRotationEffectController.Create(pairRotationEffect);
				CustomBeatmapEventManager.onCustomBeatmapEvent += pairRotationEffectController.OnEvent;
				
				pairRotationEffectControllers[i] = pairRotationEffectController;
				
			}
			
		}
		
		public void EnableDefaultLightRotationEvents() {
			
			disablePipedBeatmapEvents = false;
			
		}
		
		public void DisableDefaultLightRotationEvents() {
			
			disablePipedBeatmapEvents = true;
			
			foreach (LightRotationEventEffect rotationEffect in rotationEffects)
				rotationEffect.enabled = false;
			
			foreach (LightPairRotationEventEffect pairRotationEffect in pairRotationEffects)
				pairRotationEffect.enabled = false;
			
		}
		
		public void ResetCustomLightRotation() {
			
			foreach (LightRotationEffectController rotationEffectController in rotationEffectControllers)
				rotationEffectController.Reset();
			
			foreach (LightPairRotationEffectController pairRotationEffectController in pairRotationEffectControllers)
				pairRotationEffectController.Reset();
			
		}
		
		private void OnBeatmapEvent(BeatmapEventData eventData) {
			
			if (!disablePipedBeatmapEvents)
				onPipedBeatmapEvent?.Invoke(eventData);
			
		}
		
		public void OnDestroy() {
			
			if (bocc != null)
				bocc.beatmapEventDidTriggerEvent -= OnBeatmapEvent;
			
			foreach (LightRotationEffectController rotationEffectController in rotationEffectControllers)
				CustomBeatmapEventManager.onCustomBeatmapEvent -= rotationEffectController.OnEvent;
			
			foreach (LightPairRotationEffectController pairRotationEffectController in pairRotationEffectControllers)
				CustomBeatmapEventManager.onCustomBeatmapEvent -= pairRotationEffectController.OnEvent;
			
		}
		
	}
	
}