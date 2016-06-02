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
                foreach (var item in field[blub[0]])
                {
                    if (Keys.Length != Values.Length)
                    {
                        return false;
                    }
                    for (int i = 0; i < Keys.Length; i++)
                    {
                        if (field[blub[0]][blub[1]] == Values[i])
                        {

                            System.Diagnostics.Debug.WriteLine("edwjgberuigeiuuvnefjvkenbujnejnetjjetjnhuenheiunhujnjasgek");
                            return true;
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