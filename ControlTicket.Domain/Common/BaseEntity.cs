namespace ControlTicket.Domain.Common;

/// <summary>
/// Base class for all domain entities. Provides identity and audit timestamps.
/// </summary>
public class BaseEntity
{
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}