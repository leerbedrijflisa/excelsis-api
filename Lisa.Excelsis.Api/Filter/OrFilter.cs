using Lisa.Common.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lisa.Excelsis.Api
{
    public class OrFilter : FilterProperties
    {
        public OrFilter(string key, string[] value)
        {
            Key = key;
            Value = value;
        }

        public override bool Apply(DynamicModel field)
        {
            if (Key.Contains("."))
            {
                var k = Key.Split('.');
                dynamic subfield = field;
                foreach (string item in k)
                {
                    bool m = false;
                    foreach (dynamic e in subfield)
                    {
                        //if (e.Key == item)
                        //{
                        //    m = true;
                        //}
                    }
                    if (m == false)
                    {
                        return false;
                    }
                    subfield = (dynamic) subfield[item];
                }
            }
            if (!field.Contains(Key))
            {
                return false;
            }
            foreach (var filterValue in Value)
            {
                if (field[Key] is IEnumerable<string>)
                {
                    var fieldProperties = (IEnumerable<string>) field[Key];
                    if (fieldProperties.Contains(filterValue))
                    {
                        return true;
                    }
                }
                else if (string.Equals((string) field[Key], filterValue, StringComparison.Ordinal))
                {
                    return true;
                }
            }
            return false;
        }
        public string Key { get; set; }
        public string[] Value { get; set; }
    }
}