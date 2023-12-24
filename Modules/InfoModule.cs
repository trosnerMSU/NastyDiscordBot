using Discord.Commands;
using Discord.WebSocket;

namespace NastyDiscordBot.Modules {
    public class InfoModule : ModuleBase<SocketCommandContext> {
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder][Summary("The text to echo")] string echo) => ReplyAsync(echo);

        [Command("userinfo")]
        [Summary
            ("Returns info about the current user, or the user parameter, if one passed.")]
        [Alias("user", "whois")]
        public async Task UserInfoAsyn([Summary("The (optional) user to get info from")] SocketUser user = null) {
            var userInfo = user ?? Context.Client.CurrentUser;
            await ReplyAsync(String.Format("{0}#{1}", userInfo.Username, userInfo.Discriminator));
        }

        [Command("square")]
        [Summary("Squares a number.")]
        public async Task SquareAsync([Summary("The number to a square")] int num) {
            await Context.Channel.SendMessageAsync(String.Format("{0}^2 = {1}", num.ToString(), Math.Pow(num, 2).ToString()));
        }
    }
}
