namespace TwitchFX.Commands {
	
	public class CommandHelpTFX : Command {
		
		public CommandHelpTFX() {
			
			requireEnabled = false;
			requireInLevel = false;
			
		}
		
		protected override void Execute(string[] args) {
			
			Plugin.chat.Send("For a list of available TwitchFX commands, please refer to https://github.com/rakso20000/TwitchFX#commands");
			
		}
		
	}
	
}