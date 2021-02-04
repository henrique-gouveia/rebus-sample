namespace Mail.Events
{
    public class MailUnsentEvent : MailEvent
    {
        public MailUnsentEvent(string from, string to, string subject, string message) 
            : base(from, to, subject, message)
        {
        }
    }
}
