using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Core.Enums
{
    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserTypeEnum : int
    {
        /// <summary>
        /// 普通用户
        /// </summary>
        [Description("普通用户")]
        Normal = 0,
        /// <summary>
        /// 系统管理员
        /// </summary>
        [Description("系统管理员")]
        Admin = 1,
    }
}
