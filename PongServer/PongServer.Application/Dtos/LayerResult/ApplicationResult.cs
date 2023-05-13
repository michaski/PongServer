using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongServer.Application.Dtos.LayerResult
{
    public class ApplicationResult<T>
    {
        public int Status { get; set; }
        public T? Result { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
