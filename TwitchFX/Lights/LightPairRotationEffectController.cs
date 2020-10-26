using UnityEngine;
using MonoBehavior = UnityEngine.MonoBehaviour;

namespace TwitchFX.Lights {
	
	public class LightPairRotationEffectController : MonoBehavior {
		
		public static LightPairRotationEffectController Create(LightPairRotationEventEffect baseEffect) {
			
			BeatmapEventType eventToggleRandomize = Helper.GetValue<BeatmapEventType>(baseEffect, "_switchOverrideRandomValuesEvent");
			BeatmapEventType eventL = Helper.GetValue<BeatmapEventType>(baseEffect, "_eventL");
			BeatmapEventType eventR = Helper.GetValue<BeatmapEventType>(baseEffect, "_eventR");
			
			bool useZOffset = Helper.GetValue<bool>(baseEffect, "_useZPositionForAngleOffset");
			float zOffsetScale = Helper.GetValue<float>(baseEffect, "_zPositionAngleOffsetScale");
			
			Vector3 rotationVector = Helper.GetValue<Vector3>(baseEffect, "_rotationVector");
			
			object baseRotationDataL = Helper.GetValue<object>(baseEffect, "_rotationDataL");
			object baseRotationDataR = Helper.GetValue<object>(baseEffect, "_rotationDataR");
			
			Transform transformL = Helper.GetValue<Transform>(baseRotationDataL, "transform");
			Quaternion startRotationL = Helper.GetValue<Quaternion>(baseRotationDataL, "startRotation");
			float startRotationAngleL = Helper.GetValue<float>(baseRotationDataL, "startRotationAngle");
			
			Transform transformR = Helper.GetValue<Transform>(baseRotationDataR, "transform");
			Quaternion startRotationR = Helper.GetValue<Quaternion>(baseRotationDataR, "startRotation");
			float startRotationAngleR = Helper.GetValue<float>(baseRotationDataR, "startRotationAngle");
			
			RotationData rotationDataL = new RotationData(transformL, startRotationL, startRotationAngleL);
			RotationData rotationDataR = new RotationData(transformR, startRotationR, startRotationAngleR);
			
			LightPairRotationEffectController controller = new GameObject("TwitchFXLightPairRotationEffectController").AddComponent<LightPairRotationEffectController>();
			
			controller.eventToggleRandomize = eventToggleRandomize;
			controller.eventL = eventL;
			controller.eventR = eventR;
			controller.transform = baseEffect.transform;
			controller.useZOffset = useZOffset;
			controller.zOffsetScale = zOffsetScale;
			controller.rotationVector = rotationVector;
			controller.rotationL = rotationDataL;
			controller.rotationR = rotationDataR;
			
			return controller;
			
		}
		
		private BeatmapEventType eventToggleRandomize;
		private BeatmapEventType eventL;
		private BeatmapEventType eventR;
		
		private new Transform transform;
		
		private bool useZOffset;
		private float zOffsetScale;
		
		private Vector3 rotationVector;
		
		private RotationData rotationL;
		private RotationData rotationR;
		
		private float startRotationGenerated;
		private float directionGenerated;
		
		private bool randomizeValues = true;
		private int lastValueGenerationFrame = -1;
		
		public void Awake() {
			
			enabled = false;
			
		}
		
		public void Reset() {
			
			rotationL.enabled = false;
			rotationR.enabled = false;
			enabled = false;
			
			rotationL.transform.localRotation = rotationL.startRotation * Quaternion.Euler(rotationVector * rotationL.startRotationAngle);
			rotationR.transform.localRotation = rotationR.startRotation * Quaternion.Euler(rotationVector * rotationR.startRotationAngle);
			
		}
		
		public void OnEvent(BeatmapEventData eventData) {
			
			if (eventData.type == eventToggleRandomize)
				randomizeValues = !randomizeValues;
			
			int currentFrame = Time.frameCount;
			
			if (
				(eventData.type == eventL ||
				eventData.type == eventR ||
				eventData.type == eventToggleRandomize) &&
				currentFrame != lastValueGenerationFrame
			) {
				
				if (randomizeValues) {
					
					startRotationGenerated = Random.Range(0f, 180f);
					directionGenerated = Random.value > 0.5f ? 1f : -1f;
					
				} else {
					
					startRotationGenerated = eventData.type == eventL ? currentFrame : -currentFrame;
					
					if (useZOffset)
						startRotationGenerated += transform.position.z * zOffsetScale;
					
					directionGenerated = eventData.type == eventL ? 1f : -1f;
					
				}
				
				lastValueGenerationFrame = currentFrame;
				
			}
			
			if (eventData.type == eventL || eventData.type == eventR) {
				
				CustomBeatmapEventData customEventData = eventData as CustomBeatmapEventData;
				
				float direction = customEventData?.direction ?? directionGenerated;
				float speed = customEventData?.rotationSpeed ?? eventData.value;
				bool lockPosition = customEventData?.rotationLockPosition ?? false;
				float? startPosition = customEventData?.rotationStartPosition;
				
				if (eventData.type != eventL)
					direction = -direction;
				
				RotationData rotation = eventData.type == eventL ? rotationL : rotationR;
				
				if (startPosition.HasValue) {
					
					rotation.rotationAngle = (eventData.type == eventL ? startPosition.Value : -startPosition.Value) + rotation.startRotationAngle;
					
					rotation.transform.localRotation = rotation.startRotation * Quaternion.Euler(rotationVector * rotation.rotationAngle);
					
				}
				
				if (eventData.value == 0) {
					
					rotation.enabled = false;
					
					if (!lockPosition && !startPosition.HasValue)
						rotation.transform.localRotation = rotation.startRotation * Quaternion.Euler(rotationVector * rotation.startRotationAngle);
					
				} else if (eventData.value > 0) {
					
					rotation.enabled = true;
					
					rotation.rotationSpeed = speed * direction * 20f;
					
					if (!lockPosition && !startPosition.HasValue) {
						
						rotation.rotationAngle = (eventData.type == eventL ? startRotationGenerated : -startRotationGenerated) + rotation.startRotationAngle;
						
						rotation.transform.localRotation = rotation.startRotation * Quaternion.Euler(rotationVector * rotation.rotationAngle);
						
					}
					
				}
				
			} else if (eventData.type == eventToggleRandomize) {
				
				rotationL.rotationAngle = startRotationGenerated + rotationL.startRotationAngle;
				rotationL.rotationSpeed = Mathf.Abs(rotationL.rotationSpeed);
				rotationR.rotationAngle = -startRotationGenerated + rotationR.startRotationAngle;
				rotationR.rotationSpeed = -Mathf.Abs(rotationL.rotationSpeed);
				
			}
			
			enabled = rotationL.enabled || rotationR.enabled;
			
		}
		
		public void Update() {
			
			if (rotationL.enabled) {
				
				rotationL.rotationAngle += rotationL.rotationSpeed * Time.deltaTime;
				rotationL.transform.localRotation = rotationL.startRotation * Quaternion.Euler(rotationVector * rotationL.rotationAngle);
				
			}
			
			if (rotationR.enabled) {
				
				rotationR.rotationAngle += rotationR.rotationSpeed * Time.deltaTime;
				rotationR.transform.localRotation = rotationR.startRotation * Quaternion.Euler(rotationVector * rotationR.rotationAngle);
				
			}
			
		}
		
		private class RotationData {
			
			public readonly Transform transform;
			
			public readonly Quaternion startRotation;
			public readonly float startRotationAngle;
			
			public bool enabled = false;
			
			public float rotationSpeed;
			public float rotationAngle;
			
			public RotationData(Transform transform, Quaternion startRotation, float startRotationAngle) {
				
				this.transform = transform;
				this.startRotation = startRotation;
				this.startRotationAngle = startRotationAngle;
				
			}
			
		}
		
	}
	
}