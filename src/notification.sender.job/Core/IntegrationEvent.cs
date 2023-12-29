using System;

namespace Notification.Sender.Job.Events;

public class IntegrationEvent
{
    public string SpecVersion { get; set; }
    public string Type { get; set; }
    public string Source { get; set; }
    public string Subject { get; set; }
    public string Id { get; set; }
    public string Time { get; set; }
    public string DataContentType { get; set; }
    public string Data { get; set; }
}