namespace Integrations.ModernService.Domain.Entities;

public class LinkedInEventMessage
{
    public string TopicCode { get; set; } = string.Empty;
    public LinkedInEventMessageBody Message { get; set; } = null!;
    public int OrgId { get; set; }
    public string Site { get; set; } = string.Empty;
    public string MessageId { get; set; } = string.Empty;
    public string Encryption { get; set; } = string.Empty;
    public string CorrelationId { get; set; } = string.Empty;
}

public class LinkedInEventMessageBody
{
    public string Data { get; set; } = string.Empty;
    public int OrgId { get; set; }
    public int UserId { get; set; }
    public string EventCode { get; set; } = string.Empty;
    public DateTime UtcEventDate { get; set; }
}
