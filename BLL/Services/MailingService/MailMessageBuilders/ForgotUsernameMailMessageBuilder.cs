namespace BLL.Services.MailingService.MailMessageBuilders
{
    public class ForgotUsernameMailMessageBuilder : MailMessageBuilder
    {
        private readonly string _body = "Hello, dear user. Your username is {0}";
        private readonly string _subject = "User name remindering";

        public ForgotUsernameMailMessageBuilder(string username)
        {
            Body = string.Format(_body, username);
            Subject = _subject;
        }
    }
}