using Flurl.Http;
using Microsoft.Extensions.Options;
using Notification.Sender.Job.Core;
using Notification.Sender.Job.Models.IAgentSmtp;
using Serilog;
using System.Threading.Tasks;

namespace Notification.Sender.Job.Commands;

public class SendEmailCommandHandler : ICommandHandler<SendEmailCommand>
{
    private readonly IAgentSmtpConfig _agentConfig;
    private readonly ILogger _logger;

    public SendEmailCommandHandler(IOptions<IAgentSmtpConfig> agentConfig, ILogger logger)
    {
        _agentConfig = agentConfig.Value;
        _logger = logger;
    }
    public async Task ExecuteAsync(SendEmailCommand command)
    {
        var model = new SendEmailModel(command.To, command.Name, command.Subject, command.Html);

        var url = $"{_agentConfig.BaseUrl}/send/";

        var from = new IAgentEmailModel.Sender()
        {
            Name = command.SenderName ?? _agentConfig.NameSender,
            Email = _agentConfig.FromSender
        };

        var request = new IAgentEmailModel(_agentConfig.ApiKey, _agentConfig.ApiUser, model, from);

        _logger.Information("[{Id}] Post to {Url}", command.Id, url);

        var response = await url.PostJsonAsync(request);
        if (response.StatusCode >= 400)
        {
            var content = await response.ResponseMessage.Content.ReadAsStringAsync();
            _logger.Error("Response calling iagent provider {Content}", content);
            throw new System.Exception("Error sending email");
        }
    }
}
