using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using SuperSchnell.CompanyDataProvider.Contracts;

namespace SuperSchnell.CompanyDataProvider.Helpers
{
    public static class ExtensionMethods
    {
        public static PagedResultSet<T> ToPagedSet<T>(this IQueryOver<T> query, ListOptions options)
        {
            var countQuery = query.ToRowCountQuery().FutureValue<int>();
            var resultQuery = query.Skip(options.Page*options.PageSize)
                .Take(options.PageSize)
                .Future();
            return new PagedResultSet<T>(countQuery.Value, resultQuery.ToList());
        }

        public static T FirstOrDefault<T>(this IQueryOver<T> query, ListOptions options)
        {
            return query.Take(1).SingleOrDefault();
        }
        public static IEnumerable<string> TrimFreeTextQueryString(this string queryString)
        {
            if (string.IsNullOrWhiteSpace(queryString))
                return new[]{string.Empty};
            return queryString.Trim()
                              .Replace("+", "")
                              .Replace("-", "")
                              .Replace("*", "")
                              .Replace("?", "")
                              .Replace("!", "")
                              .Replace("(", "")
                              .Replace(")", "")
                              .Replace("'", "")
                              .Replace("\"", "")
                              .Replace("^", "")
                              .Replace(".", "")
                              .Replace(",", "")
                              .Replace(";", "")
                              .Replace(":", "")
                              .Replace("[", "")
                              .Replace("]", "")
                              .Replace("~", "")
                              .Replace("{", "")
                              .Replace("}", "")
                              .Replace("|", "")
                              .Split(new []{" "},StringSplitOptions.RemoveEmptyEntries);
        }
    }
}