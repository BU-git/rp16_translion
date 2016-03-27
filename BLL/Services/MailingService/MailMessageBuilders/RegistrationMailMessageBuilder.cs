namespace BLL.Services.MailingService.MailMessageBuilders
{
    public class RegistrationMailMessageBuilder : MailMessageBuilder
    {
        private readonly string _body = "Thank, you, {0} for registration";
        private readonly string _subject = "Thank for your registration";

        public RegistrationMailMessageBuilder(string loginName)
        {
            Body = string.Format(_body, loginName);
            Subject = _subject;
        }
    }
}