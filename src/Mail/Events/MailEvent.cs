using Core;

namespace Mail.Events
{
    public class MailEvent : Event
    {
        public MailEvent(string from, string to, string subject)
        {
            From = from;
            To = to;
            Subject = subject;
        }

        public string From { get; }
        public string To { get; }
        public string Subject { get; }
    }
}
