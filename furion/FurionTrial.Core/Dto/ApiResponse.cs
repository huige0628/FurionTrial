using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Core.Dto
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }

        public object Msg { get; set; }

        public bool Success { get; set; }

        public T Data { get; set; }

        public int? Count { get; set; }

        public object Extras { get; set; }
        public long Timestamp { get; set; }
    }
}
