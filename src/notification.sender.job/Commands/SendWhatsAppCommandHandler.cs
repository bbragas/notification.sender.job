using Flurl.Http;
using Microsoft.Extensions.Options;
using Notification.Sender.Job.Core;
using Notification.Sender.Job.Models.MessagingServiceWhatsApp;
using Serilog;
using System.Threading.Tasks;

namespace Notification.Sender.Job.Commands;

public class SendWhatsAppCommandHandler : ICommandHandler<SendWhatsAppCommand>
{
    private readonly WhatsAppServiceConfig _whatsAppServiceConfig;
    private readonly ILogger _logger;

    public SendWhatsAppCommandHandler(IOptions<WhatsAppServiceConfig> whatsAppServiceConfig, ILogger logger)
    {
        _whatsAppServiceConfig = whatsAppServiceConfig.Value;
        _logger = logger;
    }

    public async Task ExecuteAsync(SendWhatsAppCommand command)
    {
        var model = new SendWhatsAppModel(command.SenderName ?? _whatsAppServiceConfig.FromSender,
                                           new string[] { command.To },
                                           command.Text);

        var url = $"{_whatsAppServiceConfig.BaseUrl}/";
        var request = new MessagingServiceWhatsAppModel(model);

        _logger.Information($"[{command.Id}] Post to {url}");

        var response = await url.WithBasicAuth(_whatsAppServiceConfig.User,
                                               _whatsAppServiceConfig.Password)
                                .PostJsonAsync(request);

        if (response.StatusCode >= 400)
        {
            var content = await response.ResponseMessage.Content.ReadAsStringAsync();
            _logger.Error("Response calling iagent provider {Content}", content);
            throw new System.Exception("Error sending email");
        }
    }
}
