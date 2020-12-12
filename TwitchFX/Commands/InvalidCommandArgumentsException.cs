namespace TwitchFX.Commands {
	
	public class InvalidCommandArgumentsException : InvalidCommandExecutionException {
		
		public InvalidCommandArgumentsException(string[] usage) : base(usage) {
			
			lines[0] = "Usage: " + lines[0];
			
		}
		
	}
	
}