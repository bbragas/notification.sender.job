using Flurl.Http;
using Microsoft.Extensions.Options;
using Notification.Sender.Job.Core;
using Notification.Sender.Job.Models.MessagingServiceSms;
using Serilog;
using System.Threading.Tasks;

namespace Notification.Sender.Job.Commands;

public class SendSmsCommandHandler : ICommandHandler<SendSmsCommand>
{
    private readonly MessagingServiceConfig _messagingServiceConfig;
    private readonly ILogger _logger;

    public SendSmsCommandHandler(IOptions<MessagingServiceConfig> messagingServiceConfig, ILogger logger)
    {
        _messagingServiceConfig = messagingServiceConfig.Value;
        _logger = logger;
    }

    public async Task ExecuteAsync(SendSmsCommand command)
    {
        var model = new SendSmsModel(command.SenderName ?? _messagingServiceConfig.FromSender,
                                           new string[] { command.To },
                                           command.Text);

        var url = $"{_messagingServiceConfig.BaseUrl}/sms/1/text/single";
        var request = new MessagingServiceSmsModel(model);

        _logger.Information($"[{command.Id}] Post to {url}");

        var response = await url.WithBasicAuth(_messagingServiceConfig.User,
                                               _messagingServiceConfig.Password)
                                .PostJsonAsync(request);

        if (response.StatusCode >= 400)
        {
            var content = await response.ResponseMessage.Content.ReadAsStringAsync();
            _logger.Error("Response calling iagent provider {Content}", content);
            throw new System.Exception("Error sending email");
        }
    }
}
