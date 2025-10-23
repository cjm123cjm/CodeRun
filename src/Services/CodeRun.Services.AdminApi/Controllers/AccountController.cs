using CodeRun.Services.AdminApi.CustomerPolicy;
using CodeRun.Services.Common.Captcha;
using CodeRun.Services.Common.RedisUtil;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.Web;
using CodeRun.Services.IService.Enums;
using CodeRun.Services.IService.Interfaces.Web;
using Microsoft.AspNetCore.Mvc;

namespace CodeRun.Services.AdminApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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
        /// 登录
        /// </summary>
        /// <param name="loginInput"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> Login([FromBody] LoginInput loginInput)
        {
            try
            {
                var emailCode = CacheManager.Get<VerifyCode>(RedisKeyPrefix.VerifyCode + loginInput.CodeKey);
                if (emailCode == null || emailCode.Code != loginInput.Code)
                {
                    ResponseDto responseDto = new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "图片验证码不正确"
                    };
                    return responseDto;
                }

                var data = await _accountService.LoginAsync(loginInput);

                return new ResponseDto(data);
            }
            finally
            {
                //移除验证码
                CacheManager.Remove(RedisKeyPrefix.VerifyCode + loginInput.CodeKey);
            }
        }


        /// <summary>
        /// 加载用户数据
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize(PermissionCodeEnum.settings_account_list)]
        public async Task<ResponseDto> LoadAccountList(AccountQueryInput queryInput)
        {
            var data = await _accountService.LoadAccountListAsync(queryInput);

            return new ResponseDto(data);
        }

        /// <summary>
        /// 添加账户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.settings_account_edit)]
        public async Task<ResponseDto> AddAccount(AccountAddInput input)
        {
            await _accountService.AddAccountAsync(input);

            return new ResponseDto();
        }

        /// <summary>
        /// 修改账户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.settings_account_edit)]
        public async Task<ResponseDto> UpdateAccount(AccountUpdateInput input)
        {
            await _accountService.UpdateAccountAsync(input);

            return new ResponseDto();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.settings_account_update_password)]
        public async Task<ResponseDto> UpdatePassword(UpdatePasswordInput input)
        {
            await _accountService.UpdatePasswordAsync(input);

            return new ResponseDto();
        }

        /// <summary>
        /// 修改账户状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.settings_account_op_status)]
        public async Task<ResponseDto> UpdateAccountStatus(UpdateAccountStatusInput input)
        {
            await _accountService.UpdateAccountStatusAsync(input);

            return new ResponseDto();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [PermissionAuthorize(PermissionCodeEnum.settings_account_del)]
        public async Task<ResponseDto> DeleteAccountAsync(long accountId)
        {
            await _accountService.DeleteAccountAsync(accountId);

            return new ResponseDto();
        }
    }
}
