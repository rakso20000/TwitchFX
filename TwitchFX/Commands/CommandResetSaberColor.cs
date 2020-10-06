namespace TwitchFX.Commands {
	
	public class CommandResetSaberColor : Command {
		
		public override void Execute(string argsStr) {
			
			ParseArgs(argsStr, 0);
			
			ColorController.instance?.DisableSaberColors();
			
		}
		
	}
	
}