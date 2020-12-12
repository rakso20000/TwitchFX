using System;

namespace TwitchFX.Commands {
	
	public class InvalidCommandExecutionException : Exception {
		
		public readonly string[] lines;
		
		public InvalidCommandExecutionException(string message) {
			
			lines = new string[] { message };
			
		}
		
		public InvalidCommandExecutionException(string[] lines) {
			
			this.lines = lines;
			
		}
		
	}
	
}