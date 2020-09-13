using UnityEngine;

namespace TwitchFX {
	
	public enum ColorMode {
		
		Default,
		Custom,
		Disabled
		
	}
	
	public class ConfigurableColorSO : ColorSO {
		
		private ColorSO baseColorSO;
		private Color offColor;
		
		private ColorMode mode = ColorMode.Default;
		
		private Color customColor;
		
		public void Init(ColorSO baseColorSO, Color offColor) {
			
			this.baseColorSO = baseColorSO;
			this.offColor = offColor;
			customColor = baseColorSO;
			
		}
		
		public void SetMode(ColorMode mode) {
			
			this.mode = mode;
			
		}
		
		public void SetColor(Color color) {
			
			customColor = color;
			
		}
		
		public override Color color {
			
			get {
				
				switch (mode) {
				case ColorMode.Disabled:
					return offColor;
				case ColorMode.Custom:
					return customColor;
				case ColorMode.Default:
				default:
					return baseColorSO;
				}
				
			}
			
		}
		
	}
	
}