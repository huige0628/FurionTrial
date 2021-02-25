using Furion.DependencyInjection;
using FurionTrial.Core.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    public class SysRoleService:ISysRoleService, ITransient
    {
        private readonly ISqlSugarRepository<SysRole> _repository;
        private readonly ISqlSugarClient dbClint; // 核心对象：拥有完整的SqlSugar全部功能
        public SysRoleService(ISqlSugarRepository<SysRole> sqlSugarRepository)
        {
            _repository = sqlSugarRepository;
            dbClint = _repository.Context;
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public List<SysRole> GetAllRoles()
        {
            return dbClint.Queryable<SysRole>().Where(r => r.IsDeleted == false).ToList();
        }
    }
}
