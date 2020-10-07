namespace TwitchFX.Commands {
	
	public class CommandResetWallColor : Command {
		
		public override void Execute(string argsStr) {
			
			ParseArgs(argsStr, 0);
			
			ColorController.instance?.DisableWallColor();
			
		}
		
	}
	
}