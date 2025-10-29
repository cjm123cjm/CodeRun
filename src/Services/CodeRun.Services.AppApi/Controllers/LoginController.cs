using CodeRun.Services.Common.Captcha;
using CodeRun.Services.Common.RedisUtil;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Interfaces.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeRun.Services.AppApi.Controllers
{
    /// <summary>
    /// 登录/注册
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAppUserInfoService _userInfoService;

        public LoginController(IAppUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseDto CheckCode()
        {
            VerifyCode codeInfo = CreateCaptcha.CreateVerifyCode(4, VerifyCodeType.CHAR);

            //保存到redis中
            CacheManager.Set(RedisKeyPrefix.VerifyCode + codeInfo.CodeKey, codeInfo, TimeSpan.FromMinutes(5));


            ResponseDto dto = new ResponseDto()
            {
                Code = 200,
                Result = new { codeInfo.Image, codeInfo.CodeKey }
            };

            return dto;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerInput"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> Register(AppRegisterInput registerInput)
        {
            var codeInfo = CacheManager.Get<VerifyCode>(RedisKeyPrefix.VerifyCode + registerInput.CheckCodeKey);
            if (codeInfo == null || !codeInfo.Code.Equals(registerInput.CheckCode))
            {
                return new ResponseDto(false, "验证码不正确");
            }
            try
            {
                await _userInfoService.RegisterAsync(registerInput);
            }
            finally
            {
                CacheManager.Remove(RedisKeyPrefix.VerifyCode + registerInput.CheckCodeKey);
            }

            return new ResponseDto();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInput"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> Login(AppLoginInput loginInput)
        {
            var codeInfo = CacheManager.Get<VerifyCode>(RedisKeyPrefix.VerifyCode + loginInput.CheckCodeKey);
            if (codeInfo == null || !codeInfo.Code.Equals(loginInput.CheckCode))
            {
                return new ResponseDto(false, "验证码不正确");
            }
            try
            {
                var data = await _userInfoService.LoginAsync(loginInput);

                return new ResponseDto(data);
            }
            finally
            {
                CacheManager.Remove(RedisKeyPrefix.VerifyCode + loginInput.CheckCodeKey);
            }
        }

        /// <summary>
        /// 每次打开app token自动续约一下
        /// </summary>
        /// <param name="autoLoginInput"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ResponseDto> AutoLogin(AutoLoginInput autoLoginInput)
        {
            var data = await _userInfoService.AutoLoginAsync(autoLoginInput);

            return new ResponseDto(data);
        }
    }
}
