using SqlSugar;

namespace FurionTrial.Core.Entity
{
    /// <summary>
    /// 部门表
    /// </summary>
    [SugarTable("dbo.SysOrg")]
    public class SysOrg
    {
        /// <summary>
        /// 部门id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int64 OrgId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public System.String OrgName { get; set; }

        /// <summary>
        /// 父级部门id
        /// </summary>
        public System.Int64? ParentOrgId { get; set; }

        /// <summary>
        /// 父级部门名称
        /// </summary>
        public System.String ParentOrgName { get; set; }

        /// <summary>
        /// 是否叶子节点[0否 1是]
        /// </summary>
        public System.Boolean IsLeaf { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String CascadeId { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public System.String IconName { get; set; }

        /// <summary>
        /// 状态[0禁用 1启用 ]
        /// </summary>
        public System.Int32 Status { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public System.Int32 SortNo { get; set; }

        /// <summary>
        /// 公司id
        /// </summary>
        public System.Int32? CompanyId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public System.Int64 CreateUserId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public System.String CreateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public System.DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 修改人id
        /// </summary>
        public System.Int64? ModifyUserId { get; set; }

        /// <summary>
        /// 修改人名称
        /// </summary>
        public System.String ModifyUserName { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public System.DateTime? DeleteDate { get; set; }

        /// <summary>
        /// 删除人id
        /// </summary>
        public System.Int64? DeleteUserId { get; set; }

        /// <summary>
        /// 删除人名称
        /// </summary>
        public System.String DeleteUserName { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public System.Boolean IsDeleted { get; set; }
    }
}