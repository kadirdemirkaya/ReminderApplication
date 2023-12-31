using ReminderApp.Api;
using ReminderApp.Application;
using ReminderApp.Infrastructure;
using ReminderApp.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.ApiDependencyInjection(builder.Configuration);

builder.Services.ApplicationDependencyInjection();

builder.Services.PersistenceDependencyInjection(builder.Configuration);

builder.Services.InfrastructureDependencyInjection(builder.Configuration);

builder.InfrastructureDependencyInjectionBuilder(builder.Configuration);

var app = builder.Build();

app.UseRouting();

app.InfrastructureDependencyInjectionApp();

app.PersistenceInjectionApp();

app.UseHttpsRedirection();

app.ApiDependencyApp();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
