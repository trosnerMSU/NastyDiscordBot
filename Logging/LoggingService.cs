using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NastyDiscordBot.Logging {
    public class LoggingService {
        public LoggingService(DiscordSocketClient client, CommandService command) {
            client.Log += LogAsync;
            command.Log += LogAsync;
        }

        private Task LogAsync(LogMessage message) {
            if(message.Exception is CommandException cmdException) {
                Console.WriteLine(String.Format("[Command/{0}] {1} failed to execute in {2}"
                    , message.Severity.ToString()
                    , cmdException.Command.Aliases.First().ToString()
                    , cmdException.Context.Channel));
            } else {
                Console.WriteLine(String.Format("[General/{0}] {1}", message.Severity.ToString(), message));
            }

            return Task.CompletedTask;
        }
    }
}
