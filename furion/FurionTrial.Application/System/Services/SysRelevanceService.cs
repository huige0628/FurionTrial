using Furion.DependencyInjection;
using Furion.FriendlyException;
using FurionTrial.Core;
using FurionTrial.Core.Entity;
using FurionTrial.Core.Enums;
using Mapster;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    public class SysRelevanceService:ISysRelevanceService, ITransient
    {
        private readonly ISqlSugarRepository<SysRelevance> _repository;
        private readonly ISqlSugarClient dbClint; // 核心对象：拥有完整的SqlSugar全部功能
        /// <summary>
        /// 用户管理类
        /// </summary>
        private readonly IUserManager _userManager;

        public SysRelevanceService(ISqlSugarRepository<SysRelevance> sqlSugarRepository, IUserManager userManager)
        {
            _repository = sqlSugarRepository;
            dbClint = _repository.Context;
            _userManager = userManager;
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SysRole> GetUserRoles(long userId)
        {
            return dbClint.Queryable<SysRole>().Where(r => r.IsDeleted == false && SqlFunc.Subqueryable<SysRelevance>().Where(s => s.FirstId == userId && r.RoleId == s.SecondId && s.Key == SysRelevanceEnum.SysUser_SysRole.ToString()).Any()).ToList();
        }

        /// <summary>
        /// 获取用户模块
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserModuleDto> GetUserModules(long userId)
        {
            var list = dbClint.Queryable<SysRelevance, SysRelevance, SysModule>((r1, r2, m) => new object[]
            {
                JoinType.Inner,r1.SecondId==r2.FirstId && r2.Key==SysRelevanceEnum.SysRole_SysModule.ToString() && r2.IsDeleted==false,
                JoinType.Inner,r2.SecondId==m.ModuleId && m.IsDeleted==false
            })
                .Where((r1, r2, m) => r1.FirstId == userId && r1.IsDeleted == false && r1.Key == SysRelevanceEnum.SysUser_SysRole.ToString())
                 .Select((r1, r2, m) => m).Distinct().ToList();
            var result = list.Adapt<List<UserModuleDto>>();
            return result;
        }

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="dto"></param>
        public void SetUserRole(UserSetRoleDto dto)
        {
            var roleKey = SysRelevanceEnum.SysUser_SysRole.ToString();
            var roleList = dbClint.Queryable<SysRelevance>().Where(o => o.Key == roleKey && o.FirstId == dto.UserId && o.IsDeleted == false).Select(o => new
            {
                o.RelevanceId,
                UserId = o.FirstId,
                RoleId = o.SecondId
            }).Distinct().ToArray();

            List<SysRelevance> addList = new List<SysRelevance>();
            List<long> delList = new List<long>();
            var addRoles = dto.RoleId.Except(roleList.Select(r => r.RoleId).ToList()).ToList();
            if (addRoles.Count > 0)
            {
                addRoles.ForEach(r =>
                {
                    addList.Add(new SysRelevance()
                    {
                        Key = roleKey,
                        FirstId = dto.UserId,
                        SecondId = r,
                        CreateDate = DateTime.Now,
                        CreateUserId = _userManager.UserId,
                        CreateUserName = _userManager.UserName,
                        IsDeleted = false
                    });
                });
            }
            var delRoles = roleList.Select(r => r.RoleId).ToList().Except(dto.RoleId).ToList();
            if (delRoles.Count > 0)
            {
                delList.AddRange(roleList.Where(r => delRoles.Contains(r.RoleId)).Select(r => r.RelevanceId).ToList());
            }
            var result= dbClint.Ado.UseTran(() =>
            {
                if (addList.Count > 0)
                    dbClint.Insertable(addList).ExecuteCommand();
                if (delList.Count > 0)
                {
                    dbClint.Updateable<SysRelevance>().SetColumns(r => new SysRelevance()
                    {
                        DeleteDate = DateTime.Now,
                        DeleteUserId = _userManager.UserId,
                        DeleteUserName = _userManager.UserName,
                        IsDeleted = true
                    }).Where(r => delList.Contains(r.RelevanceId)).ExecuteCommand();
                }
            });
            if(!result.IsSuccess)
                throw Oops.Oh(result.ErrorMessage);
        }

        /// <summary>
        /// 设置用户部门
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orgId"></param>
        public void SetUserOrg(long userId, long orgId)
        {
            var orgs = dbClint.Queryable<SysRelevance>().Where(r => r.FirstId == userId && r.IsDeleted == false && r.Key == SysRelevanceEnum.SysUser_SysOrg.ToString()).ToList();
            if (orgs.Where(o => o.SecondId != orgId).Any())
            {
                var orgIds = orgs.Where(o => o.SecondId != orgId).Select(r => r.RelevanceId).ToList();
                dbClint.Updateable<SysRelevance>().SetColumns(u => new SysRelevance
                {
                    DeleteUserId = _userManager.UserId,
                    DeleteUserName = _userManager.UserName,
                    DeleteDate = DateTime.Now,
                    IsDeleted = true
                }).Where(u => orgIds.Contains(u.RelevanceId)).ExecuteCommand();
            }
            if (orgs.Where(o => o.SecondId == orgId).Any())
                return;

            var relevance = new SysRelevance()
            {
                FirstId = userId,
                SecondId = orgId,
                Key = SysRelevanceEnum.SysUser_SysOrg.ToString(),
                CreateUserId = _userManager.UserId,
                CreateUserName = _userManager.UserName,
                CreateDate = DateTime.Now,
                IsDeleted = false
            };
            dbClint.Insertable(relevance).ExecuteCommand();
        }

        /// <summary>
        /// 获取角色模块权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<long> GetRoleModule(long roleId)
        {
            return dbClint.Queryable<SysRelevance>().Where(r => r.Key == SysRelevanceEnum.SysRole_SysModule.ToString() && r.IsDeleted == false && r.FirstId == roleId).Select(r => r.SecondId).ToList();
        }

    }
}
