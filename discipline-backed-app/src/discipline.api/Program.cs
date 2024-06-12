using discipline.application.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication(builder.Configuration);
var app = builder.Build();

app.UseApplication();
app.UseHttpsRedirection();
app.Run();

