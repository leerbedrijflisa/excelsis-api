using Lisa.Common.WebApi;

namespace Lisa.Excelsis.Api
{
    public class AssessmentValidator : Validator
    {
        protected override void ValidatePatch()
        {
            Ignore("id");
            Allow("studentName");
            Allow("studentNumber");
            Allow("crebo");
            Allow("cohort");
            Allow("examSubject");
            Allow("examName");
            Allow("assessors");
            Allow("assessed");
            Allow("observations");
            Allow("criteria");
        }

        protected override void ValidateModel()
        {
            Ignore("id");
            Optional("student.name", NotEmpty);
            Optional("student.number", NotEmpty);
            Optional("crebo", NotEmpty, Length(5));
            Optional("cohort", NotEmpty, Length(4));
            Required("examSubject", NotEmpty);
            Required("examName", NotEmpty);
            //Optional("assessors", NotEmpty, IsArray(DataTypes.Object));
            Optional("assessors.firstName", NotEmpty, TypeOf(DataTypes.String));
            Optional("assessors.lastName", NotEmpty, TypeOf(DataTypes.String));
            Optional("assessors.userName", NotEmpty, TypeOf(DataTypes.String));
            Optional("assessors.teacherCode", NotEmpty, TypeOf(DataTypes.String));
            Optional("assessed", NotEmpty);
            Optional("observations.value", NotEmpty, TypeOf(DataTypes.String));
            Optional("observations.remark", NotEmpty, TypeOf(DataTypes.String));
            Optional("observations.markings", NotEmpty, TypeOf(DataTypes.Array));
            Required("norm.excellent", NotEmpty, IsArray(DataTypes.Number));
            Required("norm.pass", NotEmpty, IsArray(DataTypes.Number));
            Required("norm.fail", NotEmpty, IsArray(DataTypes.Number));
            Required("criteria.description", NotEmpty, TypeOf(DataTypes.String));
            Required("criteria.details", NotEmpty, TypeOf(DataTypes.String));
            Required("criteria.rating", NotEmpty, TypeOf(DataTypes.String));
        }
    }
}