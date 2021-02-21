using SqlSugar;

namespace FurionTrial.Core.Entity
{
    /// <summary>
    /// bestwo用户
    /// </summary>
    [SugarTable("dbo.SysUser")]
    public class SysUser
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int64 UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public System.String UserName { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public System.String UserCode { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public System.String Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public System.String Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public System.String Phone { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public System.String Qq { get; set; }

        /// <summary>
        /// 性别[0男 1女]
        /// </summary>
        public System.Int32 Sex { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public System.String Avatar { get; set; }

        /// <summary>
        /// 上级
        /// </summary>
        public System.String Manager { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        public System.String Job { get; set; }

        /// <summary>
        /// 类型(0:普通,1:管理员)
        /// </summary>
        public System.Int32 Type { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public System.Int32 Status { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public System.DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// 最后登录ip
        /// </summary>
        public System.String LastLoginIp { get; set; }

        /// <summary>
        /// 是否放开IP访问[0否 1是]
        /// </summary>
        public System.Boolean IpOpen { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public System.Int64 CreateUserId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public System.String CreateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public System.DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 修改人id
        /// </summary>
        public System.Int64? ModifyUserId { get; set; }

        /// <summary>
        /// 修改人名称
        /// </summary>
        public System.String ModifyUserName { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public System.DateTime? DeleteDate { get; set; }

        /// <summary>
        /// 删除人id
        /// </summary>
        public System.Int64? DeleteUserId { get; set; }

        /// <summary>
        /// 删除人名称
        /// </summary>
        public System.String DeleteUserName { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public System.Boolean IsDeleted { get; set; }
    }
}