using UnityEngine;

namespace TwitchFX.Commands {
	
	public class CommandSetNoteColor : Command {
		
		public override void Execute(string argsStr) {
			
			SetUsage("<left color> <right color> OR\n" +
			"<left color> <right color> <duration in seconds>");
			
			string[] args = ParseArgs(argsStr);
			
			if (args.Length < 2 || args.Length > 3)
				throw CreateInvalidArgs();
			
			Color leftColor = ParseColor(args[0]);
			Color rightColor = ParseColor(args[1]);
			
			float duration = 0f;
			
			if (args.Length >= 3 && !float.TryParse(args[2], out duration))
				throw CreateInvalidArgs();
			
			if (ColorController.instance == null) {
				
				Plugin.chat.Send("Please use this command during a song");
				
				return;
				
			}
			
			ColorController.instance.SetNoteColors(leftColor, rightColor);
			
			if (args.Length >= 3) {
				
				ColorController.instance.DisableNoteColorsIn(duration);
				
			}
			
		}
		
	}
	
}