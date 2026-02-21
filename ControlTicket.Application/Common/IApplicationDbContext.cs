namespace ControlTicket.Application.Common;

/// <summary>
/// Abstraction for the database context.
/// Infrastructure provides the concrete EF Core (or other ORM) implementation.
/// </summary>
public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
