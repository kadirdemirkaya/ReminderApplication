﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ReminderApp.Domain.Constats;
using ReminderApp.Infrastructure.Hubs;

namespace ReminderApp.Infrastructure.Registrations
{
    public static class Hub
    {
        public static IServiceCollection SignalRRegistration(this IServiceCollection services)
        {
            services.AddSignalR();

            services.AddScoped<NotificationHub>();

            services.AddScoped<UserHub>();

            return services;
        }

        public static WebApplication HubRegistrationApp(this WebApplication app)
        {
            app.MapHub<NotificationHub>($"/{Channels.NotificationHub}");

            app.MapHub<UserHub>($"/{Channels.UserHub}");

            return app;
        }
    }
}
