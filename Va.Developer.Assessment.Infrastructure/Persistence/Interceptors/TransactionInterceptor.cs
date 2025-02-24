using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Va.Developer.Assessment.Domain.Contracts;
using Va.Developer.Assessment.Domain.Models;

namespace Va.Developer.Assessment.Infrastructure.Persistence.Interceptors
{
    public class TransactionInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null)
            {
                return base.SavedChangesAsync(eventData, result, cancellationToken);
            }
            IEnumerable<EntityEntry<Transaction>> entries = eventData
                .Context.ChangeTracker.Entries<Transaction>()
                .Where(e => e.State != EntityState.Deleted);
            foreach (EntityEntry<Transaction> entry in entries)
            {

                entry.Entity.CaptureDate = DateTime.Now;
            }
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }
    }
}
