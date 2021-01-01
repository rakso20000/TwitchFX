using TwitchFX.Colors;
using UnityEngine;

namespace TwitchFX.Commands {
	
	public class CommandSetWallColor : Command {
		
		public CommandSetWallColor() {
			
			usage = "<color> OR\n" +
			"<color> <duration in seconds>";
			
			SetArgsCount(1, 2);
			
		}
		
		protected override void Execute(string[] args) {
			
			Color color = ParseColor(args[0]);
			
			float? duration = TryParseFloat(args, 1);
			
			ColorController.instance.SetWallColor(color, duration);
			
		}
		
	}
	
}