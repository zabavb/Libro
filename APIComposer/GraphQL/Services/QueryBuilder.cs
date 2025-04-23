using System.Reflection;
using System.Web;

namespace APIComposer.GraphQL.Services
{
    public static class QueryBuilder
    {
        public static string BuildQuery(params object?[] querySources)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            foreach (var source in querySources)
            {
                if (source == null)
                    continue;

                foreach (PropertyInfo prop in source.GetType()
                             .GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var value = prop.GetValue(source);

                    // Skip nulls or special enum values like Bool.NULL
                    if (value == null ||
                        (value is Enum enumValue && enumValue.ToString() == "NULL"))
                        continue;

                    query[prop.Name] = value.ToString();
                }
            }

            return query.ToString()!;
        }
    }
}