using UnityEngine;
using MonoBehavior = UnityEngine.MonoBehaviour;

namespace TwitchFX.Lights {
	
	public class LightRotationEffectController : MonoBehavior {
		
		public static LightRotationEffectController Create(LightRotationEventEffect baseEffect) {
			
			BeatmapEventType eventTypeForThisEffect = Helper.GetValue<BeatmapEventType>(baseEffect, "_event");
			Transform transform = Helper.GetValue<Transform>(baseEffect, "_transform");
			Quaternion startRotation = Helper.GetValue<Quaternion>(baseEffect, "_startRotation");
			Vector3 rotationVector = Helper.GetValue<Vector3>(baseEffect, "_rotationVector");
			
			LightRotationEffectController controller = new GameObject("TwitchFXLightRotationEffectController").AddComponent<LightRotationEffectController>();
			
			controller.eventTypeForThisEffect = eventTypeForThisEffect;
			controller.transform = transform;
			controller.startRotation = startRotation;
			controller.rotationVector = rotationVector;
			
			return controller;
			
		}
		
		private BeatmapEventType eventTypeForThisEffect;
		
		private new Transform transform;
		
		private Quaternion startRotation;
		private Vector3 rotationVector;
		
		private float rotationSpeed;
		
		public void Awake() {
			
			enabled = false;
			
		}
		
		public void Reset() {
			
			enabled = false;
			
			transform.localRotation = startRotation;
			
		}
		
		public void OnEvent(BeatmapEventData eventData) {
			
			if (eventData.type != eventTypeForThisEffect)
				return;
			
			CustomBeatmapEventData customEventData = eventData as CustomBeatmapEventData;
			
			float speed = customEventData?.rotationSpeed ?? eventData.value;
			bool lockPosition = customEventData?.rotationLockPosition ?? false;
			float direction = customEventData?.rotationDirection ?? (Random.value > 0.5f ? 1f : -1f);
			
			if (eventData.type != BeatmapEventType.Event12)
				direction = -direction;
			
			if (eventData.value == 0) {
				
				enabled = false;
				
				if (!lockPosition)
					transform.localRotation = startRotation;
				
			} else if (eventData.value > 0) {
				
				enabled = true;
				
				if (!lockPosition) {
					
					transform.localRotation = startRotation;
					transform.Rotate(rotationVector, Random.Range(0f, 180f), Space.Self);
					
				}
				
				rotationSpeed = speed * direction * 20f;
				
			}
			
		}
		
		public void Update() {
			
			transform.Rotate(rotationVector, rotationSpeed * Time.deltaTime);
			
		}
		
	}
	
}