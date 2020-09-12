using UnityEngine;

namespace TwitchFX.Commands {
	
	public class CommandSetLightColor : Command {
		
		public override void Execute(string argsStr) {
			
			SetUsage("!setlightcolor <color> OR\n" +
			"!setlightcolor <left color> <right color> OR\n" +
			"!setlightcolor <left color> <right color> <duration in seconds>");
			
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
				
			}
			
			Color leftColor = (Color) leftColorNullable;
			Color rightColor = (Color) rightColorNullable;
			
			LightController.instance.colorLeft.SetColor(leftColor);
			LightController.instance.colorRight.SetColor(rightColor);
			LightController.instance.highlightcolorLeft.SetColor(leftColor);
			LightController.instance.highlightcolorRight.SetColor(rightColor);
			
			LightController.instance.colorLeft.Enable();
			LightController.instance.colorRight.Enable();
			LightController.instance.highlightcolorLeft.Enable();
			LightController.instance.highlightcolorRight.Enable();
			
			if (args.Length >= 3)
				LightController.instance.disableOn = Time.time + duration;
			
		}
		
	}
	
}