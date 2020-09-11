using ChatCore;
using ChatCore.Interfaces;
using ChatCore.Services.Twitch;

namespace TwitchFX {
	
	public class Chat {
		
		private readonly TwitchService twitchService;
		
		public Chat() {
			
			ChatCoreInstance chatCore = ChatCoreInstance.Create();
			
			twitchService = chatCore.RunTwitchServices();
			
			twitchService.OnTextMessageReceived += OnMessage;
			
		}
		
		private void OnMessage(IChatService service, IChatMessage message) {
			
			if (message.Message.StartsWith("!")) {
				
				int nameLength = message.Message.IndexOf(' ');
				
				string name = message.Message.Substring(1, nameLength - 1);
				string args = message.Message.Substring(nameLength + 1);
				
				Command.GetCommand(name)?.Execute(args);
				
			}
			
		}
		
		public void Send(string message) {
			
			foreach (IChatChannel channel in twitchService.Channels.Values) {
				
				twitchService.SendTextMessage(message, channel);
				
			}
			
		}
		
	}
	
}