using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    public class RoleModuleTreeNodeDto
    {
        public long Id { get; set; }
        public string Label { get; set; }
        public List<ModuleElementDto> Action { get; set; }
        public List<RoleModuleTreeNodeDto> Children { get; set; }
    }

    /// <summary>
    /// 模块对应权限
    /// </summary>
    public class ModuleElementDto
    {
        public long ModuleId { get; set; }
        public long ElementId { get; set; }
        public string ElementName { get; set; }
        public bool Checked { get; set; }
    }
}
