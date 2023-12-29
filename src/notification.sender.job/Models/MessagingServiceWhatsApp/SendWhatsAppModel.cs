namespace Notification.Sender.Job.Models.MessagingServiceWhatsApp;

public record struct SendWhatsAppModel(string From, string[] To, string Text);