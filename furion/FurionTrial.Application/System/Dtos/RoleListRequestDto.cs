using FurionTrial.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    public class RoleListRequestDto : ApiPageRequest
    {
        public string RoleName { get; set; }
    }
}
