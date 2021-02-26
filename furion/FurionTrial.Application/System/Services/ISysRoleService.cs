using FurionTrial.Core.Entity;
using SqlSugar;
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

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        SqlSugarPagedList<SysRole> GetRoleList(RoleListRequestDto dto);

        /// <summary>
        /// 添加编辑角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        void AddEdit(RoleAddEditDto dto);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ids"></param>
        bool Delete(long[] ids);
    }
}
