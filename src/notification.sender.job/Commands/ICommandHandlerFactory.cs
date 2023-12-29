using Notification.Sender.Job.Events;
using System.Threading.Tasks;

namespace Notification.Sender.Job.Commands
{
    public interface ICommandHandlerFactory
    {
        Task ResolveHandlerExecute(IntegrationEvent @event);
    }
}