using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FoldingAtHomeHeater
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IHostedService>(provider => new Worker("N8SdFAcMySqCzSPIFYFgvsVCtRi7nOAX"));
                });
    }
}
