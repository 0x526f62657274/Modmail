using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modmail
{
    public class ModmailButton
    {
        public static readonly string BUTTON_ID = "MODMAIL_MODAL_BUTTON";
        public static readonly string MODMAIL_MODAL_ID = "MODMAIL_MODAL";

        /*
         * Creates the modmail model and responds with it upon a user clicking the button.
         */
        public static async Task Run(SocketMessageComponent smc)
        {
            if (smc.Data.CustomId == BUTTON_ID)
            {
                var modal = new ModalBuilder();
                modal.WithTitle(Config.GetCachedConfig().Modal.Title);
                modal.WithCustomId(MODMAIL_MODAL_ID);
                modal.AddTextInput(Config.GetCachedConfig().Modal.TextboxTitle, "content", TextInputStyle.Paragraph, placeholder: Config.GetCachedConfig().Modal.TextboxPlaceholder, minLength: 10, maxLength: 800);
                await smc.RespondWithModalAsync(modal.Build());
            }
        }
    }
}
