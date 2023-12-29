namespace Notification.Sender.Job.Commands;

public class SendEmailCommand : Command
{
    public string SenderName { get; set; }

    public string To { get; set; }

    public string Name { get; set; }

    public string Subject { get; set; }

    public string Html { get; set; }
}