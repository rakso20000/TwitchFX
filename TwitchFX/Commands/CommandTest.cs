namespace TwitchFX.Commands {
	
	public class CommandTest : Command {
		
		public override void Execute(string args) {
			
			Plugin.chat.Send("Test: " + args);
			
		}
		
	}
	
}