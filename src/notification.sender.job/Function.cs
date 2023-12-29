using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Microsoft.Extensions.DependencyInjection;
using Notification.Sender.Job.Commands;
using Notification.Sender.Job.Core;
using Notification.Sender.Job.Events;
using Notification.Sender.Job.IoC;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using static Amazon.Lambda.SQSEvents.SQSBatchResponse;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace Notification.Sender.Job;

public class Function
{
    private readonly IServiceProvider _serviceProvider;

    public Function()
    {
        _serviceProvider = IoCConfig.Configure();
    }

    public async Task<SQSBatchResponse> HandleSQSEvent(SQSEvent sqsEvent, ILambdaContext _)
    {
        var stopwatch = new Stopwatch();
        var logger = _serviceProvider.GetService<ILogger>();

        try
        {
            if (sqsEvent.Records == null) return new SQSBatchResponse();

            return await Handle(sqsEvent, logger);
        }
        finally
        {
            stopwatch.Stop();
            logger.Information($"Email has been sent in: {stopwatch.ElapsedMilliseconds / 1000} seconds");
        }
    }

    private async Task<SQSBatchResponse> Handle(SQSEvent sqsEvent, ILogger logger)
    {
        logger.Information("Beginning to process {Count} records...", sqsEvent.Records.Count);
        var sqsFailMessageResponse = new List<BatchItemFailure>();

        foreach (var record in sqsEvent.Records)
        {
            var error = await Execute(record);
            if (error != null) sqsFailMessageResponse.Add(error);
        }

        logger.Information("Processed {Count} records with failed {FailedCount}.", sqsEvent.Records.Count, sqsFailMessageResponse.Count);
        return new SQSBatchResponse(sqsFailMessageResponse);
    }

    private async Task<BatchItemFailure> Execute(SQSEvent.SQSMessage record)
    {
        try
        {
            var integrationEvent = record.Body.Deserializer<IntegrationEvent>();

            var handler = _serviceProvider.GetService<ICommandHandlerFactory>();
            await handler.ResolveHandlerExecute(integrationEvent);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Error executing the lambda: {MessageId} - {Message}", record.MessageId, ex.Message, ex);

            return new BatchItemFailure
            {
                ItemIdentifier = record.MessageId
            };
        }

        return null;
    }
}
