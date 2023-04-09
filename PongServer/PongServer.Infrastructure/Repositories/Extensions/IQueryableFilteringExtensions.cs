using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PongServer.Domain.Enums;
using PongServer.Domain.Utils;

namespace PongServer.Infrastructure.Repositories.Extensions
{
    public static class IQueryableFilteringExtensions
    {
        public static IQueryable<T> SearchInField<T>(this IQueryable<T> query, Func<T, string> fieldSelector,
            string searchPhrase)
        {
            if (string.IsNullOrEmpty(searchPhrase))
            {
                return query;
            }

            return query.Where(dbSet => string.IsNullOrEmpty(searchPhrase) ||
                                        fieldSelector(dbSet).ToLower().Contains(searchPhrase.ToLower()));
        }

        public static IQueryable<T> OrderBy<T, T2>(this IQueryable<T> query, Func<T, T2> fieldSelector,
            SortingOrder? sortingOrder)
        {
            if (!sortingOrder.HasValue)
            {
                return query;
            }

            var order = sortingOrder.Value;
            return order switch
            {
                SortingOrder.Ascending => query.OrderBy(fieldSelector).AsQueryable(),
                SortingOrder.Descending => query.OrderByDescending(fieldSelector).AsQueryable(),
                _ => throw new ArgumentOutOfRangeException($"Unsupported ordering (received {sortingOrder}).")
            };
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, QueryFilters queryFilters)
        {
            if (queryFilters.Page.HasValue && queryFilters.PageSize.HasValue)
            {
                return query
                    .Skip(queryFilters.PageSize.Value * (queryFilters.Page.Value - 1))
                    .Take(queryFilters.PageSize.Value);
            }

            return query;
        }
    }
}
