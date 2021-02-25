using FurionTrial.Core.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    public interface ISysUserSerivce
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        SysUser Login(string userName, string password);

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        SqlSugarPagedList<UserListResponseDto> GetUserList(UserListRequestDto dto);

        /// <summary>
        /// 添加和编辑用户信息
        /// </summary>
        /// <param name="dto"></param>
        void AddEdit(UserAddEditDto dto);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool Delete(long[] userId);
    }
}
