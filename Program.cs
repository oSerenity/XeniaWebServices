using Amazon.Util.Internal.PlatformServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Reflection;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        await RunApp();
    }

    static async Task RunApp(int port = 36000)
    {
        var host = Host.CreateDefaultBuilder()
.ConfigureLogging((hostingContext, logging) =>
{
    logging.ClearProviders(); // Remove the default providers
    logging.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Error);
    // Add the console logger with a customized log message format
    logging.AddConsole(options =>
    {
        options.DisableColors = false; // Enable console colors if desired

    });
})

            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .UseStartup<Startup>()
                    .UseKestrel()
                    .ConfigureKestrel(options =>
                    {
                        options.AllowSynchronousIO = true; // Enable synchronous I/O operations
                        options.ListenLocalhost(port);
                    });
            })
            .Build();

        // Start the host and await it
        await host.StartAsync();
        Console.Clear();
        CenterTextInConsole(port); 
        await host.WaitForShutdownAsync();
    }

    static void CenterTextInConsole(int port)
    {
        string text = "===================================================================================\n\r "
                 +    $"{Assembly.GetEntryAssembly().GetName().Name} Was Made By Serenity Special Thanks To ahnewark For The Network Logic\n\r "
                 +   $"                     listening on: (http://localhost:{port})\n\r "
                 +    "===================================================================================";
        Console.WriteLine(text);
        Console.WriteLine("\n\r Application is now Ready...\n\r");
    }
}
