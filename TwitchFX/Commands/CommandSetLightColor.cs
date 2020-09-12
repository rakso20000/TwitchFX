using UnityEngine;

namespace TwitchFX.Commands {
	
	public class CommandSetLightColor : Command {
		
		public override void Execute(string argsStr) {
			
			SetUsage("!setlightcolor <left color> <right color>");
			
			string[] args = ParseArgs(argsStr, 2);
			
			if (args == null)
				return;
			
			Color? leftColorNullable = ParseColor(args[0]);
			Color? rightColorNullable = ParseColor(args[1]);
			
			if (leftColorNullable == null || rightColorNullable == null) {
				
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
			
		}
		
	}
	
}