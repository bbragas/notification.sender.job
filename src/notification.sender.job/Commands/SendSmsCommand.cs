namespace Notification.Sender.Job.Commands;
public class SendSmsCommand : Command
{
    public string To { get; set; }
    public string SenderName { get; set; }
    public string Text { get; set; }
}
