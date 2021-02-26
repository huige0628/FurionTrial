using Furion.DependencyInjection;
using FurionTrial.Core;
using FurionTrial.Core.Entity;
using FurionTrial.Core.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application.System.Services
{
    public class SysModuleService : ISysModuleService, ITransient
    {
        private readonly ISqlSugarRepository<SysModule> _repository;
        private readonly ISqlSugarClient dbClint; // 核心对象：拥有完整的SqlSugar全部功能
        /// <summary>
        /// 用户管理类
        /// </summary>
        private readonly IUserManager _userManager;

        public SysModuleService(ISqlSugarRepository<SysModule> sqlSugarRepository, IUserManager userManager)
        {
            _repository = sqlSugarRepository;
            dbClint = _repository.Context;
            _userManager = userManager;
        }

        /// <summary>
        /// 获取模块权限树
        /// </summary>
        /// <returns></returns>
        public List<RoleModuleTreeNodeDto> GetRoleModuleTree(long roleId)
        {
            var moduleTree = new List<RoleModuleTreeNodeDto>();
            var modules = dbClint.Queryable<SysModule>().Where(o => o.IsDeleted == false).ToList();

            //所有模块
            var moduleElementDtos = dbClint.Queryable<SysModule, SysModuleElement>((m, e) => new object[]
           {
                JoinType.Inner,m.ModuleId==e.ModuleId
           }).Where((m, e) => m.IsDeleted == false && e.IsDeleted == false)
            .Select((m, e) => new ModuleElementDto()
            {
                ModuleId = e.ModuleId,
                ElementId = e.ElementId,
                ElementName = e.ElementName
            }).ToList();

            //当前角色拥有的模块权限
            var roleModuleKey = SysRelevanceEnum.SysRole_SysModule.ToString();
            var roleModuleElementKey = SysRelevanceEnum.SysRole_SysModule_Element.ToString();
            var roleModuleElements = dbClint.Queryable<SysRelevance, SysRelevance>((a, b) => new object[]
             {
                    JoinType.Inner,a.RelevanceId==b.FirstId
             })
            .Where((a, b) => a.IsDeleted == false && b.IsDeleted == false && a.Key == roleModuleKey && a.FirstId == roleId && b.Key == roleModuleElementKey)
            .Select((a, b) => new ModuleElementDto()
            {
                ModuleId = a.SecondId,
                ElementId = b.SecondId
            }).ToList();

            modules.Where(o => o.ParentModuleId == 0).ToList().ForEach(o =>
            {
                moduleTree.Add(new RoleModuleTreeNodeDto()
                {
                    Id = o.ModuleId,
                    Label = o.ModuleName,
                    Action = moduleElementDtos.Where(e => e.ModuleId == o.ModuleId).Select(e => new ModuleElementDto()
                    {
                        ElementId = e.ElementId,
                        ElementName = e.ElementName,
                        Checked = roleModuleElements.Where(r => r.ModuleId == e.ModuleId && r.ElementId == e.ElementId).Any()
                    }).ToList(),
                    Children = GetRoleModuleNode(o.ModuleId, modules, moduleElementDtos, roleModuleElements)
                }); ;
            });
            return moduleTree;
        }
        private List<RoleModuleTreeNodeDto> GetRoleModuleNode(long? parentId, List<SysModule> moduleList, List<ModuleElementDto> moduleElementDtos, List<ModuleElementDto> roleModuleElements)
        {
            if (moduleList == null || moduleList.Count == 0) return null;
            var nodeList = moduleList.FindAll(c => c.ParentModuleId == parentId).Select(o => new RoleModuleTreeNodeDto()
            {
                Id = o.ModuleId,
                Label = o.ModuleName,
                Action = moduleElementDtos.Where(e => e.ModuleId == o.ModuleId).Select(e => new ModuleElementDto()
                {
                    ElementId = e.ElementId,
                    ElementName = e.ElementName,
                    Checked = roleModuleElements.Where(r => r.ModuleId == e.ModuleId && r.ElementId == e.ElementId).Any()
                }).ToList(),
                Children = GetRoleModuleNode(o.ModuleId, moduleList, moduleElementDtos, roleModuleElements)
            }).ToList();
            return nodeList;
        }

        /// <summary>
        /// 获取模块树
        /// </summary>
        /// <returns></returns>
        public List<ModuleTreeNodeDto> GetModuleTree()
        {
            var moduleTree = new List<ModuleTreeNodeDto>();
            var modules = dbClint.Queryable<SysModule>().Where(o => o.IsDeleted == false).ToList();
            modules.Where(o => o.ParentModuleId == 0).ToList().ForEach(o =>
            {
                moduleTree.Add(new ModuleTreeNodeDto()
                {
                    Id = o.ModuleId,
                    Title = o.ModuleName,
                    Icon = o.Icon,
                    Path = o.Url,
                    Code=o.Code,
                    Level=o.Level,
                    Children = GetModuleNode(o.ModuleId, o.ModuleId, modules)
                });
            });
            return moduleTree;
        }
        private List<ModuleTreeNodeDto> GetModuleNode(long? parentId, long rootId, List<SysModule> moduleList)
        {
            if (moduleList == null || moduleList.Count == 0) return null;
            var nodeList = moduleList.FindAll(c => c.ParentModuleId == parentId).Select(o => new ModuleTreeNodeDto()
            {
                Id = o.ModuleId,
                Title = o.ModuleName,
                Icon = o.Icon,
                Path = o.Url,
                RootId = rootId,
                Code=o.Code,
                Level = o.Level,
                Children = GetModuleNode(o.ModuleId, rootId, moduleList)
            }).ToList();
            return nodeList;
        }

        /// <summary>
        /// 获取模块按钮列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public SqlSugarPagedList<SysModuleElement> GetModuleElementList(ModuleElementListRequestDto dto)
        {
            var query = dbClint.Queryable<SysModuleElement>().Where(e => e.IsDeleted == false);
            query.WhereIF(dto.ModuleId.HasValue,e=>e.ModuleId==dto.ModuleId);
            int total = 0;
            var list = query.ToPageList(dto.Page, dto.Limit, ref total);
            return new SqlSugarPagedList<SysModuleElement>()
            {
                PageIndex = dto.Page,
                PageSize = dto.Limit,
                Items = list,
                TotalCount = total
            };
        }
    }
}
