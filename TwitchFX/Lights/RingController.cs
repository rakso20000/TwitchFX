using System;
using UnityEngine;
using Zenject;

namespace TwitchFX.Lights {
	
	public class RingController : LazyController<RingController> {
		
		private event Action<BeatmapEventData> onPipedOrCustomBeatmapEvent;
		
		private BeatmapObjectCallbackController bocc;
		
		private TrackLaneRingsRotationEffectSpawner[] rotationEffects;
		private TrackLaneRingsPositionStepEffectSpawner[] positionStepEffects;
		
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
			
			bocc.beatmapEventDidTriggerEvent += OnBeatmapEvent;
			CustomBeatmapEventManager.onCustomBeatmapEvent += OnCustomBeatmapEvent;
			
			foreach (TrackLaneRingsRotationEffectSpawner rotationEffect in rotationEffects) {
				
				onPipedOrCustomBeatmapEvent += rotationEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				bocc.beatmapEventDidTriggerEvent -= rotationEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				
			}
			
			foreach (TrackLaneRingsPositionStepEffectSpawner positionStepEffect in positionStepEffects) {
				
				onPipedOrCustomBeatmapEvent += positionStepEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				bocc.beatmapEventDidTriggerEvent -= positionStepEffect.HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger;
				
			}
			
		}
		
		public void Spin() {
			
			onPipedOrCustomBeatmapEvent?.Invoke(new BeatmapEventData(0f, BeatmapEventType.Event8, 0));
			
		}
		
		public void EnableDefaultRingEvents() {
			
			disablePipedBeatmapEvents = false;
			
		}
		
		public void DisableDefaultRingEvents() {
			
			disablePipedBeatmapEvents = true;
			
		}
		
		private void OnBeatmapEvent(BeatmapEventData eventData) {
			
			if (!disablePipedBeatmapEvents)
				onPipedOrCustomBeatmapEvent?.Invoke(eventData);
			
		}
		
		private void OnCustomBeatmapEvent(BeatmapEventData eventData) {
			
			onPipedOrCustomBeatmapEvent?.Invoke(eventData);
			
		}
		
		public void OnDestroy() {
			
			bocc.beatmapEventDidTriggerEvent -= OnBeatmapEvent;
			CustomBeatmapEventManager.onCustomBeatmapEvent -= OnCustomBeatmapEvent;
			
		}
		
	}
	
}