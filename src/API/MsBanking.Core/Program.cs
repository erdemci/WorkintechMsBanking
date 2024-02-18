
using MassTransit;
using MsBanking.Common.Dto;
using MsBanking.Core.Apis;
using MsBanking.Core.Domain;
using MsBanking.Core.Services;
using Serilog;

namespace MsBanking.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<DatabaseOption>(builder.Configuration.GetSection("DatabaseOption"));
            builder.Services.AddScoped<ICustomerService, CustomerService>();

            //masstransit configure
            builder.Services.AddMassTransit(busConfig =>
            {
                busConfig.UsingRabbitMq((context, config) =>
                {
                    config.Host(new Uri(builder.Configuration["RabbitMq:HostName"]!), h =>
                    {
                        h.Username(builder.Configuration["RabbitMq:UserName"]);
                        h.Password(builder.Configuration["RabbitMq:Password"]);
                    });
                    config.ConfigureEndpoints(context);
                });
            });

            
            //automapper
            builder.Services.AddAutoMapper(typeof(CustomerDtoProfile));

            var app = builder.Build();

            //DEBUG-->INFORMATION-->WARNING-->ERROR-->FATAL
            //Serilog configuration
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("logs/log-.txt",rollingInterval:RollingInterval.Day)
                .CreateLogger();
            Log.Logger = logger;


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapGroup("/api/v1/")
                .WithTags("Core Banking Api v1")
                .MapCustomerApi();


            app.UseHttpsRedirection();

            app.UseAuthorization();
          
            app.Run();
        }
    }
}
