using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PongServer.Domain.Enums;

namespace PongServer.Domain.Utils
{
    public class QueryFilters
    {
        public string? HostName { get; set; }
        public SortingOrder? Ordering { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}
