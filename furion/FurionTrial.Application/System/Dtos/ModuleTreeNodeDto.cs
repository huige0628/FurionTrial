using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    /// <summary>
    /// 模块树
    /// </summary>
    public class ModuleTreeNodeDto
    {
        public long Id { get; set; }
        public long RootId { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public string Code { get; set; }
        public int? Level { get; set; }
        public List<ModuleTreeNodeDto> Children { get; set; }
    }
}
