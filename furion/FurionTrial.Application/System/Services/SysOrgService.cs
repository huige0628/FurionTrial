using Furion.DependencyInjection;
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
        public SysOrgService(ISqlSugarRepository<SysOrg> sqlSugarRepository)
        {
            _repository = sqlSugarRepository;
            dbClint = _repository.Context;
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

    }
}
