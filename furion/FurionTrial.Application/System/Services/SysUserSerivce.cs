using Furion.DependencyInjection;
using FurionTrial.Core.Entity;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public SqlSugarPagedList<UserListResponseDto> GetUserList(UserListRequestDto dto)
        {
            var query = dbClint.Queryable<SysUser>().Where(u => u.IsDeleted == false);
            query.WhereIF(!string.IsNullOrEmpty(dto.UserName), u => u.UserName == dto.UserName);
            int count = query.Count();
            var list = query.Select(u=>new UserListResponseDto() 
            { 
                UserId=u.UserId,
                UserCode=u.UserCode,
                UserName=u.UserName
            }).ToPagedList(dto.Page, dto.Limit);
            //var result = list.Adapt<List<UserListResponseDto>>();
            return list;
            //return new PagedList()
            //{
            //    PageIndex = dto.Page,
            //    PageSize = dto.Limit,
            //    Items = result,
            //    TotalCount = count
            //};
        }

    }
}
