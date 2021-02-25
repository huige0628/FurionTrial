using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    /// <summary>
    /// 用户部门
    /// </summary>
    public class UserOrgDto
    {
        public long UserId { get; set; }

        public long OrgId { get; set; }

        public string OrgName { get; set; }
        public string OrgNamePath { get; set; }
    }
}
