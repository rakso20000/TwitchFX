using UnityEngine;

namespace TwitchFX.Commands {
	
	public class CommandSetLightColor : Command {
		
		public override void Execute(string argsStr) {
			
			SetUsage("<color> OR\n" +
			"<left color> <right color> OR\n" +
			"<left color> <right color> <duration in seconds>");
			
			string[] args = ParseArgs(argsStr);
			
			if (args.Length < 1 || args.Length > 3) {
				
				PrintUsage();
				
				return;
				
			}
			
			Color? leftColorNullable = ParseColor(args[0]);
			Color? rightColorNullable = args.Length > 1 ? ParseColor(args[1]) : leftColorNullable;
			
			if (leftColorNullable == null || rightColorNullable == null) {
				
				PrintUsage();
				
				return;
				
			}
			
			float duration = 0f;
			
			if(args.Length >= 3 && !float.TryParse(args[2], out duration)) {
				
				PrintUsage();
				
				return;
				
			}
			
			if (LightController.instance == null) {
				
				Plugin.chat.Send("Please use this command during a song");
				
				return;
				
			}
			
			Color leftColor = (Color) leftColorNullable;
			Color rightColor = (Color) rightColorNullable;
			
			LightController.instance.SetColors(leftColor, rightColor);
			LightController.instance.UpdateLights(ColorMode.Custom);
			
			if (args.Length >= 3) {
				
				LightController.instance.DisableIn(duration);
				
			} else {
				
				LightController.instance.CancelDisable();
				
			}
			
		}
		
	}
	
}