using Lisa.Common.WebApi;
using Newtonsoft.Json.Linq;

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
            Required("subject", NotEmpty);
            Required("exam", NotEmpty);
            Optional("assessors", NotEmpty);
            Optional("assessed", NotEmpty);
            Optional("observations", NotEmpty);
            Required("norm", NotEmpty);
            Required("criteria", NotEmpty);
        }
    }
}