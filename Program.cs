
public class Program
{
    public static async Task Main(string[] args)
    {
        await RunApp();
    }

    static async Task RunApp(int port = 36000)
    {
        await Host.CreateDefaultBuilder()
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
            .Build()
            .RunAsync();
    }
}
