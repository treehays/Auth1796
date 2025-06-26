using Auth1796.Core.Application.Utilities.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Auth1796.Infrastructure.Persistence.Context;

/// <summary>
/// Represents the application's database context.
/// Always set entity configurations in the Configuration folder.
/// </summary>
public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, IHttpContextAccessor httpContextAccessor)
        : base(options, currentUser, serializer, dbSettings, httpContextAccessor)
    {
    }

    public DbSet<ApiUser> ApiUsers => Set<ApiUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
