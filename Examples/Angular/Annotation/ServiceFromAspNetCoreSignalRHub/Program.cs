using ServiceFromAspNetCoreSignalRHub.Hubs;
using ServiceFromAspNetCoreSignalRHub.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSingleton<WeatherService>();

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllers();
app.MapHub<WeatherHub>("/hub/weather");

// In development the Angular CLI serves the SPA and Microsoft.AspNetCore.SpaProxy forwards
// to it (see SpaProxyServerUrl in the csproj). In production the published ClientApp output
// is served from wwwroot.
app.MapFallbackToFile("index.html");

app.Run();
