using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// builder.Services.Add(new ServiceDescriptor(
//     typeof(ICitiesServices),
//     typeof(CitiesService),
//     ServiceLifetime.Scoped
// ));
builder.Services.AddTransient<ICitiesServices, CitiesService>();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();



app.Run();
