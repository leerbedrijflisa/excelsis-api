using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Lisa.Excelsis.Api
{
    internal class CompositeAndFilter : FilterProperties
    {
        public CompositeAndFilter(string[] key, string[] value)
        {
            Keys = key;
            Values = value;
        }
        public override bool Apply(dynamic field)
        {
            foreach (var key in Keys)
            {
                dynamic arraySubfield = string.Empty;
                if (key.Contains("."))
                {
                    var k = key.Split('.');
                    dynamic subfield = field;
                    foreach (string item in k)
                    {
                        if (subfield is IEnumerable<object>)
                        {
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
                else if (!field.Contains(key))
                {
                    break;
                }
            }
            var blub = Keys[0].Split('.');
            if (blub[0] == "assessors")
            {
                var meep = new List<string>();
                foreach (var item in Keys)
                {
                    meep.Add(item.Split('.')[1]);
                }
                if (Keys.Length != Values.Length)
                {
                    return false;
                }
                var derp = new List<KeyValuePair<string, String>>();
                for (int i = 0; i < Keys.Length; i++)
                {
                    derp.Add(new KeyValuePair<string, string>(meep[i], Values[i]));
                }
                foreach (var item in field[blub[0]])
                {
                    foreach (var okie in derp)
                    {
                        if (item[okie.Key].ToLower() == okie.Value.ToLower())
                        {
                            System.Diagnostics.Debug.WriteLine(okie.Value);
                        }
                        else
                        {
                            
                        }
                    }
                }
            }
            return false;
        }

        public string[] Keys { get; set; }
        public string[] Values { get; set; }
    }
}