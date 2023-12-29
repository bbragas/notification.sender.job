namespace Notification.Sender.Job.Models.MessagingServiceSms;

public record struct SendSmsModel(string From, string[] To, string Text);