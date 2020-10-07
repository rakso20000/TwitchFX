using UnityEngine;

namespace TwitchFX.Commands {
	
	public class CommandSetWallColor : Command {
		
		public override void Execute(string argsStr) {
			
			SetUsage("<color> OR\n" +
			"<color> <duration in seconds>");
			
			string[] args = ParseArgs(argsStr);
			
			if (args.Length < 1 || args.Length > 2)
				throw CreateInvalidArgs();
			
			Color color = ParseColor(args[0]);
			
			float duration = 0f;
			
			if (args.Length >= 2 && !float.TryParse(args[1], out duration))
				throw CreateInvalidArgs();
			
			if (ColorController.instance == null) {
				
				Plugin.chat.Send("Please use this command during a song");
				
				return;
				
			}
			
			ColorController.instance.SetWallColor(color);
			
			if (args.Length >= 2) {
				
				ColorController.instance.DisableWallColorIn(duration);
				
			}
			
		}
		
	}
	
}