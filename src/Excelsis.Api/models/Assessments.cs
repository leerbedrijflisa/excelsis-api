using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Excelsis.Api.models
{
    public class Assessments : TableEntity
    {
        public Guid Id { get; set; }
        public string Student { get; set; }
        public string name { get; set; }
        public string assessor { get; set; }
        public string date { get; set; }
        public string result { get; set; }
    }
}
