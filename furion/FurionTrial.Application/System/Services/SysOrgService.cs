using Furion.DependencyInjection;
using Furion.FriendlyException;
using FurionTrial.Core;
using FurionTrial.Core.Entity;
using FurionTrial.Core.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    /// <summary>
    /// 组织结构类
    /// </summary>
    public class SysOrgService : ISysOrgService, ITransient
    {
        private readonly ISqlSugarRepository<SysOrg> _repository;
        private readonly ISqlSugarClient dbClint; // 核心对象：拥有完整的SqlSugar全部功能
        private readonly IUserManager _userManager;
        public SysOrgService(ISqlSugarRepository<SysOrg> sqlSugarRepository, IUserManager userManager)
        {
            _repository = sqlSugarRepository;
            dbClint = _repository.Context;
            _userManager = userManager;
        }

        /// <summary>
        /// 获取部门树
        /// </summary>
        /// <param name="isRoot"></param>
        /// <returns></returns>
        public List<OrgTreeNodeDto> GetOrgTree(bool isRoot)
        {
            var orgTree = new List<OrgTreeNodeDto>();
            //如果选中，则只加载选中的公司
            var orgs = dbClint.Queryable<SysOrg>().Where(o => o.IsDeleted == false && o.Status == 1).ToList();
            if (isRoot)
            {
                orgTree.Add(new OrgTreeNodeDto()
                {
                    Id = "0",
                    Label = "根目录"
                });
            }
            orgs.Where(o => o.ParentOrgId == 0).ToList().ForEach(o =>
            {
                orgTree.Add(new OrgTreeNodeDto()
                {
                    Id = o.OrgId.ToString(),
                    Label = o.OrgName,
                    Children = GetOrgNode(o.OrgId, orgs)
                });
            });
            return orgTree;
        }
        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="orgList"></param>
        /// <returns></returns>
        private List<OrgTreeNodeDto> GetOrgNode(long? parentId, List<SysOrg> orgList)
        {
            if (orgList == null || orgList.Count == 0) return null;
            var nodeList = orgList.FindAll(c => c.ParentOrgId == parentId).Select(o => new OrgTreeNodeDto()
            {
                Id = o.OrgId.ToString(),
                Label = o.OrgName,
                Children = GetOrgNode(o.OrgId, orgList)
            }).ToList();
            return nodeList;
        }

        /// <summary>
        /// 获取用户部门
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserOrgDto> GetUserOrg(List<long> userId)
        {
            //用户关联部门
            var reKey = SysRelevanceEnum.SysUser_SysOrg.ToString();
            var list = dbClint.Queryable<SysOrg, SysRelevance>((so, sr) => new object[] { JoinType.Inner, so.OrgId == sr.SecondId })
                .Where((so, sr) => sr.Key == reKey && userId.Contains(sr.FirstId) && so.IsDeleted == false && sr.IsDeleted == false)
                .Select((so, sr) => new UserOrgDto
                {
                    UserId = sr.FirstId,
                    OrgId = so.OrgId,
                    OrgName = so.OrgName
                }).ToList();
            return list;
        }

        /// <summary>
        /// 获取所有子集
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public List<SysOrg> GetAllChild(List<long> orgId)
        {
            var self = dbClint.Queryable<SysOrg>().Where(o => orgId.Contains(o.OrgId) && o.IsDeleted == false).ToList();
            return GetOrgChild(self);
        }
        private List<SysOrg> GetOrgChild(List<SysOrg> orgList)
        {
            var orgId = orgList.Select(o => o.OrgId).ToArray();
            var childList = dbClint.Queryable<SysOrg>().Where(o => orgId.Contains(o.ParentOrgId.Value) && o.IsDeleted == false).ToList();
            if (childList != null && childList.Count > 0)
            {
                orgList.AddRange(childList);
                GetOrgChild(childList);
            }
            return orgList;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public SqlSugarPagedList<SysOrg> GetOrgList(OrgListRequestDto dto)
        {
            var query = dbClint.Queryable<SysOrg>().Where(o => o.IsDeleted == false);
            query.WhereIF(!string.IsNullOrWhiteSpace(dto.OrgName), o => o.OrgName.Contains(dto.OrgName.Trim()));
            query.WhereIF(dto.ParentOrgId.HasValue, o => o.ParentOrgId == dto.ParentOrgId || o.OrgId == dto.ParentOrgId);
            int total = 0;
            var list = query.ToPageList(dto.Page, dto.Limit, ref total);
            return new SqlSugarPagedList<SysOrg>()
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
        public void AddEdit(OrgAddEditDto dto)
        {
            dto.OrgName = dto.OrgName.Trim();
            SysOrg org = null;
            if (dto.OrgId.HasValue)
            {
                org = _repository.FirstOrDefault(o=>o.OrgId==dto.OrgId.Value);
                org.ModifyDate = DateTime.Now;
                org.ModifyUserId = _userManager.UserId;
                org.ModifyUserName = _userManager.UserName;
            }
            else
            {
                org = new SysOrg()
                {
                    CreateDate = DateTime.Now,
                    CreateUserId = _userManager.UserId,
                    CreateUserName = _userManager.UserName,
                    IsLeaf = false,
                    IsDeleted = false
                };
            }
            org.OrgName = dto.OrgName;
            org.ParentOrgId = dto.ParentOrgId;
            org.ParentOrgName = dto.ParentOrgId > 0 ? dto.ParentOrgName : "";
            org.Status = dto.Status;
            org.SortNo = dto.SortNo ?? 0;
            if (org.ParentOrgId > 0)
            {
                if (dbClint.Queryable<SysOrg>().Where(o => o.OrgName == dto.OrgName && o.ParentOrgId == dto.ParentOrgId && o.OrgId != dto.OrgId && o.IsDeleted == false).Any())
                {
                    throw Oops.Oh("同部门下的子部门不能重复!");
                }
            }
            if (dto.OrgId.HasValue)
                dbClint.Updateable(org).ExecuteCommand();
            else
                dbClint.Insertable(org).ExecuteCommand();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ids"></param>
        public bool Delete(long[] ids)
        {
            var count = dbClint.Updateable<SysOrg>().SetColumns(u => new SysOrg
            {
                DeleteUserId = _userManager.UserId,
                DeleteUserName = _userManager.UserName,
                DeleteDate = DateTime.Now,
                IsDeleted = true,
                Status = 0
            }).Where(u => ids.Contains(u.OrgId)).ExecuteCommand();
            return count > 0;
        }

    }
}
