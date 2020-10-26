using System;
using UnityEngine;
using Zenject;

namespace TwitchFX.Lights {
	
	public class RingController : LazyController<RingController> {
		
		private event Action<BeatmapEventData> onPipedBeatmapEvent;
		
		private BeatmapObjectCallbackController bocc;
		
		private TrackLaneRingsRotationEffectSpawner[] rotationEffects;
		private TrackLaneRingsPositionStepEffectSpawner[] positionStepEffects;
		
		private RingRotationEffectController[] rotationControllers;
		
		private bool disablePipedBeatmapEvents = false;
		
		[Inject]
		public void Inject(
			BeatmapObjectCallbackController bocc,
			TrackLaneRingsRotationEffectSpawner[] rotationEffects,
			TrackLaneRingsPositionStepEffectSpawner[] positionStepEffects
		) {
			
			this.bocc = bocc;
			this.rotationEffects = rotationEffects;
			this.positionStepEffects = positionStepEffects;
			
		}
		
		protected override void Init() {
			
			if (rotationEffects.Length == 0 && positionStepEffects.Length == 0)
				return;
			
			bocc.beatmapEventDidTriggerEvent += OnBeatmapEvent;
			
			rotationControllers = new RingRotationEffectController[rotationEffects.Length];
			
			for (int i = 0; i < rotationEffects.Length; i++) {
				
				TrackLaneRingsRotationEffectSpawner rotationEffect = rotationEffects[i];
				
				onPipedBeatmapEvent += rotationEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				bocc.beatmapEventDidTriggerEvent -= rotationEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				
				RingRotationEffectController rotationController = RingRotationEffectController.Create(rotationEffect);
				CustomBeatmapEventManager.onCustomBeatmapEvent += rotationController.OnEvent;
				
				rotationControllers[i] = rotationController;
				
			}
			
			foreach (TrackLaneRingsPositionStepEffectSpawner positionStepEffect in positionStepEffects) {
				
				onPipedBeatmapEvent += positionStepEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				bocc.beatmapEventDidTriggerEvent -= positionStepEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				
				CustomBeatmapEventManager.onCustomBeatmapEvent += positionStepEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				
			}
			
		}
		
		public void Spin() {
			
			onPipedBeatmapEvent?.Invoke(new BeatmapEventData(0f, BeatmapEventType.Event8, 0));
			
		}
		
		public void EnableDefaultRingEvents() {
			
			disablePipedBeatmapEvents = false;
			
		}
		
		public void DisableDefaultRingEvents() {
			
			disablePipedBeatmapEvents = true;
			
		}
		
		private void OnBeatmapEvent(BeatmapEventData eventData) {
			
			if (!disablePipedBeatmapEvents)
				onPipedBeatmapEvent?.Invoke(eventData);
			
		}
		
		public void OnDestroy() {
			
			if (bocc != null)
				bocc.beatmapEventDidTriggerEvent -= OnBeatmapEvent;
			
			foreach (RingRotationEffectController rotationController in rotationControllers)
				CustomBeatmapEventManager.onCustomBeatmapEvent -= rotationController.OnEvent;
			
			foreach (TrackLaneRingsPositionStepEffectSpawner positionStepEffect in positionStepEffects)
				CustomBeatmapEventManager.onCustomBeatmapEvent -= positionStepEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
			
		}
		
	}
	
}