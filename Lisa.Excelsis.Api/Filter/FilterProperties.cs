using Lisa.Common.WebApi;

namespace Lisa.Excelsis.Api
{
    public abstract class FilterProperties
    {
        public abstract bool Apply(DynamicModel field);
    }
}