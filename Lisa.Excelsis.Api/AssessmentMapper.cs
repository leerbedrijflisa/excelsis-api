using Lisa.Common.TableStorage;
using Lisa.Common.WebApi;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;

namespace Lisa.Excelsis.Api
{
    public class AssessmentMapper
    {
        public static ITableEntity ToEntity(dynamic model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Model");
            }

            dynamic entity = new DynamicEntity();

            dynamic metadata = model.GetMetadata();
            if (metadata == null)
            {
                entity.Id = Guid.NewGuid();
                entity.Observations = JsonConvert.SerializeObject(model.Observations);
                entity.RowKey = entity.Id.ToString();
                entity.PartitionKey = "";
            }
            else
            {
                entity.Id = model.Id;
                entity.Observations = JsonConvert.SerializeObject(model.Observations);
                entity.PartitionKey = model.PartitionKey;
                entity.RowKey = model.RowKey;
            }

            return entity;
        }

        public static DynamicModel ToModel(dynamic entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("Entity");
            }

            dynamic model = new DynamicModel();
            model.Id = entity.Id;
            model.Observations = JsonConvert.DeserializeObject(entity.Observations);

            var metadata = new
            {
                PartitionKey = entity.PartitionKey,
                RowKey = entity.RowKey
            };
            model.SetMetadata(metadata);

            return model;
        }
    }
}