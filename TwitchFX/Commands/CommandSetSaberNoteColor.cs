using TwitchFX.Colors;
using UnityEngine;

namespace TwitchFX.Commands {
	
	public class CommandSetSaberNoteColor : Command {
		
		public override void Execute(string argsStr) {
			
			RequireInLevel();
			
			SetUsage("<left color> <right color> OR\n" +
			"<left color> <right color> <duration in seconds>");
			
			string[] args = ParseArgs(argsStr, 2, 3);
			
			Color leftColor = ParseColor(args[0]);
			Color rightColor = ParseColor(args[1]);
			
			float? duration = TryParseFloat(args, 2);
			
			ColorController.instance.SetSaberColors(leftColor, rightColor, duration);
			ColorController.instance.SetNoteColors(leftColor, rightColor, duration);
			
		}
		
	}
	
}