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
                //entity.StudentName = model.Student.Name;
                //entity.StudentNumber = model.Student.Number;
                entity.Crebo = model.Crebo;
                entity.Cohort = model.Cohort;
                entity.ExamSubject = model.Exam.Subject;
                entity.ExamName = model.Exam.Name;
                entity.Assessors = JsonConvert.SerializeObject(model.Assessors);
                entity.Assessed = model.Assessed;
                
                entity.RowKey = entity.Id.ToString();
                entity.PartitionKey = "";
            }
            else
            {
                entity.Id = model.Id;
                entity.Observations = JsonConvert.SerializeObject(model.Observations);
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
            model.Observations = JsonConvert.DeserializeObject(entity.Observations);
            model.Student.Name = entity.StudentName;
            model.Student.Number = entity.StudentNumber;
            model.Crebo = entity.Crebo;
            model.Cohort = entity.Cohort;
            model.Exam.Subject = entity.ExamSubject;
            model.Exam.Name = entity.ExamName;
            model.Assessors = JsonConvert.DeserializeObject(entity.Assessors);
            model.Assessed = entity.Assessed;

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