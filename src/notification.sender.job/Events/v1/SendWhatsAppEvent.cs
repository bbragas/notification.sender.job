using Notification.Sender.Job.Commands;
using System;

namespace Notification.Api.Infrastructure.Messages.v1;
public record class SendWhatsAppEvent(
    Guid Id,
    string Campaign,
    string SenderName,
    string To,
    string Text)
{
    public static explicit operator SendWhatsAppCommand(SendWhatsAppEvent value)
    {
        return new SendWhatsAppCommand
        {
          Id = value.Id,
          To = value.To,
          SenderName  = value.SenderName,
          Text = value.Text,
        };
    }
}

