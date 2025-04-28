
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using StockAnalyzer.Domain.Contracts;
using StockAnalyzer.Infrastructure;
using StockAnalyzer.Infrastructure.Repositories;
using StockAnalyzer.Shared.CQRS;

namespace StockAnalyzer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<StockDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("StockDb")));
            builder.Services.AddScoped<IStockRepository, StockRepository>();
            builder.Services.AddMediator();

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 104857600; // Increase to 100 MB (in bytes)
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<StockDbContext>();
                dbContext.Database.Migrate();
            }

            app.Run();
        }
    }
}
