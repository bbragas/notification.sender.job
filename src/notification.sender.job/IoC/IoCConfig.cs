using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Notification.Sender.Job.Commands;
using Notification.Sender.Job.Core;
using Serilog;
using Serilog.Formatting.Json;

namespace Notification.Sender.Job.IoC;

public static class IoCConfig
{
    public static IServiceProvider Configure()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var services = new ServiceCollection();
        services.AddLogging(config => config.ClearProviders().SetMinimumLevel(LogLevel.Trace));

        services.Configure<IAgentSmtpConfig>(configuration.GetSection(nameof(IAgentSmtpConfig)));
        services.Configure<MessagingServiceConfig>(configuration.GetSection(nameof(MessagingServiceConfig)));
        services.Configure<WhatsAppServiceConfig>(configuration.GetSection(nameof(WhatsAppServiceConfig)));

        services.AddSingleton<ICommandHandlerFactory, CommandHandlerFactory>();
        services.AddSingleton<ICommandHandler<SendEmailCommand>, SendEmailCommandHandler>();
        services.AddSingleton<ICommandHandler<SendSmsCommand>, SendSmsCommandHandler>();
        services.AddSingleton<ICommandHandler<SendWhatsAppCommand>, SendWhatsAppCommandHandler>();


        Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .WriteTo.Console(new JsonFormatter())
                        .CreateLogger();

        services.AddSingleton<Serilog.ILogger>(_ => Log.Logger);

        return services.BuildServiceProvider();
    }
}
