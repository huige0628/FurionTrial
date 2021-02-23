using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Core.Dto
{
    /// <summary>
    /// 请求接口参数
    /// </summary>
    public class ApiPageRequest
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 条数
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public string Order { get; set; }
    }
}
