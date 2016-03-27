namespace BLL.Services.MailingService.MailMessageBuilders
{
    public abstract class MailMessageBuilder
    {
        public string Body { get; protected set; }
        public string Subject { get; protected set; }
    }
}