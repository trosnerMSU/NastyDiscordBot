using Discord;
using Discord.WebSocket;
using System.Diagnostics;

namespace NastyDiscordBot {
    public class Program {
        private DiscordSocketClient? client;
        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync() {
            var config = new DiscordSocketConfig { MessageCacheSize = 100 };
            client = new DiscordSocketClient(config);

            try {
                client.Log += Log;

                var token = File.ReadAllText("C:\\GithubProjects\\nastybottoken.txt");
                await client.LoginAsync(TokenType.Bot, token);
                await client.StartAsync();

                

                client.MessageUpdated += MessageUpdated;
                client.Ready += () => {
                    Console.WriteLine("Bot is connected!");
                    return Task.CompletedTask;
                };

                await Task.Delay(-1);

            } catch(Exception ex) {
                client.Log += Log;
                Console.WriteLine(ex.Message);
                await client.StopAsync();
            }
        }

        private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel) {
            // If the message was not in the cache, fownloading it will result in getting a copy of 'after'
            var message = await before.GetOrDownloadAsync();
            Console.WriteLine(String.Format("{0} -> {1}", message, after.ToString()));
        }

        private Task Log(LogMessage msg) {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}