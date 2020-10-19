using System;
using UnityEngine;

namespace TwitchFX.Lights {
	
	public class RingController : LazyController<RingController> {
		
		private event Action<BeatmapEventData> onPipedOrCustomBeatmapEvent;
		
		private BeatmapObjectCallbackController bocc;
		
		private bool disablePipedBeatmapEvents = false;
		
		protected override void Init() {
			
			TrackLaneRingsRotationEffectSpawner[] rotationEffects = Resources.FindObjectsOfTypeAll<TrackLaneRingsRotationEffectSpawner>();
			TrackLaneRingsPositionStepEffectSpawner[] positionStepEffects = Resources.FindObjectsOfTypeAll<TrackLaneRingsPositionStepEffectSpawner>();
			
			object someEffect = rotationEffects.Length != 0 ? (object) rotationEffects[0] : positionStepEffects.Length != 0 ? positionStepEffects[0] : null;
			
			if (someEffect == null)
				return;
			
			bocc = Helper.GetValue<BeatmapObjectCallbackController>(someEffect, "_beatmapObjectCallbackController");
			
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
			
			if (bocc != null)
				bocc.beatmapEventDidTriggerEvent -= OnBeatmapEvent;
			
			CustomBeatmapEventManager.onCustomBeatmapEvent -= OnCustomBeatmapEvent;
			
		}
		
	}
	
}