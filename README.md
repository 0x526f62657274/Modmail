# Modmail

Example config:

```yaml
#Your bot's token
token: abcd1234
#The channel where the "Create Modmail" message with button will be sent automatically
modmailChannel: 1234
# Role ID to ping when a modmail thread is created.
modPingId: 1234
# Controls text for the modmail form
modal:
  title: Send Modmail
  textboxPlaceholder: Enter your private modmail here. You can always write more or add an attachment later on.
  textboxTitle: Message
internal:
  createModmailMessageId: 1234 # does not need to be set
#Controls the embed for the "Create Modmail" message
embed:
  title: Modmail
  description: Click the button to send a private message to the moderators.
  buttonName: Send Modmail
