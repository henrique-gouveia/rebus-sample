using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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

namespace Mail.Producer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add Swagger
            services.AddSwaggerGen();

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
                    o.SimpleRetryStrategy(
                        errorQueueAddress: mailsQueueErrorName,
                        secondLevelRetriesEnabled: true);
                })
            );

            // Register handlers 

            // # 1 - Single
            // services.AutoRegisterHandlersFromAssemblyOf<MailCommandHandler>();
            
            // # 2 - Distributed
            services.AddRebusHandler<MailCommandHandler>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo Rebus Mail");
            });

            // # 1 - Single
            //app.ApplicationServices.UseRebus(async c =>
            //{
            //    await c.Subscribe<MailSentEvent>();
            //    await c.Subscribe<MailUnsentEvent>();
            //});

            // # 2 - Distributed
            app.ApplicationServices.UseRebus();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
