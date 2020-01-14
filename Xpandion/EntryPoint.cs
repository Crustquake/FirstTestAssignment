using System;
using System.IO;

using NLog;
using NLog.Web;
using NLog.Config;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Loglevel = Microsoft.Extensions.Logging.LogLevel;


namespace Xpandion.WebSite
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {
            Logger logger = null;
            try
            {
                logger = GetLogger();
                logger.Debug("Initialization");

                IHostBuilder hostBuilder = CreateHostBuilder(args);
                IHost host = hostBuilder.Build();
                host.Run();
            }
            catch (Exception exception)
            {
                logger?.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
        //This public method is needed for Migrations
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Loglevel.Trace);
                })
                .UseNLog();
        }
        private static Logger GetLogger()
        {
            IConfigurationSection configSection = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build()
                .GetSection("NLog");
            LoggingConfiguration loggingConfig = new NLogLoggingConfiguration(configSection);
            Logger logger = NLogBuilder.ConfigureNLog(loggingConfig).GetCurrentClassLogger();
            return logger;
        }
    }
}
