using Furion.DependencyInjection;
using FurionTrial.Core.Entity;
using FurionTrial.Core.Enums;
using Mapster;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    public class SysRelevanceService:ISysRelevanceService, ITransient
    {
        private readonly ISqlSugarRepository<SysRelevance> _repository;
        private readonly ISqlSugarClient dbClint; // 核心对象：拥有完整的SqlSugar全部功能
        public SysRelevanceService(ISqlSugarRepository<SysRelevance> sqlSugarRepository)
        {
            _repository = sqlSugarRepository;
            dbClint = _repository.Context;
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SysRole> GetUserRoles(long userId)
        {
            return dbClint.Queryable<SysRole>().Where(r => r.IsDeleted == false && SqlFunc.Subqueryable<SysRelevance>().Where(s => s.FirstId == userId && r.RoleId == s.SecondId && s.Key == SysRelevanceEnum.SysUser_SysRole.ToString()).Any()).ToList();
        }

        /// <summary>
        /// 获取用户模块
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserModuleDto> GetUserModules(long userId)
        {
            var list = dbClint.Queryable<SysRelevance, SysRelevance, SysModule>((r1, r2, m) => new object[]
            {
                JoinType.Inner,r1.SecondId==r2.FirstId && r2.Key==SysRelevanceEnum.SysRole_SysModule.ToString() && r2.IsDeleted==false,
                JoinType.Inner,r2.SecondId==m.ModuleId && m.IsDeleted==false
            })
                .Where((r1, r2, m) => r1.FirstId == userId && r1.IsDeleted == false && r1.Key == SysRelevanceEnum.SysUser_SysRole.ToString())
                 .Select((r1, r2, m) => m).Distinct().ToList();
            var result = list.Adapt<List<UserModuleDto>>();
            return result;
        }
    }
}
