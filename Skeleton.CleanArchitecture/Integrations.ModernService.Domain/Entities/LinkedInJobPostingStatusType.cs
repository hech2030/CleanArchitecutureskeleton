namespace Integrations.ModernService.Domain.Entities;

public class LinkedInJobPostingStatusType : StatusEnum<string, LinkedInJobPostingStatusType>
{
    /// <summary>
    /// Required static constructor.
    /// </summary>  
    static LinkedInJobPostingStatusType()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LinkedInJobPostingStatusType"/>.
    /// </summary>
    /// <param name="code">The code value.</param>
    /// <param name="externalCode">The friendly name.</param>
    private LinkedInJobPostingStatusType(string code, string externalCode)
        : base(code, externalCode: externalCode)
    {
    }

    /// <summary>
    /// The job status is listed. Accepting applications. Live on LinkedIn.
    /// </summary>
    public static readonly LinkedInJobPostingStatusType Listed = new("listed", "LISTED");

    /// <summary>
    /// The job status is closed. No longer accepting applications, or unlisted.
    /// </summary>
    public static readonly LinkedInJobPostingStatusType NotListed = new("not-listed", "NOT_LISTED");

    /// <summary>
    /// The job has not been sent.
    /// </summary>
    public static readonly LinkedInJobPostingStatusType NotSent = new("not-sent", "NOT_SENT");

    /// <summary>
    /// The job status is in progress. Currently being reviewed or processed.
    /// </summary>
    public static readonly LinkedInJobPostingStatusType InProgress = new("in-progress", "IN_PROGRESS");

    /// <summary>
    /// The job status is unknown.
    /// </summary>
    public static readonly LinkedInJobPostingStatusType Unknown = new("unknown", "UNKNOWN");

    /// <summary>
    /// The job status is failed as a result of an outage.
    /// </summary>
    public static readonly LinkedInJobPostingStatusType Failure = new("failure", "FAILURE");

    /// <summary>
    /// The job status is closing as a result of an outage.
    /// </summary>
    public static readonly LinkedInJobPostingStatusType Closing = new("closing", "CLOSING");
}
