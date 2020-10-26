using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using MonoBehavior = UnityEngine.MonoBehaviour;
using RotationStepType = TrackLaneRingsRotationEffectSpawner.RotationStepType;

namespace TwitchFX.Lights {
	
	public class RingRotationEffectController : MonoBehavior {
		
		public static RingRotationEffectController Create(TrackLaneRingsRotationEffectSpawner baseEffect) {
			
			BeatmapEventType eventTypeForThisEffect = Helper.GetValue<BeatmapEventType>(baseEffect, "_beatmapEventType");
			RotationStepType stepType = Helper.GetValue<RotationStepType>(baseEffect, "_rotationStepType");
			float stepMax = Helper.GetValue<float>(baseEffect, "_rotationStep");
			float rotation = Helper.GetValue<float>(baseEffect, "_rotation");
			int propagationSpeed = Helper.GetValue<int>(baseEffect, "_rotationPropagationSpeed");
			float flexySpeed = Helper.GetValue<float>(baseEffect, "_rotationFlexySpeed");
			
			TrackLaneRingsRotationEffect rotationEffect = Helper.GetValue<TrackLaneRingsRotationEffect>(baseEffect, "_trackLaneRingsRotationEffect");
			
			TrackLaneRingsManager manager = Helper.GetValue<TrackLaneRingsManager>(rotationEffect, "_trackLaneRingsManager");
			
			float startupAngle = Helper.GetValue<float>(rotationEffect, "_startupRotationAngle");
			float startupStep = Helper.GetValue<float>(rotationEffect, "_startupRotationStep");
			int startupPropagationSpeed = Helper.GetValue<int>(rotationEffect, "_startupRotationPropagationSpeed");
			float startupFlexySpeed = Helper.GetValue<float>(rotationEffect, "_startupRotationFlexySpeed");
			
			RotationEffect startupRotationEffect = new RotationEffect
			{
				progress = 0,
				angle = startupAngle,
				step = startupStep,
				propagationSpeed = startupPropagationSpeed,
				flexySpeed = startupFlexySpeed
			};
			
			RingRotationEffectController controller = new GameObject("TwitchFXRingRotationController").AddComponent<RingRotationEffectController>();
			
			controller.eventTypeForThisEffect = eventTypeForThisEffect;
			controller.manager = manager;
			controller.name = baseEffect.name;
			controller.isBig = baseEffect.name.Contains("Big");
			controller.stepType = stepType;
			controller.stepMax = stepMax;
			controller.startupRotationEffect = startupRotationEffect;
			controller.rotation = rotation;
			controller.propagationSpeed = propagationSpeed;
			controller.flexySpeed = flexySpeed;
			
			return controller;
			
		}
		
		private BeatmapEventType eventTypeForThisEffect;
		
		private TrackLaneRingsManager manager;
		
		private new string name;
		private bool isBig;
		
		private RotationStepType stepType;
		private float stepMax;
		
		private RotationEffect startupRotationEffect;
		
		private float rotation;
		private int propagationSpeed;
		private float flexySpeed;
		
		private List<RotationEffect> activeRotationEffects = new List<RotationEffect>(20);
		
		public void Start() {
			
			activeRotationEffects.Add(startupRotationEffect);
			
			startupRotationEffect = null;
			
		}
		
		public void OnEvent(BeatmapEventData eventData) {
			
			if (eventData.type != eventTypeForThisEffect)
				return;
			
			CustomBeatmapEventData customEventData = eventData as CustomBeatmapEventData;
			
			string nameFilter = customEventData?.ringsNameFilter;
			
			if (nameFilter != null && !nameFilter.Equals(name, StringComparison.OrdinalIgnoreCase))
				return;
			
			float direction = -(customEventData?.direction ?? (Random.value > 0.5f ? 1f : -1f));
			bool counterSpin = customEventData?.ringsCounterSpin ?? false;
			bool reset = customEventData?.ringsReset ?? false;
			
			if (counterSpin && isBig)
				direction = -direction;
			
			float step;
			
			switch (stepType) {
			case RotationStepType.Range0ToMax:
				step = Random.Range(0f, stepMax);
				break;
			case RotationStepType.Range:
				step = Random.Range(-stepMax, stepMax);
				break;
			case RotationStepType.MaxOr0:
				step = Random.value > 0.5f ? stepMax : 0f;
				break;
			default:
				step = 0f;
				break;
			}
			
			step = customEventData?.ringsStep ?? step;
			float propagationSpeed = customEventData?.ringsPropagationSpeed ?? this.propagationSpeed;
			float flexySpeed = customEventData?.ringsFlexySpeed ?? this.flexySpeed;
			
			float? stepMultiplier = customEventData?.ringsStepMultiplier;
			float? propagationSpeedMultiplier = customEventData?.ringsPropagationSpeedMultiplier;
			float? flexySpeedMultiplier = customEventData?.ringsFlexySpeedMultiplier;
			
			if (stepMultiplier.HasValue)
				step *= stepMultiplier.Value;
			
			if (propagationSpeedMultiplier.HasValue)
				propagationSpeed *= propagationSpeedMultiplier.Value;
			
			if (flexySpeedMultiplier.HasValue)
				flexySpeed *= flexySpeedMultiplier.Value;
			
			if (reset) {
				
				step = 0;
				propagationSpeed = 50;
				flexySpeed = 50;
				
			}
			
			RotationEffect rotationEffect = new RotationEffect {
				progress = 0,
				angle = manager.Rings[0].GetDestinationRotation() + rotation * direction,
				step = step,
				propagationSpeed = propagationSpeed,
				flexySpeed = flexySpeed
			};
			
			activeRotationEffects.Add(rotationEffect);
			
		}
		
		public void FixedUpdate() {
			
			TrackLaneRing[] rings = manager.Rings;
			
			for (int i = activeRotationEffects.Count - 1; i >= 0; i--) {
				
				RotationEffect rotation = activeRotationEffects[i];
				
				for (int j = (int) rotation.progress; j < rotation.progress + rotation.propagationSpeed && j < rings.Length; j++)
					rings[j].SetDestRotation(
						rotation.angle + j * rotation.step,
						rotation.flexySpeed
					);
				
				rotation.progress += rotation.propagationSpeed;
				
				if (rotation.progress >= rings.Length)
					activeRotationEffects.RemoveAt(i);
				
			}
			
		}
		
		private class RotationEffect {
			
			public float progress;
			public float angle;
			public float step;
			public float flexySpeed;
			public float propagationSpeed;
			
		}
		
	}
	
}