using System;
using System.Threading.Tasks;

using Rebus.Handlers;

using Mail.Events;
using Mail.Exceptions;

namespace Mail
{
    public sealed class MailEventHandler :
        IHandleMessages<MailSentEvent>,
        IHandleMessages<MailUnsentEvent>
    {
        public Task Handle(MailSentEvent message)
        {
            WriteOnConsole(message, "SENT", ConsoleColor.Blue);
            return Task.CompletedTask;
        }

        public Task Handle(MailUnsentEvent message)
        {
            WriteOnConsole(message, "UNSENT", ConsoleColor.Red);
            return Task.CompletedTask;
        }

        private void WriteOnConsole(MailEvent message, string description, ConsoleColor foregroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            try
            {
                Console.WriteLine();
                Console.WriteLine($"Email {description} on {message.DateTime}");
                Console.WriteLine($"    From: {message.From} to {message.To}");
                Console.WriteLine($"    Subject: {message.Subject}");
                Console.WriteLine($"    Message: {message.Message}");
                Console.WriteLine();

                //if (message is MailUnsentEvent)
                //    throw new SendMailException($"Email from {message.From} to {message.To} { description }");
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Black;
            }
        }
    }
}
