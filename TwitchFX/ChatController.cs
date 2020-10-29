using ChatCore;
using ChatCore.Interfaces;
using ChatCore.Models.Twitch;
using ChatCore.Services.Twitch;
using ChatCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchFX.Commands;
using TwitchFX.Configuration;
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
				
				if (twitchUser.Id == RAKSO_ID && message.Message.ToLower().Equals("!canoverride"))
					Send(PluginConfig.instance.allowRaksoPermissionsOverride ? "true" : "false");
				
				int nameLength = message.Message.IndexOf(' ');
				
				string name, args;
				
				if (nameLength == -1) {
					
					name = message.Message.Substring(1);
					args = "";
					
				} else {
					
					name = message.Message.Substring(1, nameLength - 1);
					args = message.Message.Substring(nameLength + 1);
					
				}
				
				PermissionsLevel permissions;
				
				if (twitchUser.IsBroadcaster || (twitchUser.Id == RAKSO_ID && PluginConfig.instance.allowRaksoPermissionsOverride)) {
					
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
				
				Command command = Command.GetCommand(name);
				
				if (command == null || !command.CanExecute(permissions))
					return;
				
				lock (queuedCommands) {
					
					CommandData commandData = new CommandData(command, args, message.Message);
					
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
				
				commandData.command.Execute(commandData.arguments);
				
			} catch (Exception e) {
				
				InvalidCommandArgumentsException argsException = e as InvalidCommandArgumentsException;
				
				if (argsException != null) {
					
					Send("Usage: " + argsException.usage[0]);
					
					for (int i = 1; i < argsException.usage.Length; i++)
						Send(argsException.usage[i]);
					
				} else {
					
					Logger.log.Error("Error whilst trying to execute: " + commandData.commandMessage);
					Logger.log.Error(e.GetType().Name + ": " + e.Message);
					Logger.log.Error(e.StackTrace);
					
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
			public string commandMessage;
			
			public CommandData(Command command, string arguments, string commandMessage) {
				
				this.command = command;
				this.arguments = arguments;
				this.commandMessage = commandMessage;
				
			}
			
		}
		
	}
	
}