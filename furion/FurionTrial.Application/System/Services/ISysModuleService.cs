using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    public interface ISysModuleService
    {
        /// <summary>
        /// 获取模块权限树
        /// </summary>
        /// <returns></returns>
        List<RoleModuleTreeNodeDto> GetRoleModuleTree(long roleId);
    }
}
