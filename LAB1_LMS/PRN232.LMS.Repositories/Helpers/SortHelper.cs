using System.Linq.Expressions;

namespace PRN232.LMS.Repositories.Helpers;

public static class SortHelper
{
    /// <summary>
    /// Applies sorting from a comma-separated sort string.
    /// e.g. "fullName,-dateOfBirth" => order by fullName ASC, dateOfBirth DESC
    /// Property name matching is case-insensitive.
    /// </summary>
    public static IQueryable<T> ApplySort<T>(IQueryable<T> source, string? sortString)
    {
        if (string.IsNullOrWhiteSpace(sortString))
            return source;

        var fields = sortString.Split(',', StringSplitOptions.RemoveEmptyEntries);
        IOrderedQueryable<T>? ordered = null;

        foreach (var rawField in fields)
        {
            var descending = rawField.TrimStart().StartsWith('-');
            var fieldName  = rawField.TrimStart('-', '+', ' ').Trim();

            var prop = typeof(T).GetProperties()
                .FirstOrDefault(p => p.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));

            if (prop is null) continue;

            var param = Expression.Parameter(typeof(T), "x");
            var body  = Expression.PropertyOrField(param, prop.Name);
            var key   = Expression.Lambda(body, param);

            var methodName = ordered is null
                ? (descending ? "OrderByDescending" : "OrderBy")
                : (descending ? "ThenByDescending"  : "ThenBy");

            var result = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), prop.PropertyType)
                .Invoke(null, new object[] { ordered ?? source, key });

            ordered = (IOrderedQueryable<T>)result!;
        }

        return ordered ?? source;
    }
}
