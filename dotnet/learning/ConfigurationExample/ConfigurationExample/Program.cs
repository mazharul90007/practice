using ConfigurationExample.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Configuration.AddJsonFile("myownconfig.json", optional: true, reloadOnChange: true);

builder.Services.Configure<WeatherApiOptions>(builder.Configuration.GetSection("weatherapi"));

var app = builder.Build();


app.UseStaticFiles();
app.UseRouting();

app.MapControllers();

app.Run();
