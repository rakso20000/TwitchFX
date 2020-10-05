using UnityEngine;

namespace TwitchFX {
	
	public class ColorController : MonoBehaviour {
		
		public static ColorController instance { get; private set; }
		
		public bool useCustomNoteColors { get; private set; } = false;
		public Color noteColorLeft { get; private set; }
		public Color noteColorRight { get; private set; }
		
		private float disableNoteColorsOn = -1f;
		
		public void Awake() {
			
			if (instance != null) {
				
				Logger.log.Warn("Instance of LightController already exists, destroying.");
				
				Destroy(this);
				
				return;
				
			}
			
			instance = this;
			
			enabled = false;
			
		}
		
		public void SetNoteColors(Color leftColor, Color rightColor) {
			
			noteColorLeft = leftColor;
			noteColorRight = rightColor;
			
			useCustomNoteColors = true;
			
		}
		
		public void DisableNoteColorsIn(float duration) {
			
			disableNoteColorsOn = Time.time + duration;
			
			enabled = true;
			
		}
		
		public void DisableNoteColors() {
			
			useCustomNoteColors = false;
			
		}
		
		public void Update() {
			
			if (disableNoteColorsOn != -1f && Time.time > disableNoteColorsOn) {
				
				useCustomNoteColors = false;
				
				disableNoteColorsOn = -1f;
				
				enabled = false;
				
			}
			
		}
		
		public void OnDestroy() {
			
			if (instance == this)
				instance = null;
			
		}
		
	}
	
}