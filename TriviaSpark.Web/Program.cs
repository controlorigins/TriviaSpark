using HttpClientDecorator;
using HttpClientDecorator.Interfaces;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using TriviaSpark.Core.Interfaces;
using TriviaSpark.OpenTriviaDb.Services;
using TriviaSpark.Web.Areas.Identity.Data;
using TriviaSpark.Web.Areas.Identity.Services;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);
    var connectionString = builder.Configuration.GetConnectionString("TriviaSparkWebContextConnection") ?? throw new InvalidOperationException("Connection string 'TriviaSparkWebContextConnection' not found.");
    builder.Services.AddDbContext<TriviaSparkWebContext>(options => options.UseSqlite(connectionString));
    builder.Services.AddDefaultIdentity<TriviaSparkWebUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<TriviaSparkWebContext>();

    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession();
    builder.Services.AddHttpContextAccessor();

    // Add the HttpGetCall and Telemetry Decorator for IHttpGetCallService interface
    // Add Http Client Factory
    builder.Services.AddHttpClient("TriviaSpark", client =>
    {
        client.Timeout = TimeSpan.FromMilliseconds(1500);

        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("User-Agent", "TriviaSpark");
        client.DefaultRequestHeaders.Add("X-Request-QuestionId", Guid.NewGuid().ToString());
        client.DefaultRequestHeaders.Add("X-Request-Source", "TriviaSpark");
    });

    builder.Services.AddSingleton(serviceProvider =>
    {
        var logger = serviceProvider.GetRequiredService<ILogger<HttpGetCallService>>();
        var telemetryLogger = serviceProvider.GetRequiredService<ILogger<HttpGetCallServiceTelemetry>>();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        IHttpGetCallService baseService = new HttpGetCallService(logger, httpClientFactory);
        IHttpGetCallService telemetryService = new HttpGetCallServiceTelemetry(telemetryLogger, baseService);
        return telemetryService;
    });

    builder.Services.AddScoped<IQuestionSourceAdapter, OpenTriviaDbQuestionSource>();
    builder.Services.AddScoped<IMatchService, TriviaMatchService>();

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();
    app.UseCookiePolicy();
    app.UseSession();
    app.MapRazorPages();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
          name: "areas",
          pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
        );
    });


    app.Run();

}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}

