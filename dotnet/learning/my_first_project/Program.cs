using my_first_project.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); //add all the controllers as services
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();


app.MapControllers();
app.Run();
