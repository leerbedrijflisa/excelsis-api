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
            Allow("norm");
        }
        private void ValidateList(string fieldName, object value)
        {
            string[] expectedValues = new string[] { "seen", "not seen", "not rated" };
            if ((value == null) ||
                (value is JArray) && ((JArray)value).Count == 0)
            {
                var error = new Error
                {
                    Code = ErrorCode.EmptyValue,
                    Message = $"The field '{fieldName}' should not be empty",
                    Values = new
                    {
                        Field = fieldName
                    }
                };
                Result.Errors.Add(error);
                return;
            }

            foreach(string val in (JArray)value)
            {
                bool expected = false;
                foreach(string expectedValue in expectedValues)
                {
                    if (val == expectedValue)
                    {
                        expected = true;
                    }
                }
                if (expected == false)
                {
                    var error = new Error
                    {
                        Code = 10,
                        Message = $"The field '{fieldName}' doesn't expects the value '{val}' in it's array. Only 'seen', 'not seen' and 'not rated' allowed.",
                        Values = new
                        {
                            Field = fieldName
                        }
                    };
                    Result.Errors.Add(error);
                }
            }
        }
        protected override void ValidateModel()
        {
            Ignore("id");
            Optional("studentName", NotEmpty);
            Optional("studentNumber", NotEmpty);
            Optional("crebo", NotEmpty);
            Optional("cohort", NotEmpty);
            Required("examSubject", NotEmpty);
            Required("examName", NotEmpty);
            Optional("assessors", NotEmpty);
            Optional("assessed", NotEmpty);
            Optional("observations", ValidateList);
            Optional("norm", NotEmpty);
        }
    }
}