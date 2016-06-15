using Lisa.Common.TableStorage;
using Lisa.Common.WebApi;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Lisa.Excelsis.Api
{
    public class AssessorMapper
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
                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.UserName = model.UserName;
                entity.TeacherCode = model.TeacherCode;

                entity.RowKey = entity.UserName;
                entity.PartitionKey = "";
            }
            else
            {
                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.UserName = metadata.UserName;
                entity.TeacherCode = model.TeacherCode;

                entity.PartitionKey = metadata.PartitionKey;
                entity.RowKey = metadata.RowKey;
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
            model.FirstName = entity.FirstName;
            model.LastName = entity.LastName;
            model.UserName = entity.UserName;
            model.TeacherCode = entity.TeacherCode;
            
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