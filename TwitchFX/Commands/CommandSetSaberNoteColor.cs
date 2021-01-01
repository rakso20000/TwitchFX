using TwitchFX.Colors;
using UnityEngine;

namespace TwitchFX.Commands {
	
	public class CommandSetSaberNoteColor : Command {
		
		public CommandSetSaberNoteColor() {
			
			usage = "<left color> <right color> OR\n" +
			"<left color> <right color> <duration in seconds>";
			
			SetArgsCount(2, 3);
			
		}
		
		protected override void Execute(string[] args) {
			
			Color leftColor = ParseColor(args[0]);
			Color rightColor = ParseColor(args[1]);
			
			float? duration = TryParseFloat(args, 2);
			
			ColorController.instance.SetSaberColors(leftColor, rightColor, duration);
			ColorController.instance.SetNoteColors(leftColor, rightColor, duration);
			
		}
		
	}
	
}