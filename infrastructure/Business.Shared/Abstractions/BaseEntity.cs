using Infrastructure.Constants;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Abstractions
{

    public abstract class BaseEntity : ITableEntity 
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public DateTime CreatedAtDateTimeUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAtDateTimeUtc { get; set; } = DateTime.UtcNow;
        
        public string CreatedBy { get; set; }

        public string ArchivedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string DeletedBy { get; set; }

        public string AssignedTo { get; set; }

        public string TsvToken { get; set; }

        public string SystemType { get; set; }

        public string Url { get; set; }
      
        public string PartitionKey { get; set; } = typeof(BaseEntity).Name;
        public string RowKey { get => Id; set => _ = Id; }
        public DateTimeOffset Timestamp { get => DateTime.UtcNow; set => _ = DateTime.UtcNow; }
        public string ETag { get => Id; set => _ = Id; }

        public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            var baseEntity = TableEntity.ConvertBack<BaseEntity>(properties, operationContext);
            // Do the memberwise clone for this object from the returned BaseModel object
            baseEntity.MemberwiseClone();
        }

        public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            IDictionary<string, EntityProperty> flattenedProperties = TableEntity.Flatten(this, operationContext);
            flattenedProperties.Remove("PartitionKey");
            flattenedProperties.Remove("RowKey");
            flattenedProperties.Remove("Timestamp");
            flattenedProperties.Remove("ETag");
            var objectProperties = GetType().GetProperties();
            foreach (var prop in objectProperties.Where(m => m.PropertyType == typeof(decimal)))
            {
                flattenedProperties.Add(InfrastructureConstants.DecimalPrefix + prop.Name, new EntityProperty(prop.GetValue(this, null).ToString()));
            }
            return flattenedProperties;
        }
    }
}
