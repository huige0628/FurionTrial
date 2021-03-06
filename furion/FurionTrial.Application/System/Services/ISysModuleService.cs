﻿using FurionTrial.Core.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    public interface ISysModuleService
    {
        /// <summary>
        /// 获取模块权限树
        /// </summary>
        /// <returns></returns>
        List<RoleModuleTreeNodeDto> GetRoleModuleTree(long roleId);

        /// <summary>
        /// 获取模块树
        /// </summary>
        /// <returns></returns>
        List<ModuleTreeNodeDto> GetModuleTree();

        /// <summary>
        /// 根据模块id获取模块信息
        /// </summary>
        /// <returns></returns>
        SysModule GetModuleById(long moduleId);

        /// <summary>
        /// 获取模块按钮列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        SqlSugarPagedList<SysModuleElement> GetModuleElementList(ModuleElementListRequestDto dto);

        /// <summary>
        /// 添加编辑模块
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        void AddEdit(ModuleAddEditDto dto);

        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="ids"></param>
        bool Delete(long[] ids);
    }
}
