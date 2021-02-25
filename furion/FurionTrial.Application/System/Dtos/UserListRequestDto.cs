using FurionTrial.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    public class UserListRequestDto: ApiPageRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public long? OrgId { get; set; }
    }
}
