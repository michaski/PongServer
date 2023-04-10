using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PongServer.Domain.Enums;
using PongServer.Domain.Utils;
using PongServer.Infrastructure.Extensions;

namespace PongServer.Infrastructure.Extensions
{
    public static class IQueryableFilteringExtensions
    {
        public static IQueryable<T> SearchInField<T>(this IQueryable<T> query, Expression<Func<T, string>> fieldSelector,
            string searchPhrase)
        {
            if (string.IsNullOrEmpty(searchPhrase))
            {
                return query;
            }

            var entity = Expression.Parameter(typeof(T), "type");
            var propertyName = (fieldSelector.Body as MemberExpression).Member.Name;
            var property = Expression.Property(entity, propertyName);

            var containsMethod = typeof(string).GetMethod("Contains", new []{ typeof(string) });
            var toLowerMethod = typeof(string).GetMethod("ToLower", new Type[]{});

            var searchValue = Expression.Constant(searchPhrase.ToLower(), typeof(string));

            var loweredProperty = Expression.Call(
                property,
                toLowerMethod);
            var expressionBody = Expression.Call(
                loweredProperty,
                containsMethod,
                searchValue
            );
            var lambda = Expression.Lambda<Func<T, bool>>(expressionBody, entity);

            return query.Where(lambda);
        }

        public static IQueryable<T> OrderBy<T, T2>(this IQueryable<T> query, Expression<Func<T, T2>> fieldSelector,
            SortingOrder? sortingOrder)
        {
            if (!sortingOrder.HasValue)
            {
                return query;
            }

            var order = sortingOrder.Value;
            query = order switch
            {
                SortingOrder.Ascending => query.OrderBy(fieldSelector),
                SortingOrder.Descending => query.OrderByDescending(fieldSelector),
                _ => throw new ArgumentOutOfRangeException($"Unsupported ordering (received {sortingOrder}).")
            };
            return query;
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
