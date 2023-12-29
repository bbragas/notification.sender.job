using Amazon.Lambda.SQSEvents;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notification.Sender.Job.Tests
{
    [TestClass]
    public class FunctionTest
    {
        [Ignore("Need to change the body to be a envelop")]
        [TestMethod]
        public async Task SendEmailToEmailProvider()
        {
            var function = new Function();

            var emailEvent = @"{""Id"":""e48c615c-046f-4333-bb91-0473c055463c"",""SpecVersion"":""1.0"",""Type"":""SendEmail"",""Source"":""Notification.API"",""Subject"":""Envio de Email"",""Time"":""04/06/2022 18:02:02"",""DataContentType"":""application/json"",""Data"":""{\u0022Id\u0022:\u0022e48c615c-046f-4333-bb91-0473c055463c\u0022,\u0022Client\u0022:\u0022feedback-core-api\u0022,\u0022Campaign\u0022:\u0022feedback-core-api\u0022,\u0022ProjectId\u0022:\u00229cd7d6ea-9bc2-41e2-b8f2-c3dd1ffb72f7\u0022,\u0022ProjectEntityId\u0022:\u0022f4ed32ef-3adf-46c7-a2d7-ee795bdaa1cc\u0022,\u0022SenderName\u0022:\u0022B\\u00E3o\u0022,\u0022SenderEmail\u0022:\u0022tchebueno@gmail.com\u0022,\u0022RecipientName\u0022:\u0022Luan\u0022,\u0022RecipientEmail\u0022:\u0022tchebueno@gmail.com\u0022,\u0022Subject\u0022:\u0022b\\u00E3oo\u0022,\u0022Body\u0022:\u0022123456\u0022}""}";

            var sqsEvent = new SQSEvent()
            {
                Records = new List<SQSEvent.SQSMessage>()
                {
                    new SQSEvent.SQSMessage()
                    {
                        Body = emailEvent
                    }
                }
            };
            var execution = await function.HandleSQSEvent(sqsEvent, null);
            Assert.IsNotNull(execution);
        }
    }
}