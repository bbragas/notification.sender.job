using Notification.Sender.Job.Commands;
using System;

namespace Notification.Api.Infrastructure.Messages.v1;
public record class SendSmsEvent(
    Guid Id,
    string Campaign,
    string SenderName,
    string To,
    string Text)
{
    public static explicit operator SendSmsCommand(SendSmsEvent value)
    {
        return new SendSmsCommand
        {
          Id = value.Id,
          To = value.To,
          SenderName  = value.SenderName,
          Text = value.Text,
        };
    }
}

