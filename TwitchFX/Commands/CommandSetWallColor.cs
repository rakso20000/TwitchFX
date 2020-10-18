using UnityEngine;

namespace TwitchFX.Commands {
	
	public class CommandSetWallColor : Command {
		
		public override void Execute(string argsStr) {
			
			SetUsage("<color> OR\n" +
			"<color> <duration in seconds>");
			
			string[] args = ParseArgs(argsStr, 1, 2);
			
			Color color = ParseColor(args[0]);
			
			float duration = TryParseFloat(args, 1);
			
			if (!Plugin.instance.inLevel) {
				
				Plugin.chat.Send("Please use this command during a song");
				
				return;
				
			}
			
			ColorController.instance.SetWallColor(color);
			
			if (args.Length >= 2)
				ColorController.instance.DisableWallColorIn(duration);
			
		}
		
	}
	
}