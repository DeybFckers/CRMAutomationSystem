using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace CRMSystem.Helpers
{
    public static class AuditLogHelper
    {
        private static readonly HashSet<string> ExcludedProperties = new(StringComparer.OrdinalIgnoreCase)
        {
            "PasswordHash",
            "SecurityStamp",
            "ConcurrencyStamp"
        };

        public static string GetValues(EntityEntry entry, bool currentValues)
        {
            var values = new Dictionary<string, object?>();

            foreach (var property in entry.Properties)
            {
                if (ExcludedProperties.Contains(property.Metadata.Name))
                    continue;

                if (entry.State == EntityState.Modified && !property.IsModified)
                    continue;

                values[property.Metadata.Name] = currentValues
                    ? property.CurrentValue
                    : property.OriginalValue;
            }

            return JsonSerializer.Serialize(values);
        }
    }
}