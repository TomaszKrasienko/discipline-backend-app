using discipline.application.Configuration;
using discipline.application.Features.DailyProductivities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication(builder.Configuration);
var app = builder.Build();
app.UseRouting();
app.UseApplication();
app.UseHttpsRedirection();
app.Run();

