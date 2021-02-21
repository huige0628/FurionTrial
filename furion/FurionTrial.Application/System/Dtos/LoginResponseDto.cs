using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    public class LoginResponseDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// token
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Token过期时间
        /// </summary>
        public DateTime ExipreTime { get; set; }
    }
}
