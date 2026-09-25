using CRMSystem.Helpers;
using CRMSystem.Models.Entities;
using CRMSystem.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CRMSystem.Interceptors
{
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserServices _currentUserServices;

        public AuditSaveChangesInterceptor(ICurrentUserServices currentUserServices)
        {
            _currentUserServices = currentUserServices;
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            AddAuditLogs(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            AddAuditLogs(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void AddAuditLogs(DbContext? context)
        {
            if (context == null)
                return;

            context.ChangeTracker.DetectChanges(); 

            var entries = context.ChangeTracker.Entries()
                .Where(e => IsAuditableEntity(e.Entity))
                .Where(e => e.State == EntityState.Added
                    || e.State == EntityState.Modified
                    || e.State == EntityState.Deleted)
                .ToList();

            if (entries.Count == 0)
                return;

            var userId = _currentUserServices.UserId;

            if (userId == Guid.Empty)
                return;

            foreach (var entry in entries)
            {
                var entityId = GetEntityId(entry);

                if (entityId == Guid.Empty)
                    continue;

                var organizationId = GetOrganizationId(entry);

                if (organizationId == Guid.Empty)
                    organizationId = _currentUserServices.OrganizationId;

                if (organizationId == Guid.Empty)
                    continue;

                var auditLog = new AuditLog
                {
                    Id = Guid.NewGuid(),
                    OrganizationId = organizationId,
                    UserId = userId,
                    EntityType = entry.Metadata.ClrType.Name,
                    EntityId = entityId,
                    Action = GetAction(entry),
                    OldValues = GetOldValues(entry),
                    NewValues = GetNewValues(entry),
                    CreatedAt = DateTime.UtcNow
                };

                context.Set<AuditLog>().Add(auditLog);
            }
        }

        private static bool IsAuditableEntity(object entity)
        {
            return entity is Customer
                or CustomerContact
                or CustomerAddress
                or Lead
                or LeadSource
                or LeadStatus
                or Pipeline
                or PipelineStage
                or Opportunity
                or TaskItem
                or Activity
                or Note;
        }

        private static Guid GetEntityId(EntityEntry entry)
        {
            var property = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "Id");

            if (property?.CurrentValue is Guid currentId && currentId != Guid.Empty)
                return currentId;

            if (property?.OriginalValue is Guid originalId && originalId != Guid.Empty)
                return originalId;

            return Guid.Empty;
        }

        private static Guid GetOrganizationId(EntityEntry entry)
        {
            var property = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "OrganizationId");

            if (property?.CurrentValue is Guid currentId && currentId != Guid.Empty)
                return currentId;

            if (property?.OriginalValue is Guid originalId && originalId != Guid.Empty)
                return originalId;

            return Guid.Empty;
        }

        private static string GetAction(EntityEntry entry)
        {
            return entry.State switch
            {
                EntityState.Added => "CREATE",
                EntityState.Modified => "UPDATE",
                EntityState.Deleted => "DELETE",
                _ => "UNKNOWN"
            };
        }

        private static string? GetOldValues(EntityEntry entry)
        {
            if (entry.State == EntityState.Added)
                return null;

            return AuditLogHelper.GetValues(entry, false);
        }

        private static string? GetNewValues(EntityEntry entry)
        {
            if (entry.State == EntityState.Deleted)
                return null;

            return AuditLogHelper.GetValues(entry, true);
        }
    }
}