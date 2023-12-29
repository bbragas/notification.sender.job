using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notification.Api.Infrastructure.Messages.v1;
using Notification.Sender.Job.Commands;
using Notification.Sender.Job.Core;
using Notification.Sender.Job.Events;
using NSubstitute;
using System;
using System.Threading.Tasks;

namespace notification.sender.job.tests.Commands
{
    [TestClass]
    public class CommandHandlerFactoryTests
    {
        static Fixture Fixture = new Fixture();

        ICommandHandler<SendSmsCommand> smsCommandHandler;
        ICommandHandler<SendEmailCommand> emailCommandHandler;

        CommandHandlerFactory handler;

        [TestInitialize]
        public void Initalize()
        {
            var serviceProvider = new ServiceCollection();

            smsCommandHandler = Substitute.For<ICommandHandler<SendSmsCommand>>();
            serviceProvider.AddScoped((p) => smsCommandHandler);

            emailCommandHandler = Substitute.For<ICommandHandler<SendEmailCommand>>();
            serviceProvider.AddScoped((p) => emailCommandHandler);


            handler = new CommandHandlerFactory(serviceProvider.BuildServiceProvider());
        }

        [TestMethod]
        public async Task VerifyIfTheEventSendSmsIsHandelingTheCorrectlyHandle()
        {
            // Arrange
            var command = Fixture.Create<IntegrationEvent>();
            command.Type = typeof(SendSmsEvent).FullName;
            command.Data = Fixture.Create<SendSmsEvent>().Serializer();

            // Act
            await handler.ResolveHandlerExecute(command);

            // Assert
            await smsCommandHandler.Received().ExecuteAsync(Arg.Any<SendSmsCommand>());
            await emailCommandHandler.DidNotReceive().ExecuteAsync(Arg.Any<SendEmailCommand>());
        }

        [TestMethod]
        public async Task VerifyIfTheEventSendEmailIsHandelingTheCorrectlyHandle()
        {
            // Arrange
            var command = Fixture.Create<IntegrationEvent>();
            command.Type = typeof(SendEmailEvent).FullName;
            command.Data = Fixture.Create<SendEmailEvent>().Serializer();

            // Act
            await handler.ResolveHandlerExecute(command);

            // Assert
            await smsCommandHandler.DidNotReceive().ExecuteAsync(Arg.Any<SendSmsCommand>());
            await emailCommandHandler.Received().ExecuteAsync(Arg.Any<SendEmailCommand>());
        }

        [TestMethod]
        public async Task VerifyIfUnmappedEventIsHandelingThenThrowException()
        {
            // Arrange
            var command = Fixture.Create<IntegrationEvent>();
            command.Type = "NONE";

            // Act
            var act = () => handler.ResolveHandlerExecute(command);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("Notfound event type");
        }
    }
}
