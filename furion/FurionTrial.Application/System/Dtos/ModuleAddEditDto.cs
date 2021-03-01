using System;

namespace FurionTrial.Application
{
    public class ModuleAddEditDto
    {
        /// <summary>
        /// 模块id
        /// </summary>
        public Int64? ModuleId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public String ModuleName { get; set; }

        /// <summary>
        /// 父模块id
        /// </summary>
        public Int64? ParentModuleId { get; set; }

        /// <summary>
        /// 父模块名称
        /// </summary>
        public String ParentModuleName { get; set; }

        /// <summary>
        /// 模块地址
        /// </summary>
        public String Url { get; set; }

        /// <summary>
        /// 是否叶子节点[0否 1是]
        /// </summary>
        public Boolean IsLeaf { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32? SortNo { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public String Icon { get; set; }

        /// <summary>
        /// 模块编码
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        public Int32? Level { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public Boolean IsEnabled { get; set; }

    }
}
