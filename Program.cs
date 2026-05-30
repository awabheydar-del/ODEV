using ZeroDayAnomali.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<LogService>();
builder.Services.AddSingleton<BehaviorProfileService>();
builder.Services.AddSingleton<AnomalyDetectionService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var logService = scope.ServiceProvider.GetRequiredService<LogService>();
    var anomalyService = scope.ServiceProvider.GetRequiredService<AnomalyDetectionService>();
    SampleData.Seed(logService, anomalyService);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
