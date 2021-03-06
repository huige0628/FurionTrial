﻿using Furion;
using Furion.DataEncryption;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using FurionTrial.Core.Entity;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace FurionTrial.Application
{
    /// <summary>
    /// 系统服务接口
    /// </summary>
    public class SystemAppService : IDynamicApiController
    {
        private readonly ISysRelevanceService _sysRelevanceService;
        private readonly ISysUserSerivce _sysUserSerivce;
        private readonly ISysOrgService _sysOrgService;
        private readonly ISysRoleService _sysRoleService;
        private readonly ISysModuleService _sysModuleService;
        public SystemAppService(
            ISysRelevanceService sysRelevanceService,
            ISysUserSerivce sysUserSerivce,
            ISysOrgService sysOrgService,
            ISysRoleService sysRoleService,
            ISysModuleService sysModuleService
            )
        {
            _sysRelevanceService = sysRelevanceService;
            _sysUserSerivce = sysUserSerivce;
            _sysOrgService = sysOrgService;
            _sysRoleService = sysRoleService;
            _sysModuleService = sysModuleService;
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [ApiDescriptionSettings(KeepName = true)]
        public List<SysRole> GetUserRoles(long userId)
        {
            return _sysRelevanceService.GetUserRoles(userId);
        }

        /// <summary>
        /// 获取用户模块
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [ApiDescriptionSettings(KeepName = true)]
        public List<UserModuleDto> GetUserModules(long userId)
        {
            return _sysRelevanceService.GetUserModules(userId);
        }

        #region 用户管理
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public LoginResponseDto Login([FromServices] IHttpContextAccessor httpContextAccessor, [FromBody] LoginRequestDto dto)
        {
            var user = _sysUserSerivce.Login(dto.Account, dto.Password);
            if (user == null)
                throw Oops.Oh("用户名或者密码错误");

            var response = user.Adapt<LoginResponseDto>();
            // 生成 token
            response.AccessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { "UserId",user.UserId },
                { "UserName",user.UserName },
                { "Account",user.UserName }
            });
            response.ExipreTime = DateTimeOffset.Now.AddMinutes(20).DateTime;
            // 设置 Swagger 自动登录
            httpContextAccessor.SigninToSwagger(response.AccessToken);
            return response;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [ApiDescriptionSettings(KeepName = true)]
        [HttpPost]
        public SqlSugarPagedList<UserListResponseDto> GetUserList([FromBody] UserListRequestDto dto)
        {
            return _sysUserSerivce.GetUserList(dto);
        }

        /// <summary>
        /// 用户添加编辑
        /// </summary>
        /// <param name="dto"></param>
        [ApiDescriptionSettings(KeepName = true)]
        [HttpPost]
        public bool UserAddEdit([FromBody] UserAddEditDto dto)
        {
            _sysUserSerivce.AddEdit(dto);
            return true;
        }

        /// <summary>
        /// 用户删除
        /// </summary>
        /// <param name="userId"></param>
        [ApiDescriptionSettings(KeepName = true)]
        [HttpPost]
        public bool UserRemove([FromBody] long[] userId)
        {
            return _sysUserSerivce.Delete(userId);
        }

        /// <summary>
        /// 用户设置角色
        /// </summary>
        /// <param name="dto"></param>
        [ApiDescriptionSettings(KeepName = true)]
        [HttpPost]
        public bool UserSetRole([FromBody] UserSetRoleDto dto)
        {
            _sysRelevanceService.SetUserRole(dto);
            return true;
        }
        #endregion

        #region 角色管理
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        [ApiDescriptionSettings(KeepName = true)]
        public List<SysRole> GetAllRoles()
        {
            return _sysRoleService.GetAllRoles();
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [ApiDescriptionSettings(KeepName = true)]
        [HttpPost]
        public SqlSugarPagedList<SysRole> GetRoleList([FromBody] RoleListRequestDto dto)
        {
            return _sysRoleService.GetRoleList(dto);
        }

        /// <summary>
        /// 角色添加编辑
        /// </summary>
        /// <param name="dto"></param>
        [ApiDescriptionSettings(KeepName = true)]
        [HttpPost]
        public bool RoleAddEdit([FromBody] RoleAddEditDto dto)
        {
            _sysRoleService.AddEdit(dto);
            return true;
        }

        /// <summary>
        /// 角色删除
        /// </summary>
        /// <param name="roleIds"></param>
        [ApiDescriptionSettings(KeepName = true)]
        [HttpPost]
        public bool RoleRemove([FromBody] long[] roleIds)
        {
            return _sysRoleService.Delete(roleIds);
        }

        /// <summary>
        /// 获取角色模块权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [ApiDescriptionSettings(KeepName = true)]
        [QueryParameters]
        public List<long> GetRoleModule(long roleId)
        {
            return _sysRelevanceService.GetRoleModule(roleId);
        }

        /// <summary>
        /// 获取模块权限树
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [ApiDescriptionSettings(KeepName = true)]
        [QueryParameters]
        public List<RoleModuleTreeNodeDto> GetRoleModuleTree(long roleId)
        {
            return _sysModuleService.GetRoleModuleTree(roleId);
        }

        #endregion

        #region 部门管理
        /// <summary>
        /// 获取部门树
        /// </summary>
        /// <param name="isRoot"></param>
        /// <returns></returns>
        [ApiDescriptionSettings(KeepName = true)]
        [QueryParameters]
        [HttpGet]
        public List<OrgTreeNodeDto> GetOrgTree(bool isRoot)
        {
            return _sysOrgService.GetOrgTree(isRoot);
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [ApiDescriptionSettings(KeepName = true)]
        [HttpPost]
        public SqlSugarPagedList<SysOrg> GetOrgList([FromBody] OrgListRequestDto dto)
        {
            return _sysOrgService.GetOrgList(dto);
        }

        /// <summary>
        /// 部门添加编辑
        /// </summary>
        /// <param name="dto"></param>
        [ApiDescriptionSettings(KeepName = true)]
        [HttpPost]
        public bool OrgAddEdit([FromBody] OrgAddEditDto dto)
        {
            _sysOrgService.AddEdit(dto);
            return true;
        }

        /// <summary>
        /// 部门删除
        /// </summary>
        /// <param name="roleIds"></param>
        [ApiDescriptionSettings(KeepName = true)]
        [HttpPost]
        public bool OrgRemove([FromBody] long[] roleIds)
        {
            return _sysOrgService.Delete(roleIds);
        }
        #endregion

        #region 模块管理
        /// <summary>
        /// 获取模块树
        /// </summary>
        /// <returns></returns>
        [ApiDescriptionSettings(KeepName = true)]
        public List<ModuleTreeNodeDto> GetModuleTree()
        {
            return _sysModuleService.GetModuleTree();
        }

        /// <summary>
        /// 获取模块按钮列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [ApiDescriptionSettings(KeepName = true)]
        [HttpPost]
        public SqlSugarPagedList<SysModuleElement> GetModuleElementList(ModuleElementListRequestDto dto)
        {
            return _sysModuleService.GetModuleElementList(dto);
        }

        /// <summary>
        /// 根据模块id获取模块信息
        /// </summary>
        /// <returns></returns>
        [ApiDescriptionSettings(KeepName = true)]
        [QueryParameters]
        public SysModule GetModuleById(long moduleId)
        {
            return _sysModuleService.GetModuleById(moduleId);
        }

        /// <summary>
        /// 模块添加编辑
        /// </summary>
        /// <param name="dto"></param>
        [ApiDescriptionSettings(KeepName = true)]
        [HttpPost]
        public bool ModuleAddEdit([FromBody] ModuleAddEditDto dto)
        {
            _sysModuleService.AddEdit(dto);
            return true;
        }

        /// <summary>
        /// 模块删除
        /// </summary>
        /// <param name="roleIds"></param>
        [ApiDescriptionSettings(KeepName = true)]
        [HttpPost]
        public bool ModuleRemove([FromBody] long[] roleIds)
        {
            return _sysModuleService.Delete(roleIds);
        }

        #endregion
    }
}
