using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriLogDemo
{
    public class Program
    {
        [Obsolete]
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connString = config.GetConnectionString("Default");
            var logTable = config.GetValue<string>("MyLogs:LogTableName");
            var colOptions = new ColumnOptions();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.Console() //console loggers (errors write to Console)
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day) //file logger (errors write to file)
                .WriteTo.MSSqlServer(
                connectionString:connString,
                tableName:logTable,
                columnOptions:colOptions,
                autoCreateSqlTable:true)
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
