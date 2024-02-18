
using Microsoft.EntityFrameworkCore;
using MsBanking.Card.Domain;
using MsBanking.Card.Domain.Dto;
using MsBanking.Card.Services;

namespace MsBanking.Card
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<CardDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<ICardService, CardService>();
            builder.Services.AddScoped<ICardTransactionService, CardTransactionService>();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(CardDtoProfile));
            builder.Services.AddAutoMapper(typeof(CardTransactionDtoProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            DataSeeder.Seed(app);

            app.Run();
        }
    }
}
