using Microsoft.Extensions.DependencyInjection;
using Notification.Api.Infrastructure.Messages.v1;
using Notification.Sender.Job.Core;
using Notification.Sender.Job.Events;
using System;
using System.Threading.Tasks;

namespace Notification.Sender.Job.Commands;

public class CommandHandlerFactory : ICommandHandlerFactory
{
    private IServiceProvider _serviceProvider;

    public CommandHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task ResolveHandlerExecute(IntegrationEvent @event)
    {
        if (@event.Type == typeof(SendSmsEvent).FullName)
        {
            var handlerSms = _serviceProvider.GetService<ICommandHandler<SendSmsCommand>>();
            return handlerSms.ExecuteAsync((SendSmsCommand)@event.Data.Deserializer<SendSmsEvent>());
        }
        else if (@event.Type == typeof(SendEmailEvent).FullName)
        {
            var handlerEmail = _serviceProvider.GetService<ICommandHandler<SendEmailCommand>>();
            return handlerEmail.ExecuteAsync((SendEmailCommand)@event.Data.Deserializer<SendEmailEvent>());
        }
        else if (@event.Type == typeof(SendEmailEvent).FullName)
        {
            var handlerEmail = _serviceProvider.GetService<ICommandHandler<SendWhatsAppCommand>>();
            return handlerEmail.ExecuteAsync((SendWhatsAppCommand)@event.Data.Deserializer<SendWhatsAppEvent>());
        }
        else
            throw new ArgumentException("Notfound event type");
    }
}