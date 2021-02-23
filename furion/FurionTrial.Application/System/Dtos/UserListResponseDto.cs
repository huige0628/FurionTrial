using System;

namespace FurionTrial.Application
{
    public class UserListResponseDto
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public Int64 UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public String UserCode { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public String Phone { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public String Qq { get; set; }

        /// <summary>
        /// 性别[0男 1女]
        /// </summary>
        public Int32 Sex { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public String Avatar { get; set; }

        /// <summary>
        /// 上级
        /// </summary>
        public String Manager { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        public String Job { get; set; }

        /// <summary>
        /// 类型(0:普通,1:管理员)
        /// </summary>
        public Int32 Type { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Int32 Status { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// 最后登录ip
        /// </summary>
        public String LastLoginIp { get; set; }

        /// <summary>
        /// 是否放开IP访问[0否 1是]
        /// </summary>
        public Boolean IpOpen { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public String CreateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 修改人名称
        /// </summary>
        public String ModifyUserName { get; set; }
    }
}
