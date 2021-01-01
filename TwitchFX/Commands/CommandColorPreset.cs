using TwitchFX.Colors;
using TwitchFX.Lights;

namespace TwitchFX.Commands {
	
	public class CommandColorPreset : Command {
		
		public CommandColorPreset() {
			
			usage = "<preset> OR\n" +
			"<preset> <duration in seconds>";
			
			SetArgsCount(1, 2);
			
		}
		
		protected override void Execute(string[] args) {
			
			string name = args[0];
			
			float? duration = TryParseFloat(args, 1);
			
			ColorPreset preset = ColorPreset.GetColorPreset(name);
			
			if (preset == null) {
				
				Plugin.chat.Send("Preset with name " + name + " not found");
				
				return;
				
			}
			
			if (preset.leftLightColor.HasValue || preset.rightLightColor.HasValue) {
				
				LightController.instance.SetColors(preset.leftLightColor, preset.rightLightColor);
				LightController.instance.SetLightMode(LightMode.Custom, duration);
				
			}
			
			if (preset.leftNoteColor.HasValue || preset.rightNoteColor.HasValue)
				ColorController.instance.SetNoteColors(preset.leftNoteColor, preset.rightNoteColor, duration);
			
			if (preset.leftSaberColor.HasValue || preset.rightSaberColor.HasValue)
				ColorController.instance.SetSaberColors(preset.leftSaberColor, preset.rightSaberColor, duration);
			
			if (preset.wallColor.HasValue)
				ColorController.instance.SetWallColor(preset.wallColor.Value, duration);
			
		}
		
	}
	
}