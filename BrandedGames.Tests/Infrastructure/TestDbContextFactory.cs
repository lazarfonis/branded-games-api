using BrandedGames.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BrandedGames.Tests.Infrastructure;

/// <summary>
/// Builds <see cref="BrandedGamesDbContext"/> instances backed by the EF Core in-memory
/// provider so managers can be exercised without a real PostgreSQL database.
/// </summary>
public static class TestDbContextFactory
{
    /// <summary>
    /// Creates a fresh context over a uniquely named in-memory store. The transaction-ignored
    /// warning is suppressed so code that opens a transaction (for example
    /// <c>GameFormManager.Create</c>) runs against the in-memory provider.
    /// </summary>
    public static BrandedGamesDbContext Create()
    {
        var options = new DbContextOptionsBuilder<BrandedGamesDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        return new BrandedGamesDbContext(options);
    }
}
