namespace Notification.Sender.Job.Models.IAgentSmtp;

public record struct SendEmailModel(string To, string Name, string Subject, string Html);
