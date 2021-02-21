using SqlSugar;

namespace FurionTrial.Core.Entity
{
    /// <summary>
    /// 权限关联表
    /// </summary>
    [SugarTable("dbo.SysRelevance")]
    public class SysRelevance
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int64 RelevanceId { get; set; }

        /// <summary>
        /// 关联key
        /// </summary>
        public System.String Key { get; set; }

        /// <summary>
        /// 关联主表主键id
        /// </summary>
        public System.Int64 FirstId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Int64 SecondId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String Remark { get; set; }

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