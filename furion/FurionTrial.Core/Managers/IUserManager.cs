using FurionTrial.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Core
{
    public interface IUserManager
    {
        /// <summary>
        /// 获取用户 Id
        /// </summary>
        int UserId { get; }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        SysUser User { get; }

    }
}
