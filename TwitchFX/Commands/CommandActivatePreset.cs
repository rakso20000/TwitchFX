namespace TwitchFX.Commands {
	
	public class CommandActivatePreset : Command {
		
		public override void Execute(string argsStr) {
			
			SetUsage("<preset> OR\n" +
			"<preset> <duration in seconds>");
			
			string[] args = ParseArgs(argsStr, 1, 2);
			string name = args[0];
			
			float duration = TryParseFloat(args, 1);
			
			if (!Plugin.instance.inLevel) {
				
				Plugin.chat.Send("Please use this command during a song");
				
				return;
				
			}
			
			ColorPreset preset = ColorPreset.GetColorPreset(name);
			
			if (preset == null) {
				
				Plugin.chat.Send("Preset with name " + name + " not found");
				
				return;
				
			}
			
			LightController.instance.SetColors(preset.leftLightColor, preset.rightLightColor);
			LightController.instance.SetColorMode(ColorMode.Custom);
			
			ColorController.instance.SetNoteColors(preset.leftNoteColor, preset.rightNoteColor);
			ColorController.instance.SetSaberColors(preset.leftSaberColor, preset.rightSaberColor);
			ColorController.instance.SetWallColor(preset.wallColor);
			
			if (args.Length >= 2) {
				
				LightController.instance.DisableIn(duration);
				ColorController.instance.DisableNoteColorsIn(duration);
				ColorController.instance.DisableSaberColorsIn(duration);
				ColorController.instance.DisableWallColorIn(duration);
				
			}
			
		}
		
	}
	
}