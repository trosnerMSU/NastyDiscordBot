using Discord.Commands;
using Discord.WebSocket;
using NastyDiscordBot.TypeReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NastyDiscordBot.Handlers {
    public class CommandHandler {
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private readonly IServiceProvider? services;
        public CommandHandler(DiscordSocketClient client, CommandService commands) {
            this.client = client;
            this.commands = commands;
        }

        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services) {
            this.client = client;
            this.commands = commands;
            this.services = services;
        }

        public async Task InstallCommandsAsync() {
            // Hook the MessageReceived event into our command handler
            client.MessageReceived += HandleCommandAsync;

            // Here we discover all of the command modules in the entry 
            // assembly and load them. Starting from Discord.NET 2.0, a
            // service provider is required to be passed into the
            // module registration method to inject the 
            // required dependencies.
            //
            // If you do not use Dependency Injection, pass null.
            // See Dependency Injection guide for more information.
            await commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: null);
        }

        public async Task HandleCommandAsync(SocketMessage messageParam) {
            var message = messageParam as SocketUserMessage;
            if(message == null) return;
            
            //Track the position of where the prefix ends in the message
            int argPos = 0;

            //Check for a character prefix
            if(!(message.HasCharPrefix('!', ref argPos) || 
                message.HasMentionPrefix(client.CurrentUser, ref argPos)) ||
                message.Author.IsBot) {
                return;
            }

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            await commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null);

        }

        public async Task SetupAsync() {
            client.MessageReceived += HandleCommandAsync;

            // Add boolean type reader
            commands.AddTypeReader(typeof(bool), new BooleanTypeReader());

            // Then register the modules
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }

        //public async Task CommandHandleAsync(SocketMessage msg) {

        //}
    }
}
