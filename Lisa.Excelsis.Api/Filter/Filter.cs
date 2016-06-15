using Lisa.Common.WebApi;
using System.Collections.Generic;

namespace Lisa.Excelsis.Api
{
    public abstract class Filter
    {
        public abstract bool Applies(DynamicModel model);

        public IEnumerable<DynamicModel> Apply(IEnumerable<DynamicModel> models)
        {
            foreach (var model in models)
            {
                if (Applies(model))
                {
                    yield return model;
                }
            }
        }
    }
}