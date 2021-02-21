using SqlSugar;

namespace FurionTrial.Core.Entity
{
    /// <summary>
    /// 角色表
    /// </summary>
    [SugarTable("dbo.SysRole")]
    public class SysRole
    {
        /// <summary>
        /// 角色id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int64 RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public System.String RoleName { get; set; }

        /// <summary>
        /// 是否启用[0否 1是]
        /// </summary>
        public System.Boolean IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public System.String Remark { get; set; }

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