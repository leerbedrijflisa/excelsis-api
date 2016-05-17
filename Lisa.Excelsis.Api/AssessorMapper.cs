using Lisa.Common.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lisa.Excelsis.Api
{
    public class AssessorMapper
    {
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
