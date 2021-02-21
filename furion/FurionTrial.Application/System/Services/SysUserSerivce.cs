using Furion.DependencyInjection;
using FurionTrial.Core.Entity;
using Microsoft.AspNetCore.Authorization;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    public class SysUserSerivce : ISysUserSerivce, ITransient
    {
        private readonly ISqlSugarRepository<SysUser> _repository;
        private readonly ISqlSugarClient dbClint; // 核心对象：拥有完整的SqlSugar全部功能
        public SysUserSerivce(ISqlSugarRepository<SysUser> sqlSugarRepository)
        {
            _repository = sqlSugarRepository;
            dbClint = _repository.Context;
        }

        /// <summary>
        /// 根据用户ID获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public SysUser GetUserById(long userId)
        {
            return dbClint.Queryable<SysUser>().Where(u => u.UserId == userId).First();
        }

        /// <summary>
        /// 根据用户名称获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public SysUser GetUserByName(string userName)
        {
            return dbClint.Queryable<SysUser>().Where(u => u.UserName == userName).First();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public SysUser Login(string userName,string password)
        {
            return dbClint.Queryable<SysUser>().Where(u => u.UserCode == userName && u.Password==password).First();
        }

    }
}
