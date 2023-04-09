using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongServer.Domain.Utils
{
    public class ResultPage<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItemsCount { get; set; }

        public ResultPage(IEnumerable<T> items, int totalItemsCount)
        {
            Items = items;
            TotalItemsCount = totalItemsCount;
        }
    }
}
