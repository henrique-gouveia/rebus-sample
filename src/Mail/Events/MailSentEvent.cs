namespace Mail.Events
{
    public class MailSentEvent : MailEvent
    {
        public MailSentEvent(string from, string to, string subject) 
            : base(from, to, subject)
        {
        }
    }
}
