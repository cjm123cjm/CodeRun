using CodeRun.Services.Domain.CustomerException;
using CodeRun.Services.Domain.Entities.App;
using CodeRun.Services.Domain.IRepository.App;
using CodeRun.Services.Domain.UnitOfWork;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos.Inputs.Web;
using CodeRun.Services.IService.Dtos.Outputs.App;
using CodeRun.Services.IService.Interfaces.App;
using CodeRun.Services.IService.Interfaces.Web;
using CodeRun.Services.Service.Implements.Web;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRun.Services.Service.Implements.App
{
    public class AppUserInfoService : ServiceBase, IAppUserInfoService
    {
        private readonly IAppUserInfoRepository _userInfoRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public AppUserInfoService(
            IAppUserInfoRepository userInfoRepository,
            IUnitOfWork unitOfWork,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userInfoRepository = userInfoRepository;
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        /// <summary>
        /// 加载用户列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PageDto<AppUserInfoDto>> LoadAppUserInfoListAsync(AppUserInfoQueryInput queryInput)
        {
            var query = _userInfoRepository.Query().AsNoTracking();
            if (queryInput.JoinTimeStart != null)
            {
                query = query.Where(t => t.JoinTime >= queryInput.JoinTimeStart.Value);
            }
            if (queryInput.JoinTimeEnd != null)
            {
                query = query.Where(t => t.JoinTime <= queryInput.JoinTimeEnd.Value);
            }
            if (!string.IsNullOrWhiteSpace(queryInput.Email))
            {
                query = query.Where(t => t.Email.Contains(queryInput.Email));
            }
            if (!string.IsNullOrWhiteSpace(queryInput.DeviceId))
            {
                query = query.Where(t => t.LastUseDeviceId.Contains(queryInput.DeviceId));
            }

            var totalCount = await query.CountAsync();

            var data = await query.OrderByDescending(t => t.JoinTime).Skip((queryInput.PageIndex - 1) * queryInput.PageSize).Take(queryInput.PageSize)
                .ToListAsync();

            return new PageDto<AppUserInfoDto>
            {
                TotalCount = totalCount,
                Data = ObjectMapper.Map<List<AppUserInfoDto>>(data),
                PageIndex = queryInput.PageIndex,
                PageSize = queryInput.PageSize
            };
        }

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task UpdateStatusAppUserInfoAsync(UpdateStatusAppUserInput update)
        {
            var appUser = await _userInfoRepository.GetByIdAsync(update.AppUserId);
            if (appUser == null)
            {
                throw new BusinessException("数据不存在");
            }

            appUser.Status = update.Status;

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerInput"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task RegisterAsync(AppRegisterInput registerInput)
        {
            //判断邮箱是否存在
            int emailCount = await _userInfoRepository.QueryWhere(t => t.Email == registerInput.Email).CountAsync();
            if (emailCount != 0)
            {
                throw new BusinessException("邮箱已存在");
            }
            //昵称是否存在
            int nickNameCount = await _userInfoRepository.QueryWhere(t => t.NickName == registerInput.NickName).CountAsync();
            if (nickNameCount != 0)
            {
                throw new BusinessException("昵称已存在");
            }
            AppUserInfo appUserInfo = new AppUserInfo()
            {
                UserId = SnowIdWorker.NextId(),
                Email = registerInput.Email,
                NickName = registerInput.NickName,
                Password = registerInput.Password,
                Sex = registerInput.Sex,
                JoinTime = DateTime.Now,
                LastLoginTime = DateTime.Now,
                LastUseDeviceId = registerInput.DeviceId,
                LastUseDeviceBrand = registerInput.DeviceBrand,
                LastLoginIp = UserIp
            };

            await _userInfoRepository.AddAsync(appUserInfo);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInput"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<AppLoginDto> LoginAsync(AppLoginInput loginInput)
        {
            var appUser = await _userInfoRepository.QueryWhere(t => t.Email == loginInput.Email && t.Password == loginInput.Password).FirstOrDefaultAsync();
            if (appUser == null)
            {
                throw new BusinessException("账户密码错误");
            }
            if (appUser.Status == 0)
            {
                throw new BusinessException("账户已禁用");
            }

            appUser.LastLoginTime = DateTime.Now;
            appUser.LastUseDeviceId = loginInput.DeviceId;
            appUser.LastUseDeviceBrand = loginInput.DeviceBrand;
            appUser.LastLoginIp = UserIp;

            AppLoginDto appLoginDto = new AppLoginDto
            {
                UserId = appUser.UserId,
                NickName = appUser.NickName,
                Email = appUser.Email,
            };

            _userInfoRepository.Update(appUser);

            //生成token
            string token = _jwtTokenGenerator.AppGenerateToken(appLoginDto);
            appLoginDto.Token = token;

            await _unitOfWork.SaveChangesAsync();

            return appLoginDto;
        }

        /// <summary>
        /// 自动登录
        /// </summary>
        /// <param name="loginInput"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<AppLoginDto> AutoLoginAsync(AutoLoginInput loginInput)
        {
            AppLoginDto appLoginDto = new AppLoginDto
            {
                UserId = LoginUserId,
                NickName = LoginUserName,
                Email = Email,
            };

            var appUser = await _userInfoRepository.GetByIdAsync(LoginUserId);
            if (appUser == null)
            {
                throw new BusinessException("账户密码错误");
            }
            if (appUser.Status == 0)
            {
                throw new BusinessException("账户已禁用");
            }
            appUser.LastLoginTime = DateTime.Now;
            appUser.LastUseDeviceId = loginInput.DeviceId;
            appUser.LastUseDeviceBrand = loginInput.DeviceBrand;
            appUser.LastLoginIp = UserIp;

            _userInfoRepository.Update(appUser);

            //生成token
            string token = _jwtTokenGenerator.AppGenerateToken(appLoginDto);
            appLoginDto.Token = token;

            await _unitOfWork.SaveChangesAsync();

            return appLoginDto;
        }
    }
}
