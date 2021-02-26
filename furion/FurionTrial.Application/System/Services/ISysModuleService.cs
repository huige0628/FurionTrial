using FurionTrial.Core.Entity;
using SqlSugar;
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

        /// <summary>
        /// 获取模块树
        /// </summary>
        /// <returns></returns>
        List<ModuleTreeNodeDto> GetModuleTree();

        /// <summary>
        /// 获取模块按钮列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        SqlSugarPagedList<SysModuleElement> GetModuleElementList(ModuleElementListRequestDto dto);
    }
}
