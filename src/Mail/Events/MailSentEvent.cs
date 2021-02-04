namespace Mail.Events
{
    public class MailSentEvent : MailEvent
    {
        public MailSentEvent(string from, string to, string subject, string message) 
            : base(from, to, subject, message)
        {
        }
    }
}
