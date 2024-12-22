using discipline.centre.bootstrappers.api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
var assemblies = ModuleLoader.GetAssemblies(builder.Configuration);
var modules = ModuleLoader.GetModules(assemblies);
builder.Services.AddInfrastructure(assemblies, builder.Configuration);
builder.Services.AddModulesConfiguration(modules, builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseInfrastructure();
app.MapHealthChecks("/");
app.UseModulesConfiguration(modules);

await app.RunAsync();
