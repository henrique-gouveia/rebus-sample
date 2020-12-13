using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Rebus.Bus;

using Mail.Commands;
using Mail.Producer.ViewModels;

namespace Mail.Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailsController : ControllerBase
    {
        private readonly IBus bus;

        public MailsController(IBus bus)
            => this.bus = bus;

        [HttpPost]
        public async Task<IActionResult> Send(MailViewModel mail)
        {
            await bus.Send(new SendMailCommand(mail.From, mail.To, mail.Subject, mail.Message));
            return Ok();
        }
    }
}
