using TwitchFX.Colors;
using UnityEngine;

namespace TwitchFX.Commands {
	
	public class CommandSetWallColor : Command {
		
		public override void Execute(string argsStr) {
			
			RequireInLevel();
			
			SetUsage("<color> OR\n" +
			"<color> <duration in seconds>");
			
			string[] args = ParseArgs(argsStr, 1, 2);
			
			Color color = ParseColor(args[0]);
			
			float? duration = TryParseFloat(args, 1);
			
			ColorController.instance.SetWallColor(color, duration);
			
		}
		
	}
	
}