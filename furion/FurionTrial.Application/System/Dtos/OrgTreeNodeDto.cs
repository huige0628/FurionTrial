using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    /// <summary>
    /// 组织结构树
    /// </summary>
    public class OrgTreeNodeDto
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public List<OrgTreeNodeDto> Children { get; set; }
    }
}
