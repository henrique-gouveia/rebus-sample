using Core;

namespace Mail.Commands
{
    public class SendMailCommand : Command
    {
        public SendMailCommand(string from, string to, string subject, string message)
        {
            From = from;
            To = to;
            Subject = subject;
            Message = message;
        }

        public string From { get; }
        public string To { get; }
        public string Subject { get; }
        public string Message { get; }
    }
}
