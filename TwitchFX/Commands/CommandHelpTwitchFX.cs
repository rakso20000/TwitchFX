namespace TwitchFX.Commands {
	
	public class CommandHelpTwitchFX : Command {
		
		public override void Execute(string argsStr) {
			
			ParseArgs(argsStr, 0);
			
			Plugin.chat.Send("For a list of available TwitchFX commands, please refer to https://github.com/rakso20000/TwitchFX#commands");
			
		}
		
	}
	
}