using Castle.Core.Configuration;
using CodeRun.Services.Common;
using CodeRun.Services.Domain.CustomerException;
using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.Domain.IRepository.Web;
using CodeRun.Services.Domain.UnitOfWork;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.Web;
using CodeRun.Services.IService.Dtos.Outputs.Web;
using CodeRun.Services.IService.Interfaces.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CodeRun.Services.Service.Implements.Web
{
    public class AccountService : ServiceBase, IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleForMenuRepository _roleForMenuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public AccountService(
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            IJwtTokenGenerator jwtTokenGenerator,
            IRoleRepository roleRepository,
            IRoleForMenuRepository roleForMenuRepository,
            Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
            _roleRepository = roleRepository;
            _roleForMenuRepository = roleForMenuRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// 加载用户数据
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PageDto<AccountDto>> LoadAccountListAsync(AccountQueryInput queryInput)
        {
            var query = _accountRepository.Query().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(queryInput.UserName))
            {
                query = query.Where(t => t.UserName.Contains(queryInput.UserName));
            }
            if (!string.IsNullOrWhiteSpace(queryInput.Phone))
            {
                query = query.Where(t => t.Phone.Contains(queryInput.Phone));
            }

            PageDto<AccountDto> pageDto = new PageDto<AccountDto>();

            pageDto.TotalCount = await query.CountAsync();
            pageDto.PageIndex = queryInput.PageIndex;
            pageDto.PageSize = queryInput.PageSize;

            var accounts = await query.OrderByDescending(t => t.CreatedTime)
                                      .Skip((queryInput.PageIndex - 1) * queryInput.PageSize)
                                      .Take(queryInput.PageSize).ToListAsync();

            var accountDtos = ObjectMapper.Map<List<AccountDto>>(accounts);
            //查询角色名称
            foreach (var item in accountDtos)
            {
                if (item.Roles != null)
                {
                    var roleIds = item.Roles.Split(",");
                    var roleNames = await _roleRepository.QueryWhere(t => roleIds.Contains(t.RoleId.ToString())).Select(t => t.RoleName).ToListAsync();
                    item.RoleNames = string.Join(",", roleNames);
                }
            }

            pageDto.Data = accountDtos;
            return pageDto;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInput"></param>
        /// <returns></returns>
        public async Task<LoginDto> LoginAsync(LoginInput loginInput)
        {
            var user = await _accountRepository.QueryWhere(t => t.UserName == loginInput.UserName && t.Password == loginInput.Password).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new BusinessException("用户名密码错误");
            }

            if (user.Status == 0)
            {
                throw new BusinessException("账户已禁用");
            }

            var accountDto = ObjectMapper.Map<AccountDto>(user);

            var loginDto = new LoginDto { Account = accountDto };

            var roleIds = accountDto.Roles?.Split(",").Select(t => Convert.ToInt64(t)).ToList();
            if (roleIds == null)
            {
                throw new BusinessException("账户未分配角色,请联系管理员");
            }

            var menus = new List<Menu>();

            var superAdmin = _configuration.GetSection("SuperAdmin").Get<List<string>>();
            if (superAdmin != null && superAdmin.Contains(accountDto.Phone))
            {
                menus = await _roleForMenuRepository.GetMenusAsync();
                accountDto.IsAdmin = true;
            }
            else
            {
                //根据角色查询菜单
                menus = await _roleForMenuRepository.GetMenusByRoleIdAsync(roleIds.ToArray());
                accountDto.IsAdmin = false;
            }

            //获取权限编码
            loginDto.PermissionCodes = menus.Select(t => t.PermissionCode).ToList();

            //生成树形菜单列表
            var treeMenuDto = ObjectMapper.Map<List<MenuTreeDto>>(menus.Where(t => t.MenuType == 0).ToList());

            var menuTreeDtos = BuildTreeMenu(treeMenuDto, 0);

            //token
            loginDto.Token = _jwtTokenGenerator.GenerateToken(accountDto, loginDto.PermissionCodes);

            return loginDto;
        }

        /// <summary>
        /// 添加账户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task AddAccountAsync(AccountAddInput input)
        {
            //检查手机号是否存在
            var phoneCount = await _accountRepository.QueryWhere(t => t.Phone == input.Phone && t.UserId != input.UserId).CountAsync();
            if (phoneCount != 0)
            {
                throw new BusinessException("手机号已存在");
            }

            var account = ObjectMapper.Map<Account>(input);
            account.CreatedTime = DateTime.Now;
            account.UserId = SnowIdWorker.NextId();

            await _accountRepository.AddAsync(account);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 修改账户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateAccountAsync(AccountUpdateInput input)
        {
            //检查手机号是否存在
            var phoneCount = await _accountRepository.QueryWhere(t => t.Phone == input.Phone && t.UserId != input.UserId).CountAsync();
            if (phoneCount != 0)
            {
                throw new BusinessException("手机号已存在");
            }

            var account = await _accountRepository.GetByIdAsync(input.UserId);
            if (account == null)
            {
                throw new BusinessException("数据不存在");
            }

            ObjectMapper.Map(input, account);

            _accountRepository.Update(account);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdatePasswordAsync(UpdatePasswordInput input)
        {
            var account = await _accountRepository.GetByIdAsync(input.UserId);
            if (account == null)
            {
                throw new BusinessException("数据不存在");
            }

            if (account.Password != MD5Util.MD5Encrypt(input.OldPassword))
            {
                throw new BusinessException("旧密码不正确");
            }

            account.Password = MD5Util.MD5Encrypt(input.NewPassword);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 修改账户状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateAccountStatusAsync(UpdateAccountStatusInput input)
        {
            var account = await _accountRepository.GetByIdAsync(input.UserId);
            if (account == null)
            {
                throw new BusinessException("数据不存在");
            }

            account.Status = input.Status;

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task DeleteAccountAsync(long accountId)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null)
            {
                throw new BusinessException("数据不存在");
            }

            _accountRepository.Delete(account);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
