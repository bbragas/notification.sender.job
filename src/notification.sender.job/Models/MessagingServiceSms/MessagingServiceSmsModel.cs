using Newtonsoft.Json;

namespace Notification.Sender.Job.Models.MessagingServiceSms;

public class MessagingServiceSmsModel
{
    public MessagingServiceSmsModel(SendSmsModel sendSmsSimpleModel)
    {
        this.To = sendSmsSimpleModel.To;
        this.From = sendSmsSimpleModel.From;
        this.Text = sendSmsSimpleModel.Text;
    }

    [JsonProperty("to")]
    public string[] To { get; private set; }

    [JsonProperty("from")]
    public string From { get; private set; }

    [JsonProperty("text")]
    public string Text { get; private set; }

}
