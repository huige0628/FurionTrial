using FurionTrial.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    public interface ISysRoleService
    {
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        List<SysRole> GetAllRoles();
    }
}
