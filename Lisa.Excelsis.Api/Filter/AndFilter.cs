using System;
using System.Collections.Generic;

namespace Lisa.Excelsis.Api
{
    public class AndFilter : FilterProperties
    {
        public AndFilter(string key, string[] value)
        {
            Key = key;
            Value = value;
        }

        public override bool Apply(dynamic field)
        {
            if (Key.Contains("."))
            {
                var splittedKey = Key.Split('.');
                dynamic subfield = field;
                foreach (string item in splittedKey)
                {
                    if (!subfield.Contains(item))
                    {
                        return false;
                    }
                    subfield = (dynamic)subfield[item];
                }
            }
            else if(!field.Contains(Key))
            {
                return false;
            }
            foreach (var filterValue in Value)
            {
                if (field[Key] is IEnumerable<string>)
                {
                    var fieldProperties = new List<string>();
                    foreach (var fieldPropertyToLower in (IEnumerable<string>)field[Key])
                    {
                        fieldProperties.Add(fieldPropertyToLower.ToLower());
                    }
                    if (fieldProperties.Contains(filterValue.ToLower()))
                    {
                        return true;
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