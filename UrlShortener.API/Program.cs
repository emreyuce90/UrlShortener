
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace UrlShortener.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            builder.Services.AddOpenApi();
            builder.Services.AddScoped<UrlShorteningService>();
            builder.Services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(connectionString: builder.Configuration.GetConnectionString("SqlServer"));
            });

            var app = builder.Build();
            app.AddMinimalApisExt();
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.Run();
        }
    }
}
