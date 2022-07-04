using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modmail
{
    public class ModmailButtonHandler
    {

        public static Discord.Embed ErrorEmbed(string message)
        {
            return new EmbedBuilder().WithTitle("Error").WithDescription(message).WithColor(Color.Red).Build();
        }

        public static async Task Run(SocketModal modal)
        {
            // make sure this is the correct interaction
            if (modal.Data.CustomId != ModmailButton.MODMAIL_MODAL_ID)
            {
                return;
            }

            var components = modal.Data.Components.ToList();

            // dig the modmail message out of the data
            string content = components.First(x => x.CustomId == "content").Value;

            content = content.Replace("```", ""); // sanitize message

            var channel = await Program.Client.GetChannelAsync(Config.GetCachedConfig().ModmailChannel);

            if (channel != null && channel is SocketGuildChannel gc && channel is SocketTextChannel tc)
            {
                var msg = await tc.GetMessageAsync((ulong)Config.GetCachedConfig().Internal.CreateModmailMessageId);

                if (msg != null)
                {
                    try
                    {
                        // create a private thread
                        var thread = await tc.CreateThreadAsync("modmail-" + modal.User.Id.ToString(), Discord.ThreadType.PrivateThread,
                        invitable: false,
                        message: null);


                        // give the user who created the modmail permission
                        await thread.AddUserAsync(modal.User as SocketGuildUser);

                        var modPingRole = gc.Guild.GetRole(Config.GetCachedConfig().ModPingId);

                        if (modPingRole != null)
                        {
                            // send a message with the modmail content and ping mods
                            await thread.SendMessageAsync($"{modal.User.Mention} {modPingRole.Mention} ```{content}```", allowedMentions: new Discord.AllowedMentions()
                            {
                                RoleIds = new List<ulong>()
                        {
                            Config.GetCachedConfig().ModPingId
                        },
                                UserIds = new List<ulong>()
                        {
                            modal.User.Id
                        }
                            });

                            // does nothing except ack the interaction
                            await modal.DeferAsync();
                        }
                        else
                        {
                            await modal.RespondAsync(embed: ErrorEmbed("Mod role does not exist."), ephemeral: true);
                        }
                    }
                    catch (Exception ex)
                    {
                        await modal.RespondAsync(embed: ErrorEmbed($"Your modmail could not be created. Error: {ex.Message}"), ephemeral: true);
                    }

                }
                else
                {
                    await modal.RespondAsync(embed: ErrorEmbed("The modmail message no longer exists or is invalid."), ephemeral: true);
                }
            }
            else
            {
                await modal.RespondAsync(embed: ErrorEmbed("The modmail channel either no longer exists or is invalid."), ephemeral: true);
            }

        }
    }
}
