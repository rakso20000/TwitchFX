using UnityEngine;

namespace TwitchFX {
	
	class ConfigurableColorSO : ColorSO {
		
		private ColorSO baseColorSO;
		
		private bool enabled = false;
		
		private Color customColor;
		
		public void Init(ColorSO baseColorSO) {
			
			this.baseColorSO = baseColorSO;
			customColor = baseColorSO;
			
		}
		
		public void Enable() {
			
			enabled = true;
			
		}
		
		public void Disable() {
			
			enabled = false;
			
		}
		
		public void SetColor(Color color) {
			
			customColor = color;
			
		}
		
		public override Color color {
			
			get {
				
				if (enabled)
					return customColor;
				
				return baseColorSO.color;
				
			}
			
		}
		
	}
	
}