using Lisa.Common.WebApi;
using System.Collections.Generic;

namespace Lisa.Excelsis.Api
{
    public class OrFilter : Filter
    {
        public OrFilter(params Filter[] filters)
        {
            Filters = new List<Filter>(filters);
        }

        public ICollection<Filter> Filters { get; private set; }

        public override bool Applies(DynamicModel model)
        {
            foreach (var filter in Filters)
            {
                if (filter.Applies(model))
                {
                    return true;
                }
            }

            return false;
        }
    }
}