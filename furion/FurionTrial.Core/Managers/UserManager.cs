using Furion.DependencyInjection;
using FurionTrial.Core.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Core
{
    public class UserManager : IUserManager, IScoped
    {
        /// <summary>
        /// 用户仓储
        /// </summary>
        private readonly ISqlSugarRepository<SysUser> _userRepository;

        /// <summary>
        /// HttpContext 访问器
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 缓存
        /// </summary>
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="userRepository"></param>
        /// <param name="memoryCache"></param>
        public UserManager(IHttpContextAccessor httpContextAccessor
            , ISqlSugarRepository<SysUser> userRepository
            , IMemoryCache memoryCache)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 获取用户 Id
        /// </summary>
        public int UserId { get => int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId").Value); }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        public SysUser User { get => _userRepository.FirstOrDefault(u => u.UserId == UserId); }
    }
}
