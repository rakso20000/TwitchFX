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
			
			Logger.log.Info(message.Sender.DisplayName + " said " + message.Message);
			
			//message for test purposes
			Send("Hello " + message.Sender.DisplayName);
			
		}
		
		public void Send(string message) {
			
			foreach (IChatChannel channel in twitchService.Channels.Values) {
				
				twitchService.SendTextMessage(message, channel);
				
			}
			
		}
		
	}
	
}