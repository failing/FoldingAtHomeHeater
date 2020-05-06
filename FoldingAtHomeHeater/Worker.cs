using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using FoldingAtHomeHeater.Entites;
using RestEase;
using System.Diagnostics;
using Serilog;

namespace FoldingAtHomeHeater
{
    public class Worker : BackgroundService
    {
        private readonly string BaseUrl = "http://dataservice.accuweather.com/currentconditions/v1/";
        private readonly long LowerHuttLocationId = 250942;
        private string ApiKey { get; set; }
        private static Random random = new Random();
        private readonly Serilog.Core.Logger logger;

        public Worker(string api)
        {
            logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            ApiKey = api;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IAccWeather api = RestClient.For<IAccWeather>(BaseUrl);
            var password = RandomString(32);

            while (!stoppingToken.IsCancellationRequested)
            {
                WeatherResponse weather = (await api.GetWeatherConditions(LowerHuttLocationId, ApiKey))[0];

                var Process = GetProcessRunning();

                if (weather.Temperature.Metric.Value < 15)
                {
                    if(!Process.Running)
                    {
                        ProcessStartInfo procStartInfo = new ProcessStartInfo("FAHClient", $"--user karter --team 0 --passkey={password} --gpu=true --smp=true");

                        procStartInfo.RedirectStandardOutput = false;
                        procStartInfo.UseShellExecute = false;
                        procStartInfo.CreateNoWindow = true;

                        Process proc = new Process();
                        proc.StartInfo = procStartInfo;
                        proc.Start();
                    }
                    else
                    {
                        foreach (var process in Process.Processes)
                        {
                            process.StandardInput.WriteLine("--send-unpause");
                        }
                    }
                    logger.Information("Temperatur At: {time} {temp}C", DateTimeOffset.Now, weather.Temperature.Metric.Value);
                }
                else
                {
                    if (Process.Running)
                    {
                        foreach (var process in Process.Processes)
                        {
                            process.StandardInput.WriteLine("--send-pause");
                        }
                    }
                }
                await Task.Delay(1800000, stoppingToken);
            }
        }

        public (bool Running, Process[] Processes) GetProcessRunning()
        {
            Process[] processes = Process.GetProcessesByName("FAHClient");

            if(processes.Length > 0)
            {
                return (true, processes);
            }

            return (false, null);
        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
