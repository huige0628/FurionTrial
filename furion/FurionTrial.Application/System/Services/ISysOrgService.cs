using FurionTrial.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    /// <summary>
    /// 组织结构接口
    /// </summary>
    public interface ISysOrgService
    {
        /// <summary>
        /// 获取部门树
        /// </summary>
        /// <param name="isRoot"></param>
        /// <returns></returns>
        List<OrgTreeNodeDto> GetOrgTree(bool isRoot);

        /// <summary>
        /// 获取所有子集
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        List<SysOrg> GetAllChild(List<long> orgId);

        /// <summary>
        /// 获取用户部门
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<UserOrgDto> GetUserOrg(List<long> userId);
    }
}
