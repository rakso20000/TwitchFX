using TwitchFX.Colors;
using UnityEngine;

namespace TwitchFX.Commands {
	
	public class CommandSetSaberColor : Command {
		
		public override void Execute(string argsStr) {
			
			SetUsage("<left color> <right color> OR\n" +
			"<left color> <right color> <duration in seconds>");
			
			string[] args = ParseArgs(argsStr, 2, 3);
			
			Color leftColor = ParseColor(args[0]);
			Color rightColor = ParseColor(args[1]);
			
			float duration = TryParseFloat(args, 2);
			
			if (!Plugin.instance.inLevel) {
				
				Plugin.chat.Send("Please use this command during a song");
				
				return;
				
			}
			
			ColorController.instance.SetSaberColors(leftColor, rightColor);
			
			if (args.Length >= 3)
				ColorController.instance.DisableSaberColorsIn(duration);
			
		}
		
	}
	
}