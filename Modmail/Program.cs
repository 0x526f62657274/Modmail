using Discord;
using Discord.WebSocket;

namespace Modmail
{

    public class Program
    {
        public static DiscordSocketClient Client;

        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {
            Client = new DiscordSocketClient();

            Client.Log += (message) =>
            {
                Console.WriteLine(message);
                return Task.CompletedTask;
            };

            // when interaction is created, run modmail button task
            Client.InteractionCreated += async (inter) =>
            {
                if (inter is SocketMessageComponent smc)
                {
                    await ModmailButton.Run(smc);
                }
            };

            // create modmail message with the button interaction if it doesn't exist already
            Client.Ready += async () =>
            {
                var channel = await Client.GetChannelAsync(Config.GetCachedConfig().ModmailChannel) as SocketTextChannel;

                if (await channel.GetMessageAsync((ulong)Config.GetCachedConfig().Internal.CreateModmailMessageId) == null)
                {
                    var comps = new ComponentBuilder();

                    comps.WithButton(Config.GetCachedConfig().Embed.ButtonName, ModmailButton.BUTTON_ID, ButtonStyle.Primary, emote: new Emoji("✉️"));

                    var msg = await channel.SendMessageAsync(embed: new EmbedBuilder().WithColor(Color.Blue).
                        WithTitle(Config.GetCachedConfig().Embed.Title).
                        WithDescription(Config.GetCachedConfig().Embed.Description).Build(), components: comps.Build());

                    Config.GetCachedConfig().Internal.CreateModmailMessageId = msg.Id;
                    Config.SaveConfig(Config.GetCachedConfig());
                }
            };

            // on modmail modal submit. Does not currently spin up a seprate thread despite having to make a lot of API calls, I might fix that.
            Client.ModalSubmitted += ModmailButtonHandler.Run;

            //boilerplate below
            await Client.LoginAsync(Discord.TokenType.Bot, Config.GetCachedConfig().Token);
            await Client.StartAsync();

            await Task.Delay(-1);

        }

    }
}