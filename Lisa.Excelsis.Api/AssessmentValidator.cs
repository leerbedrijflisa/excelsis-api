using Lisa.Common.WebApi;
using System.Linq;

namespace Lisa.Excelsis.Api
{
    public class AssessmentValidator : Validator
    {
        private void CheckDigits(string fieldName, object value)
        {
            string valueString = value.ToString();
            bool digitsOnly = valueString.All(char.IsDigit);
            if (!digitsOnly)
            {
                var error = new Error
                {
                    Code = 10,
                    Message = $"The field '{fieldName}' doesn't expects the value '{valueString}' to have characters. Only numbers are allowed.",
                    Values = new
                    {
                        Field = fieldName,
                        Value = valueString,
                        Allowed = "Numbers"
                    }
                };
                Result.Errors.Add(error);
            }
        }
         
        protected override void ValidatePatch()
        {
            Ignore("id");
            Allow("student.name");
            Allow("student.number");
            Allow("crebo");
            Allow("cohort");
            Allow("subject");
            Allow("exam");
            Allow("assessors");
            Allow("assessed");
            Allow("observations");
        }

        protected override void ValidateModel()
        {
            Ignore("id");
            Optional("student.name", NotEmpty, TypeOf(DataTypes.String));
            Optional("student.number", NotEmpty, TypeOf(DataTypes.String));
            Optional("crebo", CheckDigits, NotEmpty, Length(5), TypeOf(DataTypes.String));
            Optional("cohort", CheckDigits, NotEmpty, Length(4), TypeOf(DataTypes.String));
            Required("subject", NotEmpty, TypeOf(DataTypes.String));
            Required("exam", NotEmpty, TypeOf(DataTypes.String));
            //Optional("assessors", NotEmpty, IsArray(DataTypes.Object));
            Optional("assessors.firstName", NotEmpty, TypeOf(DataTypes.String));
            Optional("assessors.lastName", NotEmpty, TypeOf(DataTypes.String));
            Optional("assessors.userName", NotEmpty, TypeOf(DataTypes.String));
            Optional("assessors.teacherCode", NotEmpty, TypeOf(DataTypes.String));
            Optional("assessed", NotEmpty);
            Optional("observations.value", NotEmpty, TypeOf(DataTypes.String), OneOf("notRated", "notSeen", "seen"));
            Optional("observations.remark", NotEmpty, TypeOf(DataTypes.String));
            Optional("observations.markings", NotEmpty, IsArray(DataTypes.String));
            Required("observations.criterion.description", NotEmpty, TypeOf(DataTypes.String));
            Required("observations.criterion.details", NotEmpty, TypeOf(DataTypes.String));
            Required("observations.criterion.rating", NotEmpty, TypeOf(DataTypes.String), OneOf("fail", "pass", "excellent"));
            Required("observations.criterion.category", NotEmpty, TypeOf(DataTypes.String));
            Required("norm.excellent", NotEmpty, IsArray(DataTypes.Number));
            Required("norm.pass", NotEmpty, IsArray(DataTypes.Number));
            Required("norm.fail", NotEmpty, IsArray(DataTypes.Number));
        }
    }
}