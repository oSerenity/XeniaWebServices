using Microsoft.OpenApi.Models;
using XeniaWebServices.Networking;
using XeniaWebServices.Networking.Sessions; 

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        Console.Clear();
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<Session>();
        services.AddSingleton<Session>();
        services.AddTransient<Network>();
        services.AddHttpClient();
        services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // To keep original casing
    });
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
                name: "index",
                pattern: "/",
                defaults: new { controller = "Home", action = "Index" });

            endpoints.MapControllerRoute(
                name: "Servers",
                pattern: "/EditFile",
                defaults: new { controller = "EditFile", action = "OnPostAsync" });
            //working - 9/27/23
            endpoints.MapControllerRoute(
                name: "whoami",
                pattern: "whoami",
                defaults: new { controller = "Startup", action = "WhoAmI" });
            //working - 9/27/23
            endpoints.MapControllerRoute(
                name: "DeleteSessions",
                pattern: "DeleteSessions",
                defaults: new { controller = "Startup", action = "DeleteSessions" });
            //working - 9/27/23
            endpoints.MapControllerRoute(
                name: "ports",
                pattern: "title/{titleId}/ports",
                defaults: new { controller = "Title", action = "Ports" });

            endpoints.MapControllerRoute(
                name: "Servers",
                pattern: "title/{titleId}/servers",
                defaults: new { controller = "Title", action = "Servers" });

            endpoints.MapControllerRoute(
                name: "players-find",
                pattern: "players/find",
                defaults: new { controller = "Players", action = "FindPlayer" });

            // Define the 'players' endpoint
            endpoints.MapControllerRoute(
                name: "players",
                pattern: "players",
                defaults: new { controller = "Players", action = "CreatePlayer" }
            );
            endpoints.MapControllerRoute(
                name: "StartSession",
                pattern: "sessions/{sessionId}",
                defaults: new { controller = "Sessions", action = "StartSession" });

            endpoints.MapControllerRoute(
                name: "modify",
                pattern: "sessions/{sessionId}/modify",
                defaults: new { controller = "Sessions", action = "modifySession" });

            endpoints.MapControllerRoute(
                name: "Create",
                pattern: "sessions",
                defaults: new { controller = "Sessions", action = "CreateSession" });


        });
    }
}