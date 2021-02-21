using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Core.Enums
{
    public enum SysRelevanceEnum : int
    {
        /// <summary>
        /// 角色部门Key
        /// </summary>
        [Description("角色部门Key")]
        SysRole_SysOrg = 1,
        /// <summary>
        /// 角色模块key
        /// </summary>
        [Description("角色模块key")]
        SysRole_SysModule = 2,

        /// <summary>
        ///角色菜单key
        /// </summary>
        [Description("角色菜单key")]
        SysRole_SysModule_Element = 3,

        /// <summary>
        /// 用户部门Key
        /// </summary>
        [Description("用户部门Key")]
        SysUser_SysOrg = 4,

        /// <summary>
        /// 用户角色Key
        /// </summary>
        [Description("用户角色Key")]
        SysUser_SysRole = 5,
    }
}
