using Application;
using Infrastructure;
using System.Reflection;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure();

            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Verwalter API",
                        Version = "v1",
                        Description = "API for managing countries, postcodes, cities, places, and addresses."
                    });

                var apiXmlFileName =
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                var apiXmlPath =
                    Path.Combine(AppContext.BaseDirectory, apiXmlFileName);

                options.IncludeXmlComments(apiXmlPath);
            });

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

            app.Run();
        }
    }
}
