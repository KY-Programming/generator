using Asp.Versioning;
using AspDotNetAnnotations.Hubs;
using AspDotNetAnnotations.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApiVersioning(options => options.ApiVersionReader = new UrlSegmentApiVersionReader())
       .AddMvc();
builder.Services.AddSignalR();
builder.Services.AddSingleton<DummyService>();
builder.Services.AddSingleton<WeatherForecastService>();

WebApplication app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.MapHub<WeatherForecastHub>("/hubs/weather-forecast");
app.MapHub<MultipleOutputHub>("/hubs/multiple-output");
app.MapFallbackToFile("index.html");

app.Run();
