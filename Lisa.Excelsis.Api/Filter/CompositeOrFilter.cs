using System;
using Lisa.Common.WebApi;

namespace Lisa.Excelsis.Api
{
    public class CompositeOrFilter : FilterProperties
    {
        public CompositeOrFilter(string[] key, string[] value)
        {
            Keys = key;
            Values = value;
        }

        public override bool Apply(DynamicModel field)
        {
            foreach (var item in Keys)
            {
                if (!field.Contains(item))
                {
                    break;
                }
                foreach (var filterValue in Values)
                {
                    if (string.Equals((string)field[item], filterValue, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public string[] Keys { get; set; }
        public string[] Values { get; set; }
    }
}
