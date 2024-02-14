
using Microsoft.EntityFrameworkCore;
using MsBanking.Common.Dto;
using MsBanking.Core.Account.Apis;
using MsBanking.Core.Account.Domain;
using MsBanking.Core.Account.Services;

namespace MsBanking.Core.Account
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

            builder.Services.AddScoped<IAccountService, AccountService>();

            string connString = builder.Configuration.GetConnectionString("Mysql");
            builder.Services.AddDbContext<AccountDbContext>(opt => opt.UseMySql(connString, ServerVersion.AutoDetect(connString)));

            builder.Services.AddAutoMapper(typeof(AccountDtoProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapGroup("/api/v1/")
             .WithTags("Account Api v1")
             .MapAccountApi();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            DataSeeder.Seed(app);
            app.Run();
        }
    }
}
