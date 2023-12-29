using Newtonsoft.Json;

namespace Notification.Sender.Job.Models.IAgentSmtp;

public class IAgentEmailModel
{
    public IAgentEmailModel(string apiKey, string apiUser, SendEmailModel model, Sender from)
    {
        ApiKey = apiKey;
        ApiUser = apiUser;

        To = new[] { new Recipient { Email = model.To, Name = model.Name } };

        From = from;
        Subject = model.Subject;
        Html = model.Html;
    }

    [JsonProperty("api_key")]
    public string ApiKey { get; set; }

    [JsonProperty("api_user")]
    public string ApiUser { get; set; }

    [JsonProperty("to")]
    public Recipient[] To { get; set; }

    [JsonProperty("from")]
    public Sender From { get; set; }

    [JsonProperty("subject")]
    public string Subject { get; set; }

    [JsonProperty("html")]
    public string Html { get; set; }

    public record struct Recipient
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public record struct Sender
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}