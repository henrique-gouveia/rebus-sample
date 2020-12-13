using System;
using System.Threading.Tasks;

using Rebus.Handlers;

using Mail.Events;

namespace Mail
{
    public class MailEventHandler :
        IHandleMessages<MailSentEvent>,
        IHandleMessages<MailUnsentEvent>
    {
        public Task Handle(MailSentEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Email SENT from {message.From} to {message.To} with subject {message.Subject} on {message.DateTime}");
            Console.ForegroundColor = ConsoleColor.Black;

            return Task.CompletedTask;
        }

        public Task Handle(MailUnsentEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Email UNSENT from {message.From} to {message.To} with subject {message.Subject} on {message.DateTime}");
            Console.ForegroundColor = ConsoleColor.Black;

            return Task.CompletedTask;
        }
    }
}
