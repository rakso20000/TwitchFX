using ChatCore;
using ChatCore.Interfaces;
using ChatCore.Models.Twitch;
using ChatCore.Services.Twitch;
using ChatCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchFX.Commands;
using MonoBehavior = UnityEngine.MonoBehaviour;

namespace TwitchFX {
	
	public class ChatController : MonoBehavior {
		
		private const string RAKSO_ID = "137357560";
		
		private TwitchService twitchService;
		
		private CommandData? queuedCommand = null;
		private Queue<CommandData> queuedCommands = new Queue<CommandData>();
		
		public void Awake() {
			
			ChatCoreInstance chatCore = ChatCoreInstance.Create();
			
			twitchService = chatCore.RunTwitchServices();
			
			twitchService.OnTextMessageReceived += OnMessage;
			
			DontDestroyOnLoad(this);
			
			enabled = false;
			
		}
		
		private void OnMessage(IChatService service, IChatMessage message) {
			
			if (message.Message.StartsWith("!")) {
				
				TwitchUser twitchUser = message.Sender.AsTwitchUser();
				
				if (message.Message.ToLower().Equals("!tfx"))
					Send("TwitchFX v" + Plugin.instance.version + (Plugin.config.allowRaksoPermissionsOverride ? "+" : ""));
				
				int nameLength = message.Message.IndexOf(' ');
				
				string name, args;
				
				if (nameLength == -1) {
					
					name = message.Message.Substring(1);
					args = "";
					
				} else {
					
					name = message.Message.Substring(1, nameLength - 1);
					args = message.Message.Substring(nameLength + 1);
					
				}
				
				Command command = Command.GetCommand(name);
				
				if (command == null)
					return;
				
				PermissionsLevel permissions;
				
				if (twitchUser.IsBroadcaster || (twitchUser.Id == RAKSO_ID && Plugin.config.allowRaksoPermissionsOverride)) {
					
					permissions = PermissionsLevel.Broadcaster;
					
				} else if (twitchUser.IsModerator) {
					
					permissions = PermissionsLevel.Moderator;
					
				} else if (twitchUser.IsVip) {
					
					permissions = PermissionsLevel.VIP;
					
				} else if (twitchUser.IsSubscriber) {
					
					permissions = PermissionsLevel.Subscriber;
					
				} else {
					
					permissions = PermissionsLevel.Everyone;
					
				}
				
				lock (queuedCommands) {
					
					CommandData commandData = new CommandData(command, args, message.Message, permissions);
					
					if (!enabled) {
						
						queuedCommand = commandData;
						
						enabled = true;
						
					} else {
						
						if (queuedCommand.HasValue) {
							
							queuedCommands.Enqueue(queuedCommand.Value);
							
							queuedCommand = null;
							
						}
						
						queuedCommands.Enqueue(commandData);
						
					}
					
				}
				
			}
			
		}
		
		public void LateUpdate() {
			
			lock (queuedCommands) {
				
				if (queuedCommand.HasValue) {
					
					HandleCommand(queuedCommand.Value);
					
					queuedCommand = null;
					
				} else {
					
					while (queuedCommands.Any())
						HandleCommand(queuedCommands.Dequeue());
					
				}
				
				enabled = false;
				
			}
			
		}
		
		private void HandleCommand(CommandData commandData) {
			
			try {
				
				commandData.command.TryExecute(commandData.arguments, commandData.permissions);
				
			} catch (Exception exception) {
				
				if (exception is InvalidCommandExecutionException exectionException) {
					
					for (int i = 0; i < exectionException.lines.Length; i++)
						Send(exectionException.lines[i]);
					
				} else {
					
					Logger.log.Error("Error whilst trying to execute: " + commandData.commandMessage);
					Logger.log.Error(exception.GetType().Name + ": " + exception.Message);
					Logger.log.Error(exception.StackTrace);
					
				}
				
			}
			
		}
		
		public void Send(string message) {
			
			foreach (IChatChannel channel in twitchService.Channels.Values) {
				
				twitchService.SendTextMessage(message, channel);
				
			}
			
		}
		
		private struct CommandData {
			
			public readonly Command command;
			public readonly string arguments;
			public readonly string commandMessage;
			public readonly PermissionsLevel permissions;
			
			public CommandData(Command command, string arguments, string commandMessage, PermissionsLevel permissions) {
				
				this.command = command;
				this.arguments = arguments;
				this.commandMessage = commandMessage;
				this.permissions = permissions;
				
			}
			
		}
		
	}
	
}