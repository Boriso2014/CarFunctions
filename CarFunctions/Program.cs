using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarFunctions
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(builder =>
                {
                    builder.UseExceptionHandling();
                    builder.Services.Configure<JsonSerializerOptions>(options =>
                    {
                        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    });
                })
                .ConfigureServices(services => ConfigureServices(services))
                .Build();

            host.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICarRepo, CarRepo>();
            services.AddTransient<ICarService, CarService>();
        }
    }
}