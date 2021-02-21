using FurionTrial.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    public interface ISysRelevanceService
    {
        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<SysRole> GetUserRoles(long userId);

        /// <summary>
        /// 获取用户模块
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<UserModuleDto> GetUserModules(long userId);

    }
}
