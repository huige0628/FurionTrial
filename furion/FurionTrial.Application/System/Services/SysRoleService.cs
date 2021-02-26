using Furion.DependencyInjection;
using Furion.FriendlyException;
using FurionTrial.Core;
using FurionTrial.Core.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    public class SysRoleService:ISysRoleService, ITransient
    {
        private readonly ISqlSugarRepository<SysRole> _repository;
        private readonly ISqlSugarClient dbClint; // 核心对象：拥有完整的SqlSugar全部功能
        private readonly IUserManager _userManager;
        public SysRoleService(ISqlSugarRepository<SysRole> sqlSugarRepository,
            IUserManager userManager)
        {
            _repository = sqlSugarRepository;
            dbClint = _repository.Context;
            _userManager = userManager;
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public List<SysRole> GetAllRoles()
        {
            return dbClint.Queryable<SysRole>().Where(r => r.IsDeleted == false).ToList();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public SqlSugarPagedList<SysRole> GetRoleList(RoleListRequestDto dto)
        {
            var query = dbClint.Queryable<SysRole>().Where(o => o.IsDeleted == false);
            query.WhereIF(!string.IsNullOrWhiteSpace(dto.RoleName), o => o.RoleName.Contains(dto.RoleName.Trim()));
            int total = 0;
            var list = query.ToPageList(dto.Page, dto.Limit, ref total);
            return new SqlSugarPagedList<SysRole>()
            {
                PageIndex = dto.Page,
                PageSize = dto.Limit,
                Items = list,
                TotalCount = total
            };
        }

        /// <summary>
        /// 添加编辑角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public void AddEdit(RoleAddEditDto dto)
        {
            dto.RoleName = dto.RoleName.Trim();
            SysRole role = null;
            if (dto.RoleId.HasValue)
            {
                if (dbClint.Queryable<SysRole>().Where(r => r.RoleName == dto.RoleName && r.IsDeleted == false && r.RoleId != dto.RoleId.Value).Any())
                    throw Oops.Oh($"已存在角色【{dto.RoleName}】!");
                role = _repository.FirstOrDefault(r=>r.RoleId==dto.RoleId.Value);
                role.ModifyDate = DateTime.Now;
                role.ModifyUserId = _userManager.UserId;
                role.ModifyUserName = _userManager.UserName;
            }
            else
            {
                if (dbClint.Queryable<SysRole>().Where(r => r.RoleName == dto.RoleName && r.IsDeleted == false).Any())
                    throw Oops.Oh($"已存在角色【{dto.RoleName}】!");
                role = new SysRole()
                {
                    CreateDate = DateTime.Now,
                    CreateUserId = _userManager.UserId,
                    CreateUserName = _userManager.UserName,
                    IsDeleted = false
                };
            }
            role.RoleName = dto.RoleName;
            role.Remark = dto.Remark;
            role.IsEnabled = dto.IsEnabled;

            if (dto.RoleId.HasValue)
                dbClint.Updateable(role).ExecuteCommand();
            else
                dbClint.Insertable(role).ExecuteCommand();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ids"></param>
        public bool Delete(long[] ids)
        {
            var count = dbClint.Updateable<SysRole>().SetColumns(u => new SysRole
            {
                DeleteUserId = _userManager.UserId,
                DeleteUserName = _userManager.UserName,
                DeleteDate = DateTime.Now,
                IsDeleted = true
            }).Where(u => ids.Contains(u.RoleId)).ExecuteCommand();
            return count > 0;
        }

    }
}
