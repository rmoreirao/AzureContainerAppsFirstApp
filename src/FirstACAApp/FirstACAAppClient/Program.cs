using Dapr.Client;
using Microsoft.ApplicationInsights.Extensibility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient("BackEndApiExternal", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>("BackendApiConfig:BaseUrlExternalHttp"));

});

builder.Services.AddSingleton<DaprClient>(_ => new DaprClientBuilder().Build());

builder.Services.AddApplicationInsightsTelemetry();
 
builder.Services.Configure<TelemetryConfiguration>((o) => {
    o.TelemetryInitializers.Add(new AppInsightsTelemetryInitializer());
});

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

app.MapRazorPages();

app.Run();
