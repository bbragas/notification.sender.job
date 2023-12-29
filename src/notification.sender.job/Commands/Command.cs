using System;

namespace Notification.Sender.Job.Commands;

public abstract class Command
{
    public Guid Id { get; set; }
}
