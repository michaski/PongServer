using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PongServer.Domain.Utils;

namespace PongServer.Application.Dtos.V1.Pagination
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalItemsCount { get; set; }

        public PagedResult(ResultPage<T> results, QueryFilters filters)
        {
            filters.Page ??= 1;
            filters.PageSize ??= 1;
            Items = results.Items;
            TotalItemsCount = results.TotalItemsCount;
            if (results.TotalItemsCount > 0)
            {
                ItemsFrom = filters.PageSize.Value * (filters.Page.Value - 1) + 1;
                ItemsTo = ItemsFrom + filters.PageSize.Value - 1;
                TotalPages = (int)Math.Ceiling(results.TotalItemsCount / (double)filters.PageSize);
            }
            else
            {
                ItemsFrom = 0;
                ItemsTo = 0;
                TotalPages = 0;
            }
        }
    }
}
