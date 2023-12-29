using AutoFixture;
using Flurl.Http.Testing;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notification.Sender.Job.Commands;
using Notification.Sender.Job.Core;
using Notification.Sender.Job.Models.MessagingServiceSms;
using NSubstitute;
using Serilog;
using System.Net.Http;
using System.Threading.Tasks;

namespace notification.sender.job.tests.Commands
{
    [TestClass]
    public class SendSmsCommandHandlerTests
    {
        static Fixture Fixture = new Fixture();
        string baseUrl = "http://test.com";

        SendSmsCommandHandler handler;
        MessagingServiceConfig config;
        HttpTest httpclient;

        [TestInitialize]
        public void Initalize()
        {
            httpclient = new HttpTest();
            var logger = Substitute.For<ILogger>();
            config = Fixture.Create<MessagingServiceConfig>();
            config.BaseUrl = baseUrl;

            var options = Substitute.For<IOptions<MessagingServiceConfig>>();
            options.Value.Returns(config);

            handler = new SendSmsCommandHandler(options, logger);


        }

        [TestMethod]
        public async Task VerifyIfCallTheEndpointCorrectely()
        {
            // Arrange
            var command = Fixture.Create<SendSmsCommand>();

            // Act
            await handler.ExecuteAsync(command);

            // Assert
            httpclient.ShouldHaveCalled($"{baseUrl}/sms/1/text/single")
                         .WithVerb(HttpMethod.Post)
                         .WithContentType("application/json")
                         .Times(1);
        }

        [TestMethod]
        public async Task VerifyIfCallTheEndpointWithCredentialsCorrectely()
        {
            // Arrange
            var command = Fixture.Create<SendSmsCommand>();

            // Act
            await handler.ExecuteAsync(command);

            // Assert
            httpclient.ShouldHaveCalled($"{baseUrl}/sms/1/text/single")
                         .WithVerb(HttpMethod.Post)
                         .WithBasicAuth(config.User, config.Password)
                         .WithContentType("application/json")
                         .Times(1);

        }

        [TestMethod]
        public async Task VerifyIfFromIsNullThenFromSenderMustBeAsDefault()
        {
            // Arrange
            var command = Fixture.Create<SendSmsCommand>();
            command.SenderName = null;

            var model = new SendSmsModel(config.FromSender,
                                          new string[] { command.To },
                                          command.Text);

            // Act
            await handler.ExecuteAsync(command);

            // Assert
            httpclient.ShouldHaveCalled($"{baseUrl}/sms/1/text/single")
                         .WithVerb(HttpMethod.Post)
                         .WithRequestJson(new MessagingServiceSmsModel(model))
                         .WithContentType("application/json")
                         .Times(1);
        }

        [TestMethod]
        public async Task VerifyIfFromIsNotNullThenFromSenderMustBeAsTheCommand()
        {
            // Arrange
            var command = Fixture.Create<SendSmsCommand>();
            var model = new SendSmsModel(command.SenderName,
                                          new string[] { command.To },
                                          command.Text);


            // Act
            await handler.ExecuteAsync(command);

            // Assert
            httpclient.ShouldHaveCalled($"{baseUrl}/sms/1/text/single")
                         .WithVerb(HttpMethod.Post)
                         .WithRequestJson(new MessagingServiceSmsModel(model))
                         .WithContentType("application/json")
                         .Times(1);
        }

    }
}
