using SqlSugar;

namespace FurionTrial.Core.Entity
{
    /// <summary>
    /// 模块表
    /// </summary>
    [SugarTable("dbo.SysModule")]
    public class SysModule
    {
        /// <summary>
        /// 模块id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int64 ModuleId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public System.String ModuleName { get; set; }

        /// <summary>
        /// 父模块id
        /// </summary>
        public System.Int64? ParentModuleId { get; set; }

        /// <summary>
        /// 父模块名称
        /// </summary>
        public System.String ParentModuleName { get; set; }

        /// <summary>
        /// 模块地址
        /// </summary>
        public System.String Url { get; set; }

        /// <summary>
        /// 是否叶子节点[0否 1是]
        /// </summary>
        public System.Boolean IsLeaf { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public System.String Icon { get; set; }

        /// <summary>
        /// 模块编码
        /// </summary>
        public System.String Code { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        public System.Int32? Level { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? SortNo { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public System.Boolean IsEnabled { get; set; }

        /// <summary>
        /// 
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