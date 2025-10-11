using CodeRun.Services.Domain.CustomerException;
using CodeRun.Services.Domain.IRepository;
using CodeRun.Services.Domain.UnitOfWork;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Dtos.Outputs;
using CodeRun.Services.IService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodeRun.Services.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(
            IAccountRepository accountRepository, 
            IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 加载用户数据
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PageDto<AccountDto>> LoadAccountList(AccountQueryInput queryInput)
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

            return pageDto;
        }

        public Task<LoginDto> LoginAsync(LoginInput loginInput)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加/修改账户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SaveAccountAsync(AccountAddOrUpdateInput input)
        {
            //检查手机号是否存在
            var phoneCount = await _accountRepository.QueryWhere(t => t.Phone == input.Phone && t.UserId != input.UserId).CountAsync();
            if (phoneCount != 0)
            {
                throw new BusinessException("手机号已存在");
            }

            if (input.UserId == 0)
            {

            }
            else
            {

            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
