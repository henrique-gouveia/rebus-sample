namespace Mail.Events
{
    public class MailUnsentEvent : MailEvent
    {
        public MailUnsentEvent(string from, string to, string subject) 
            : base(from, to, subject)
        {
        }
    }
}
