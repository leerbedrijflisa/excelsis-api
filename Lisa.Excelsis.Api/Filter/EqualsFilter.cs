using Lisa.Common.WebApi;

namespace Lisa.Excelsis.Api
{
    public class EqualsFilter : Filter
    {
        public EqualsFilter(string key, params object[] values)
        {
            Key = key;
            Values = values;
        }

        public string Key { get; set; }
        public object[] Values { get; set; }

        public override bool Applies(DynamicModel model)
        {
            foreach (var value in Values)
            {
                if (model[Key].Equals(value))
                {
                    return true;
                }
            }

            return false;
        }

        // The internal properties are used by AndFilter to find out whether an EqualsFilter should
        // be added to a SubFilter.

        internal bool HasNestedKey
        {
            get { return Key.Contains("."); }
        }

        internal string Prefix
        {
            get { return Key.Substring(0, Key.IndexOf('.')); }
        }

        internal string SubKey
        {
            get { return Key.Substring(Key.IndexOf('.') + 1); }
        }
    }
}