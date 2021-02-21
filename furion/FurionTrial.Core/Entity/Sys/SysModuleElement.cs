using SqlSugar;

namespace FurionTrial.Core.Entity
{
    /// <summary>
    /// 模块权限表
    /// </summary>
    [SugarTable("dbo.SysModuleElement")]
    public class SysModuleElement
    {
        /// <summary>
        /// 按钮id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int64 ElementId { get; set; }

        /// <summary>
        /// 按钮名称
        /// </summary>
        public System.String ElementName { get; set; }

        /// <summary>
        /// 模块id
        /// </summary>
        public System.Int64 ModuleId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public System.Int32 SortNo { get; set; }

        /// <summary>
        /// 权限值
        /// </summary>
        public System.String Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String DomId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String Attr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String Script { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String Class { get; set; }

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