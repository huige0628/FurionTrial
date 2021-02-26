using FurionTrial.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    public class ModuleElementListRequestDto : ApiPageRequest
    {
        public long? ModuleId { get; set; }
    }
}
