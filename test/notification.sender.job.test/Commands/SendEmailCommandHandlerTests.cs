using AutoFixture;
using Flurl.Http.Testing;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notification.Sender.Job.Commands;
using Notification.Sender.Job.Core;
using Notification.Sender.Job.Models.IAgentSmtp;
using NSubstitute;
using Serilog;
using System.Net.Http;
using System.Threading.Tasks;

namespace notification.sender.job.tests.Commands
{
    [TestClass]
    public class SendEmailCommandHandlerTests
    {
        static Fixture Fixture = new Fixture();
        string baseUrl = "http://test.com";
        SendEmailCommandHandler handler;
        IAgentSmtpConfig config;
        HttpTest httpclient;

        [TestInitialize]
        public void Initalize()
        {
            httpclient = new HttpTest();
            var logger = Substitute.For<ILogger>();
            config = Fixture.Create<IAgentSmtpConfig>();
            config.BaseUrl = baseUrl;

            var options = Substitute.For<IOptions<IAgentSmtpConfig>>();
            options.Value.Returns(config);

            handler = new SendEmailCommandHandler(options, logger);
        }

        [TestMethod]
        public async Task VerifyIfCallTheEndpointCorrectely()
        {
            // Arrange
            var command = Fixture.Create<SendEmailCommand>();

            // Act
            await handler.ExecuteAsync(command);

            // Assert
            httpclient.ShouldHaveCalled($"{baseUrl}/send/")
                         .WithVerb(HttpMethod.Post)
                         .WithContentType("application/json")
                         .Times(1);
        }

        [TestMethod]
        public async Task VerifyIfSenderNameIsDefaultIfCommandSenderNameIsNull()
        {
            // Arrange
            var command = Fixture.Create<SendEmailCommand>();
            command.SenderName = null;

            var model = new SendEmailModel(command.To, command.Name, command.Subject, command.Html);
            var from = new IAgentEmailModel.Sender()
            {
                Name =  config.NameSender,
                Email = config.FromSender
            };

            var request = new IAgentEmailModel(config.ApiKey, config.ApiUser, model, from);

            // Act
            await handler.ExecuteAsync(command);

            // Assert
            httpclient.ShouldHaveCalled($"{baseUrl}/send/")
                         .WithVerb(HttpMethod.Post)
                         .WithRequestJson(request)
                         .WithContentType("application/json")
                         .Times(1);
        }

        [TestMethod]
        public async Task VerifyIfSenderNameFilledThenCommandSenderNameIsFilled()
        {
            // Arrange
            var command = Fixture.Create<SendEmailCommand>();

            var model = new SendEmailModel(command.To, command.Name, command.Subject, command.Html);
            var from = new IAgentEmailModel.Sender()
            {
                Name = command.SenderName,
                Email = config.FromSender
            };

            var request = new IAgentEmailModel(config.ApiKey, config.ApiUser, model, from);

            // Act
            await handler.ExecuteAsync(command);

            // Assert
            httpclient.ShouldHaveCalled($"{baseUrl}/send/")
                         .WithVerb(HttpMethod.Post)
                         .WithRequestJson(request)
                         .WithContentType("application/json")
                         .Times(1);
        }

    }
}
