using NPV.Api.Services.Implementations;
using NPV.Api.Services.Interfaces;

namespace NPV.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowBlazorWebAssemblyClient",
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:7171")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                );
            });
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<IDiscountRatesService, DiscountRatesService>();
            builder.Services.AddScoped<INPVCalculatorService, NPVCalculatorService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            
            app.UseHttpsRedirection();

            app.UseCors("AllowBlazorWebAssemblyClient");

            app.UseAuthorization();


            app.MapControllers();

            

            app.Run();
        }
    }
}
