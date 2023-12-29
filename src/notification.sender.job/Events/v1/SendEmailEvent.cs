using Notification.Sender.Job.Commands;
using System;

namespace Notification.Api.Infrastructure.Messages.v1;

public record class SendEmailEvent(
    Guid Id,
    string To,
    string Name,
    string Subject,
    string SenderName,
    string Html)
{
    public static explicit operator SendEmailCommand(SendEmailEvent value)
    {
        return new SendEmailCommand
        {
            Html = value.Html,
            Id = value.Id,
            Name = value.Name,
            SenderName = value.SenderName,
            Subject = value.Subject,
            To = value.To
        };
    }
}

