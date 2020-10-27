using TwitchFX.Lights;
using UnityEngine;

namespace TwitchFX.Commands {
	
	public class CommandSetLightColor : Command {
		
		public override void Execute(string argsStr) {
			
			SetUsage("<color> OR\n" +
			"<left color> <right color> OR\n" +
			"<left color> <right color> <duration in seconds>");
			
			string[] args = ParseArgs(argsStr, 1, 3);
			
			Color leftColor = ParseColor(args[0]);
			Color rightColor = args.Length > 1 ? ParseColor(args[1]) : leftColor;
			
			float? duration = TryParseFloat(args, 2);
			
			if (!Plugin.instance.inLevel) {
				
				Plugin.chat.Send("Please use this command during a song");
				
				return;
				
			}
			
			LightController.instance.SetColors(leftColor, rightColor);
			LightController.instance.SetLightMode(LightMode.Custom, duration);
			
		}
		
	}
	
}