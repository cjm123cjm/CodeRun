﻿namespace CodeRun.Services.IService.Dtos.Inputs
{
    /// <summary>
    /// 修改用户状态输入参数
    /// </summary>
    public class UpdateAccountStatusInput
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 状态：0-禁用，1-启用
        /// </summary>
        public short Status { get; set; }
    }
}
