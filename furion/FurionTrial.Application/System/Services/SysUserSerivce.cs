using Furion.DependencyInjection;
using FurionTrial.Core.Entity;
using FurionTrial.Core.Enums;
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
        private readonly ISysOrgService _sysOrgService;
        public SysUserSerivce(ISqlSugarRepository<SysUser> sqlSugarRepository, ISysOrgService sysOrgService)
        {
            _repository = sqlSugarRepository;
            dbClint = _repository.Context;
            _sysOrgService = sysOrgService;
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
            if (dto.OrgId > 0)
            {
                //获取部门下面的所有子部门
                var reKey = SysRelevanceEnum.SysUser_SysOrg.ToString();
                var orgId = _sysOrgService.GetAllChild(new List<long> { dto.OrgId.Value }).Select(o => o.OrgId).ToArray();
                var rUserId = dbClint.Queryable<SysRelevance>().Where(o => o.Key == reKey && orgId.Contains(o.SecondId) && o.IsDeleted == false).Select(o => o.FirstId).Distinct().ToArray();
                query.Where(o => rUserId.Contains(o.UserId));
            }
            int count = 0;
            var list = query.Select(u=> new UserListResponseDto() 
            {
                UserId = u.UserId,
                UserName = u.UserName,
                Avatar = u.Avatar,
                Phone = u.Phone,
                Email = u.Email,
                Qq = u.Qq,
                Sex = u.Sex,
                Status = u.Status,
                LastLoginIp = u.LastLoginIp,
                LastLoginDate = u.LastLoginDate,
                CreateDate = u.CreateDate,
                Remark = u.Remark,
                //RoleId = roleList.Where(e => e.UserId == u.UserId).Any() ? roleList.Where(m => m.UserId == u.UserId).Select(m => m.RoleId).ToList() : new List<long>(),
            }).ToPageList(dto.Page, dto.Limit, ref count);
            //var result = list.Adapt<List<UserListResponseDto>>();

            if (list.Count > 0)
            {
                var userIds = list.Select(u => u.UserId).ToList();
                var userOrgs = _sysOrgService.GetUserOrg(userIds);
                list.ForEach(l =>
                {
                    var userOrg = userOrgs.Where(o => o.UserId == l.UserId).FirstOrDefault();
                    if (userOrg != null)
                    {
                        l.OrgId = userOrg.OrgId;
                        l.OrgName = userOrg.OrgName;
                    }
                });
            }
            return new SqlSugarPagedList<UserListResponseDto>()
            {
                PageIndex = dto.Page,
                PageSize = dto.Limit,
                Items = list,
                TotalCount = count
            };
        }

    }
}
