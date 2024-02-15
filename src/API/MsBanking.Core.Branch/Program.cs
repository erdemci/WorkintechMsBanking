
using Microsoft.EntityFrameworkCore;
using MsBanking.Common.Dto;
using MsBanking.Core.Branch.Apis;
using MsBanking.Core.Branch.Domain;
using MsBanking.Core.Branch.Services;
using Serilog;

namespace MsBanking.Core.Branch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<BranchDbContext>(opt=>opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IBranchService, BranchService>();

            //automapper
            builder.Services.AddAutoMapper(typeof(BranchProfile));

            builder.Services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = builder.Configuration.GetConnectionString("Redis");
            });


            var app = builder.Build();

            //Serilog configuration
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Log.Logger = logger;

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.MapGroup("/api/v1/")
             .WithTags("Core Banking Branch Api v1")
             .MapBranchApi();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            DataSeeder.Seed(app);

          

            app.Run();
        }
    }
}
