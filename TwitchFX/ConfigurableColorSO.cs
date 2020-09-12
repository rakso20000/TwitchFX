using System.Runtime.CompilerServices;
using UnityEngine;

namespace TwitchFX {
	
	public enum ColorMode {
		
		Default,
		Custom,
		Disabled
		
	}
	
	//make chroma happy by using MultipliedColorSO instead of ColorSO
	
	class ConfigurableColorSO : MultipliedColorSO {
		
		private ColorSO baseColorSO;
		private Color offColor;
		
		private ColorMode mode = ColorMode.Default;
		
		private Color customColor;
		
		public void Init(ColorSO baseColorSO, Color offColor) {
			
			this.baseColorSO = baseColorSO;
			this.offColor = offColor;
			customColor = baseColorSO;
			
			//make chroma happy
			
			Helper.SetValue<MultipliedColorSO, SimpleColorSO>(this, "_baseColor", Helper.GetValue<MultipliedColorSO, SimpleColorSO>(baseColorSO, "_baseColor"));
			Helper.SetValue<MultipliedColorSO, Color>(this, "_multiplierColor", Helper.GetValue<MultipliedColorSO, Color>(baseColorSO, "_multiplierColor"));
			
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