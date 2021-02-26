using Furion.DependencyInjection;
using Furion.FriendlyException;
using FurionTrial.Core;
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
        private readonly ISysRelevanceService _sysRelevanceService;
        private readonly IUserManager _userManager;
        public SysUserSerivce(ISqlSugarRepository<SysUser> sqlSugarRepository, 
            ISysOrgService sysOrgService,
             ISysRelevanceService sysRelevanceService,
            IUserManager userManager)
        {
            _repository = sqlSugarRepository;
            dbClint = _repository.Context;
            _sysOrgService = sysOrgService;
            _sysRelevanceService = sysRelevanceService;
            _userManager = userManager;
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
            query.WhereIF(!string.IsNullOrEmpty(dto.UserName), u =>u.UserName.Contains(dto.UserName.Trim()));
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
                UserCode=u.UserCode,
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
            }).ToPageList(dto.Page, dto.Limit, ref count);
            //var result = list.Adapt<List<UserListResponseDto>>();

            if (list.Count > 0)
            {
                var userIds = list.Select(u => u.UserId).ToList();
                var userOrgs = _sysOrgService.GetUserOrg(userIds);
                var roleList = dbClint.Queryable<SysRelevance>().Where(o => o.Key == SysRelevanceEnum.SysUser_SysRole.ToString() && userIds.Contains(o.FirstId) && o.IsDeleted == false).Select(o => new
                {
                    UserId = o.FirstId,
                    RoleId = o.SecondId
                }).Distinct().ToArray();
                list.ForEach(l =>
                {
                    var userOrg = userOrgs.Where(o => o.UserId == l.UserId).FirstOrDefault();
                    if (userOrg != null)
                    {
                        l.OrgId = userOrg.OrgId;
                        l.OrgName = userOrg.OrgName;
                    }
                    l.RoleId = roleList.Where(r => r.UserId == l.UserId).Select(r => r.RoleId).ToList();
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

        /// <summary>
        /// 添加和编辑用户信息
        /// </summary>
        /// <param name="dto"></param>
        public void AddEdit(UserAddEditDto dto)
        {
            //去前后空格
            dto.UserName = dto.UserName.Trim();
            dto.Email = dto.Email.Trim();
            long userId = 0;
            SysUser sysUser = null;
            if (dto.UserId.HasValue)
            {
                if (dbClint.Queryable<SysUser>().Where(u => u.UserCode == dto.UserCode && u.IsDeleted == false && u.UserId != dto.UserId.Value).Any())
                    throw Oops.Oh(string.Format("用户名[{0}]已经存在!", dto.UserName));
                if (dbClint.Queryable<SysUser>().Where(u => u.Email == dto.Email && u.IsDeleted == false && u.UserId != dto.UserId.Value).Any())
                    throw Oops.Oh(string.Format("邮箱[{0}]已经被使用!", dto.Email));
                sysUser = _repository.FirstOrDefault(u=>u.UserId== dto.UserId.Value);
                sysUser.ModifyDate = DateTime.Now;
                sysUser.ModifyUserId = _userManager.UserId;
                sysUser.ModifyUserName = _userManager.UserName;
                userId = sysUser.UserId;
            }
            else
            {
                if (dbClint.Queryable<SysUser>().Where(u => u.UserName == dto.UserName && u.IsDeleted == false).Any())
                    throw Oops.Oh(string.Format("用户名[{0}]已经存在!", dto.UserName));
                if (dbClint.Queryable<SysUser>().Where(u => u.Email == dto.Email && u.IsDeleted == false).Any())
                    throw Oops.Oh(string.Format("邮箱[{0}]已经被使用!", dto.Email));
                sysUser = new SysUser()
                {
                    CreateDate = DateTime.Now,
                    CreateUserId = _userManager.UserId,
                    CreateUserName = _userManager.UserName,
                    //Password = EncryptHelper.DESEnCode(dto.Password.Trim(), _auth.PasswordKey),
                    Type = (int)UserTypeEnum.Normal,
                    IsDeleted = false
                };
            }
            sysUser.UserCode = dto.UserCode;
            sysUser.UserName = dto.UserName;
            sysUser.Email = dto.Email;
            sysUser.Phone = dto.Phone;
            sysUser.Status = dto.Status;
            sysUser.Sex = dto.Sex;
            sysUser.Qq = dto.Qq;
            sysUser.Remark = dto.Remark;

            var result = dbClint.Ado.UseTran(() =>
            {
                if (dto.UserId.HasValue)
                    dbClint.Updateable(sysUser).ExecuteCommand();
                else
                    userId = dbClint.Insertable(sysUser).ExecuteReturnBigIdentity();
                //用户部门
                if (dto.OrgId.HasValue && dto.OrgId > 0)
                    _sysRelevanceService.SetUserOrg(userId, dto.OrgId.Value);
            });
            if (result.IsSuccess == false)
                throw Oops.Oh(result.ErrorMessage);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool Delete(long[] userId)
        {
            var count=  dbClint.Updateable<SysUser>().SetColumns(u => new SysUser
            {
                DeleteUserId = _userManager.UserId,
                DeleteUserName = _userManager.UserName,
                DeleteDate = DateTime.Now,
                IsDeleted = true,
                Status = 0
            }).Where(u => userId.Contains(u.UserId)).ExecuteCommand();
            return count > 0;
        }
    }
}
