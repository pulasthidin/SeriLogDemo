using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriLogDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.Console() //console loggers (errors write to Console)
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day) //file logger (errors write to file)
                .CreateLogger(); //configure seri log


            try
            {
                Log.Information("Application has started at {LogTime}", DateTime.Now);
                CreateHostBuilder(args).Build().Run();
                
            }
            catch( Exception ex)
            {
                Log.Error(ex, "Application has stopped at {LogTime}", DateTime.Now);
            }
            finally
            {
                Log.CloseAndFlush();
            }
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logger => 
                { 
                    logger.ClearProviders(); // default logger clear 
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
    }
}
