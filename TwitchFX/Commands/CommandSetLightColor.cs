using TwitchFX.Lights;
using UnityEngine;

namespace TwitchFX.Commands {
	
	public class CommandSetLightColor : Command {
		
		public CommandSetLightColor() {
			
			usage = "<color> OR\n" +
			"<left color> <right color> OR\n" +
			"<left color> <right color> <duration in seconds>";
			
			SetArgsCount(1, 3);
			
		}
		
		protected override void Execute(string[] args) {
			
			Color leftColor = ParseColor(args[0]);
			Color rightColor = args.Length > 1 ? ParseColor(args[1]) : leftColor;
			
			float? duration = TryParseFloat(args, 2);
			
			LightController.instance.SetColors(leftColor, rightColor);
			LightController.instance.SetLightMode(LightMode.Custom, duration);
			
		}
		
	}
	
}