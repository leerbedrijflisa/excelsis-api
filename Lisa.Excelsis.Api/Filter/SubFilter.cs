using Lisa.Common.WebApi;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lisa.Excelsis.Api
{
    // Applies an AND filter to a sub-field containing an object or a list of objects. Consider
    // the following query on a list of movies.
    // 
    //   releaseYear=2016 AND actors.firstName=amy AND actors.lastName=adams
    //
    // SubFilter can correctly handle the parts that query the sub-collection of actors. You don't
    // have to create SubFilter objects yourself; AndFilter takes care of that for you.
    internal class SubFilter : Filter
    {
        public SubFilter(string key, params Filter[] filters)
        {
            _key = key;
            _filters = new List<Filter>(filters);
        }

        public ICollection<Filter> Filters
        {
            get { return _filters; }
        }

        public override bool Applies(DynamicModel model)
        {
            // SubFilter works by applying an AndFilter to the collection in the sub-field.
            var filter = new AndFilter(_filters.ToArray());

            // If the sub-field isn't a list already, turn it into a list.
            // NOTE: for now, we make a copy of the list of models, because we can't cast
            // object[] to IEnumerable<DynamicModel>.
            List<DynamicModel> models = new List<DynamicModel>();

            if (model[_key] is IEnumerable)
            {
                foreach (DynamicModel m in (IEnumerable) model[_key])
                {
                    models.Add(m);
                }
            }
            else
            {
                models.Add((DynamicModel) model[_key]);
            }

            var results = filter.Apply(models);
            return results.Any();
        }

        private string _key;
        private List<Filter> _filters;
    }
}