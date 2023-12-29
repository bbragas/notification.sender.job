using Notification.Sender.Job.Commands;
using System.Threading.Tasks;

namespace Notification.Sender.Job.Core;

public interface ICommandHandler<in T> where T : Command
{
    Task ExecuteAsync(T command);
}
