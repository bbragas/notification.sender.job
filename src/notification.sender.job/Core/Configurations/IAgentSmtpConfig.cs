namespace Notification.Sender.Job.Core;

#pragma warning disable S101 //disable because of I<name> sonarqube rule against class types
public class IAgentSmtpConfig
#pragma warning restore S101
{
    public string ApiKey { get; set; }

    public string ApiUser { get; set; }

    public string BaseUrl { get; set; }

    public string FromSender { get; set; }

    public string NameSender { get; set; }
}
