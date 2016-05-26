using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lisa.Excelsis.Api
{
    [Route("test")]
    public class test : Controller
    {
        [HttpGet]
        public ActionResult k()
        {
            return new OkObjectResult("Hoi");
        }
    }
}
