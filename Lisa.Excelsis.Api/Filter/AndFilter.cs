using Lisa.Common.WebApi;
using System.Collections.Generic;

namespace Lisa.Excelsis.Api
{
    public class AndFilter : Filter
    {
        public AndFilter(params Filter[] filters)
        {
            Filters = new List<Filter>(filters);
        }

        public ICollection<Filter> Filters { get; private set; }

        public override bool Applies(DynamicModel model)
        {
            // The and-filter needs to treat filters on nested fields differently. ConvertFilters()
            // collects these filters and creates new child filters out of them, and then we apply
            // the and-filter to the result. We can't do this in the constructor, because filters
            // might be added to the AndFilter-object between its creation and the call to
            // Applies().
            var filters = ConvertFilters(Filters);

            foreach (var filter in filters)
            {
                if (!filter.Applies(model))
                {
                    return false;
                }
            }

            return true;
        }

        private IEnumerable<Filter> ConvertFilters(IEnumerable<Filter> filters)
        {
            // When applying the AndFilter to nested fields, they should behave a bit differently
            // than regular fields. Take the following query on a collection of movies, for
            // example.
            //
            //   releaseYear=2016 AND actors.firstName=amy AND actors.lastName=adams
            //
            // This means that you want all movies released in 2016 that star Amy Adams. You
            // shouldn't get movies that star Amy Poehler or Patrick Adams, but that's what the
            // regular AndFilter would give you.
            // ConvertFilters() groups fields that belong together (actors.firstName and
            // actors.lastName, in the above example) into a new SubFilter object and then adds
            // that filter as a child of the AndFilter. All other filters are kept unchanged.

            var subFilters = new Dictionary<string, SubFilter>();

            foreach (var filter in filters)
            {
                if (filter is EqualsFilter && ((EqualsFilter) filter).HasNestedKey)
                {
                    var equalsFilter = (EqualsFilter) filter;
                    if (!subFilters.ContainsKey(equalsFilter.Prefix))
                    {
                        subFilters.Add(equalsFilter.Prefix, new SubFilter(equalsFilter.Prefix));
                    }

                    subFilters[equalsFilter.Prefix].Filters.Add(new EqualsFilter(equalsFilter.SubKey, equalsFilter.Values));
                }
                else
                {
                    yield return filter;
                }
            }

            foreach (var subFilter in subFilters.Values)
            {
                yield return subFilter;
            }
        }
    }
}