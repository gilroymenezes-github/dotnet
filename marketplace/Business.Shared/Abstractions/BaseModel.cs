
using Business.Shared.Statics;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Shared.Abstractions
{

    public abstract class BaseModel : TableEntity 
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public DateTime CreatedAtDateTimeUtc { get; set; }

        public DateTime UpdatedAtDateTimeUtc { get; set; }
        
        public string CreatedBy { get; set; }

        public string ArchivedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string DeletedBy { get; set; }

        public string AssignedTo { get; set; }

        public string TsvToken { get; set; }

        public string SystemType { get; set; }

        public string Url { get; set; }

        /// <summary>
        /// Azure tables does not understand decimals
        /// </summary>
        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var entityProperties = base.WriteEntity(operationContext);
            var objectProperties = GetType().GetProperties();
            foreach (var prop in objectProperties.Where(m => m.PropertyType == typeof(decimal)))
            {
                entityProperties.Add(ApplicationConstant.DecimalPrefix + prop.Name, new EntityProperty(prop.GetValue(this, null).ToString()));
            }
            return entityProperties;
        }

        //public string PartitionKey { get => Id; set => _ = Id; }
        //public string RowKey { get => Name; set => _ = Name; }
        //public DateTimeOffset Timestamp { get => DateTime.UtcNow; set => _ = DateTime.UtcNow; }
        //public string ETag { get => Id; set => _ = Id; }

        //public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        //{
        //    BaseModel baseEntity = TableEntity.ConvertBack<BaseModel>(properties, operationContext);
        //    // Do the memberwise clone for this object from the returned BaseModel object
        //    baseEntity.MemberwiseClone();
        //}

        //public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        //{
        //    IDictionary<string, EntityProperty> flattenedProperties = TableEntity.Flatten(this, operationContext);
        //    flattenedProperties.Remove("PartitionKey");
        //    flattenedProperties.Remove("RowKey");
        //    flattenedProperties.Remove("Timestamp");
        //    flattenedProperties.Remove("ETag");
        //    return flattenedProperties;
        //}
    }
}
