using System;

namespace Lisa.Excelsis.Api
{
    public class CompositeOrFilter : FilterProperties
    {
        public CompositeOrFilter(string[] key, string[] value)
        {
            Keys = key;
            Values = value;
        }

        public override bool Apply(dynamic field)
        {
            foreach (var key in Keys)
            {
                if (key.Contains("."))
                {
                    var splittedKeys = key.Split('.');
                    dynamic subfield = field;
                    foreach (string singleKey in splittedKeys)
                    {
                        if (!subfield.Contains(singleKey))
                        {
                            return false;
                        }
                        subfield = (dynamic)subfield[singleKey];
                    }
                }
                else if (!field.Contains(key))
                {
                    break;
                }
                foreach (var filterValue in Values)
                {
                    if (string.Equals((string)field[key], filterValue, StringComparison.OrdinalIgnoreCase))
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
