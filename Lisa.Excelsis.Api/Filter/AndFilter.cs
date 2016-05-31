using Lisa.Common.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lisa.Excelsis.Api
{
    public class AndFilter : FilterProperties
    {
        public AndFilter(string key, string[] value)
        {
            Key = key;
            Value = value;
        }

        public override bool Apply(DynamicModel field)
        {
            if (!field.Contains(Key))
            {
                return false;
            }
            foreach (var filterValue in Value)
            {
                if (field[Key] is IEnumerable<string>)
                {
                    var fieldProperties = (IEnumerable<string>) field[Key];
                    if (!fieldProperties.Contains(filterValue))
                    {
                        return false;
                    }
                }
                else if (!string.Equals((string) field[Key], filterValue, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }
        public string Key { get; set; }
        public string[] Value { get; set; }
    }
}