using TwitchFX.Colors;
using TwitchFX.Lights;

namespace TwitchFX.Commands {
	
	public class CommandColorPreset : Command {
		
		public override void Execute(string argsStr) {
			
			RequireInLevel();
			
			SetUsage("<preset> OR\n" +
			"<preset> <duration in seconds>");
			
			string[] args = ParseArgs(argsStr, 1, 2);
			string name = args[0];
			
			float? duration = TryParseFloat(args, 1);
			
			ColorPreset preset = ColorPreset.GetColorPreset(name);
			
			if (preset == null) {
				
				Plugin.chat.Send("Preset with name " + name + " not found");
				
				return;
				
			}
			
			LightController.instance.SetColors(preset.leftLightColor, preset.rightLightColor);
			LightController.instance.SetLightMode(LightMode.Custom, duration);
			
			ColorController.instance.SetNoteColors(preset.leftNoteColor, preset.rightNoteColor, duration);
			ColorController.instance.SetSaberColors(preset.leftSaberColor, preset.rightSaberColor, duration);
			ColorController.instance.SetWallColor(preset.wallColor, duration);
			
		}
		
	}
	
}