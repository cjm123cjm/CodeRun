namespace CodeRun.Services.IService.Dtos
{
    /// <summary>
    /// 统一返回结果
    /// </summary>
    public class ResponseDto
    {
        /// <summary>
        /// 200-成功，404-地址不存在,600-请求参数错误,500-服务器错误
        /// </summary>
        public int Code { get; set; } = 200;

        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; } = null;
        public object? Result { get; set; } = null;
        public ResponseDto()
        {
            
        }

        public ResponseDto(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public ResponseDto(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public ResponseDto(object result)
        {
            Result = result;
        }
    }
}
