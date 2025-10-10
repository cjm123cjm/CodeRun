﻿namespace CodeRun.Services.Domain.CustomerException
{
    /// <summary>
    /// 业务异常
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }
        public BusinessException(string message, Exception innerException) : base(message, innerException) { }

        public int Code { get; }
        public BusinessException(int code, string message) : base(message)
        {
            Code = code;
        }
    }
}
