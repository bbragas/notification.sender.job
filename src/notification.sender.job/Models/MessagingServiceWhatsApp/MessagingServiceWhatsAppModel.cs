using Newtonsoft.Json;

namespace Notification.Sender.Job.Models.MessagingServiceWhatsApp;

public class MessagingServiceWhatsAppModel
{
    public MessagingServiceWhatsAppModel(SendWhatsAppModel sendWhatsAppSimpleModel)
    {
        this.To = sendWhatsAppSimpleModel.To;
        this.From = sendWhatsAppSimpleModel.From;
        this.Text = sendWhatsAppSimpleModel.Text;
    }

    [JsonProperty("to")]
    public string[] To { get; private set; }

    [JsonProperty("from")]
    public string From { get; private set; }

    [JsonProperty("text")]
    public string Text { get; private set; }

}
