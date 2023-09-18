using Microsoft.OpenApi.Models;
using XeniaWebServices;
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;

    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddControllers();
        services.AddRazorPages(options =>
        {
            options.Conventions.AddPageRoute("/EditFile", "title/{titleId}/{fileType}/edit");
        });
        // Add Swagger
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Xeno Live",
                Version = "v1",
                Description = "Xenia Network Api",
                Contact = new OpenApiContact
                {
                    Name = "OSerenity",
                    Email = "os3renity@gmail.com",
                },
            });

            // Define the XML comments file for documentation (if using XML comments)
            // options.IncludeXmlComments("YourApi.xml");
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        // Define your custom middleware here.
        app.UseRequestBodyMiddleware(async (ctx, next) =>
        {
            await next.Invoke();
        });

        // Enable middleware to serve generated Swagger as a JSON endpoint
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("v1/swagger.json", "Xeno API v1");
            options.RoutePrefix = "api-docs"; // You can customize the URL path
        });

        // Add other middleware and routing as needed.
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                name: "whoami",
                pattern: "whoami",
                defaults: new { controller = "Sessions", action = "WhoAmI" });

            endpoints.MapControllerRoute(
                name: "DeleteSessions",
                pattern: "DeleteSessions",
                defaults: new { controller = "Sessions", action = "DeleteSessions" });

            endpoints.MapControllerRoute(
                name: "players-find",
                pattern: "players/find",
                defaults: new { controller = "Player", action = "FindPlayer" });

            // Define the 'players' endpoint
            endpoints.MapControllerRoute(
                name: "players",
                pattern: "players",
                defaults: new { controller = "Players", action = "PostPlayerInfo" }
            );

            // Add a route for the URL format "127.0.0.1:36000/title/58410ae9/ports"
            endpoints.MapControllerRoute(
                name: "ports",
                pattern: "title/{titleId}/ports",
                defaults: new { controller = "Title", action = "Ports" });

            endpoints.MapControllerRoute(
                name: "Servers",
                pattern: "title/{titleId}/servers",
                defaults: new { controller = "Title", action = "Servers" });
        });
    }
}