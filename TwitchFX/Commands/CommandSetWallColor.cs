using UnityEngine;

namespace TwitchFX.Commands {
	
	public class CommandSetWallColor : Command {
		
		public override void Execute(string argsStr) {
			
			SetUsage("<color> OR\n" +
			"<color> <duration in seconds>");
			
			string[] args = ParseArgs(argsStr);
			
			if (args.Length < 1 || args.Length > 2) {
				
				PrintUsage();
				
				return;
				
			}
			
			Color? color = ParseColor(args[0]);
			
			if (!color.HasValue) {
				
				PrintUsage();
				
				return;
				
			}
			
			float duration = 0f;
			
			if (args.Length >= 2 && !float.TryParse(args[1], out duration)) {
				
				PrintUsage();
				
				return;
				
			}
			
			if (ColorController.instance == null) {
				
				Plugin.chat.Send("Please use this command during a song");
				
				return;
				
			}
			
			ColorController.instance.SetWallColor(color.Value);
			
			if (args.Length >= 2) {
				
				ColorController.instance.DisableWallColorIn(duration);
				
			}
			
		}
		
	}
	
}