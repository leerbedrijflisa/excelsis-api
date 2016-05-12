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
                entity.Student = JsonConvert.SerializeObject(model.Student);
                entity.Crebo = model.Crebo;
                entity.Cohort = model.Cohort;
                entity.Subject = model.Subject;
                entity.Exam = model.Exam;
                entity.Assessors = JsonConvert.SerializeObject(model.Assessors);
                entity.Assessed = model.Assessed;
                entity.Observations = JsonConvert.SerializeObject(model.Observations);
                entity.Norm = JsonConvert.SerializeObject(model.Norm);

                entity.RowKey = entity.Id.ToString();
                entity.PartitionKey = "";
            }
            else
            {
                entity.Id = model.Id;
                entity.Student = JsonConvert.SerializeObject(model.Student);
                entity.Crebo = model.Crebo;
                entity.Cohort = model.Cohort;
                entity.Subject = model.Subject;
                entity.Exam = model.Exam;
                entity.Assessors = JsonConvert.SerializeObject(model.Assessors);
                entity.Assessed = model.Assessed;
                entity.Observations = JsonConvert.SerializeObject(model.Observations);
                entity.Norm = JsonConvert.SerializeObject(model.Norm);

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
            model.Id = entity.Id;
            model.Student = JsonConvert.DeserializeObject(entity.Student ?? string.Empty);
            model.Crebo = entity.Crebo;
            model.Cohort = entity.Cohort;
            model.Subject = entity.Subject;
            model.Exam = entity.Exam;
            model.Assessors = JsonConvert.DeserializeObject(entity.Assessors ?? string.Empty);
            model.Assessed = entity.Assessed;
            model.Observations = JsonConvert.DeserializeObject(entity.Observations ?? string.Empty);
            model.Norm = JsonConvert.DeserializeObject(entity.Norm ?? string.Empty);

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