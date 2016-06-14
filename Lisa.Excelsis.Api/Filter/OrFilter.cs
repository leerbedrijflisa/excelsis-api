using System;
using System.Collections.Generic;

namespace Lisa.Excelsis.Api
{
    public class OrFilter : FilterProperties
    {
        public OrFilter(string key, string[] value)
        {
            Key = key;
            Value = value;
        }

        public override bool Apply(dynamic field)
        {
            dynamic arraySubfield = string.Empty;
            if (Key.Contains("."))
            {
                var k = Key.Split('.');
                dynamic subfield = field;
                foreach (string item in k)
                {
                    if (subfield is IEnumerable<object>)
                    {
                        field = subfield;
                        arraySubfield = item;
                        foreach (var subfieldItem in subfield)
                        {
                            if (!subfieldItem.Contains(item))
                            {
                                return false;
                            }
                        }
                    }
                    else if (!subfield.Contains(item))
                    {
                        return false;
                    }
                    else
                    {
                        subfield = (dynamic)subfield[item];
                    }
                }
            }
            else if (!field.Contains(Key))
            {
                return false;
            }
            foreach (var filterValue in Value)
            {
                if (field is IEnumerable<object>)
                {
                    var fieldProperties = new List<string>();
                    foreach (var fieldPropertyToLower in field)
                    {
                        string meep = (string)fieldPropertyToLower[arraySubfield];
                        fieldProperties.Add(meep.ToLower());
                    }
                    if (fieldProperties.Contains(filterValue.ToLower()))
                    {
                        return true;
                    }
                }
                else if (field is IEnumerable<string>)
                {
                    var fieldProperties = new List<string>();

                    foreach (var fieldPropertyToLower in (IEnumerable<string>)field)
                    {
                        string meep = (string)fieldPropertyToLower[arraySubfield];
                        fieldProperties.Add(meep.ToLower());
                    }
                    if (fieldProperties.Contains(filterValue.ToLower()))
                    {
                        return true;
                    }
                }
                else if (string.Equals((string) field[Key], filterValue, StringComparison.OrdinalIgnoreCase))
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