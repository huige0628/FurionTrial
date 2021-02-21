using Furion;
using Furion.DataEncryption;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using FurionTrial.Core.Entity;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public SystemAppService(
            ISysRelevanceService sysRelevanceService,
            ISysUserSerivce sysUserSerivce)
        {
            _sysRelevanceService = sysRelevanceService;
            _sysUserSerivce = sysUserSerivce;
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

        /// <summary>
        /// 根据用户ID获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [ApiDescriptionSettings(KeepName =true)]
        public SysUser GetUserById(long userId)
        {
            return _sysUserSerivce.GetUserById(userId);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public LoginResponseDto Login([FromServices] IHttpContextAccessor httpContextAccessor, [FromBody]LoginRequestDto dto)
        {
            var user = _sysUserSerivce.Login(dto.Account, dto.Password);
            if (user == null)
                throw Oops.Oh("用户名或者密码错误");

            var response= user.Adapt<LoginResponseDto>();
            // 生成 token
            response.AccessToken= JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { "UserId",user.UserId },
                { "Account",user.UserName },
                { "UserInfo",response },
            });
            response.ExipreTime = DateTimeOffset.Now.AddMinutes(20).DateTime;
            // 设置 Swagger 自动登录
            httpContextAccessor.SigninToSwagger(response.AccessToken);
            return response;
        }

    }
}
