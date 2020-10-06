using UnityEngine;

namespace TwitchFX.Commands {
	
	public class CommandSetSaberColor : Command {
		
		public override void Execute(string argsStr) {
			
			SetUsage("<left color> <right color> OR\n" +
			"<left color> <right color> <duration in seconds>");
			
			string[] args = ParseArgs(argsStr);
			
			if (args.Length < 2 || args.Length > 3) {
				
				PrintUsage();
				
				return;
				
			}
			
			Color? leftColor = ParseColor(args[0]);
			Color? rightColor = ParseColor(args[1]);
			
			if (!leftColor.HasValue || !rightColor.HasValue) {
				
				PrintUsage();
				
				return;
				
			}
			
			float duration = 0f;
			
			if (args.Length >= 3 && !float.TryParse(args[2], out duration)) {
				
				PrintUsage();
				
				return;
				
			}
			
			if (ColorController.instance == null) {
				
				Plugin.chat.Send("Please use this command during a song");
				
				return;
				
			}
			
			ColorController.instance.SetSaberColors(leftColor.Value, rightColor.Value);
			
			if (args.Length >= 3) {
				
				ColorController.instance.DisableSaberColorsIn(duration);
				
			}
			
		}
		
	}
	
}