namespace Lisa.Excelsis.Api
{
    public class Filter
    {
        public Filter(string type, string value)
        {
            Type = type;
            Value = value;
        }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
