using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Rebus.Config;
using Rebus.Persistence.InMem;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using Rebus.Transport.InMem;

using Core;

using Mail.Commands;
using Mail.Events;

namespace Mail.Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    // Configure Rebus
                    var mailsQueueName = "mails.send";
                    var mailsQueueErrorName = "mails.send.error";

                    services.AddRebus(configure => configure
                        // # 1 - Single (InMemory)
                        //.Transport(t => t.UseInMemoryTransport(new InMemNetwork(), mailsQueueName))
                        //.Subscriptions(s => s.StoreInMemory())

                        // # 2 - Distributed (RabbitMQ)
                        .Transport(t => t.UseRabbitMq("amqp://localhost", mailsQueueName))

                        // Common
                        .Routing(r =>
                        {
                            r.TypeBased()
                                .MapAssemblyOf<Message>(mailsQueueName)
                                .MapAssemblyOf<SendMailCommand>(mailsQueueName);
                        })
                        .Options(o =>
                        {
                            o.SetNumberOfWorkers(1);
                            o.SetMaxParallelism(1);
                            o.SetBusName("Rebus Mail Sample");
                            o.SimpleRetryStrategy(mailsQueueErrorName);
                        })
                    );

                    services.AddRebusHandler<MailEventHandler>();

                    services.BuildServiceProvider().UseRebus(async c =>
                    {
                        await c.Subscribe<MailSentEvent>();
                        await c.Subscribe<MailUnsentEvent>();
                    });
                });
    }
}
