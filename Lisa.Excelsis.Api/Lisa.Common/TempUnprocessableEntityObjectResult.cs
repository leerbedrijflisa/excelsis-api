using Microsoft.AspNetCore.Mvc;

namespace Lisa.Excelsis.Api
{
    public class TempUnprocessableEntityObjectResult : ObjectResult
    {
        public TempUnprocessableEntityObjectResult(object value) : base(value)
        {
            StatusCode = 422;
        }
    }
}
