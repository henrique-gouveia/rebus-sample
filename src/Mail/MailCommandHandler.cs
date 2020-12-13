using System;
using System.Threading.Tasks;

using Rebus.Bus;
using Rebus.Handlers;

using Mail.Commands;
using Mail.Events;

namespace Mail
{
    public class MailCommandHandler :
        IHandleMessages<SendMailCommand>
    {
        private readonly IBus bus;

        public MailCommandHandler(IBus bus)
            => this.bus = bus;

        public async Task Handle(SendMailCommand message)
        {
            var sended = new Random().NextDouble() >= 0.5;

            if (sended)
            {
                await bus.Publish(new MailSentEvent(message.From, message.To, message.Subject));
                return;
            }

            await bus.Publish(new MailUnsentEvent(message.From, message.To, message.Subject));
        }
    }
}
