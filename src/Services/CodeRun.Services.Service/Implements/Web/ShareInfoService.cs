using AutoMapper.QueryableExtensions;
using CodeRun.Services.Domain.CustomerException;
using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.Domain.IRepository.Web;
using CodeRun.Services.Domain.UnitOfWork;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.Web;
using CodeRun.Services.IService.Dtos.Outputs.Web;
using CodeRun.Services.IService.Interfaces.Web;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CodeRun.Services.Service.Implements.Web
{
    public class ShareInfoService : ServiceBase, IShareInfoService
    {
        private readonly IShareInfoRepository _shareInfoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ShareInfoService(
            IShareInfoRepository shareInfoRepository,
            IUnitOfWork unitOfWork)
        {
            _shareInfoRepository = shareInfoRepository;
            _unitOfWork = unitOfWork;
        }

        private IQueryable<ShareInfo> SearchQuery(ShareInfoQueryInput queryInput)
        {
            var query = _shareInfoRepository.Query().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(queryInput.Title))
            {
                query = query.Where(t => t.Title.Contains(queryInput.Title));
            }
            if (queryInput.Status.HasValue)
            {
                query = query.Where(t => t.Status == queryInput.Status.Value);
            }
            if (!string.IsNullOrWhiteSpace(queryInput.CreatedUserName))
            {
                query = query.Where(t => t.CreatedUserName == queryInput.CreatedUserName);
            }
            if (queryInput.ShareIds != null && queryInput.ShareIds.Any())
            {
                query = query.Where(t => queryInput.ShareIds.Contains(t.ShareId));
            }
            query = query.OrderByDescending(t => t.ShareId);
            return query;
        }

        /// <summary>
        /// 不分页查询列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        public async Task<List<ShareInfoDto>> LoadShareWhereListAsync(ShareInfoQueryInput queryInput)
        {
            var query = SearchQuery(queryInput);

            var shareInfoDtos = await query.OrderByDescending(t => t.ShareId)
                                           .ProjectTo<ShareInfoDto>(ObjectMapper.ConfigurationProvider)
                                           .ToListAsync();

            return shareInfoDtos;
        }

        /// <summary>
        /// 分享列表查询
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        public async Task<PageDto<ShareInfoDto>> LoadShareInfoListAsync(ShareInfoQueryInput queryInput)
        {
            var query = SearchQuery(queryInput);

            var totalCount = await query.CountAsync();

            var shareInfoDtos = await query.Skip((queryInput.PageIndex - 1) * queryInput.PageSize)
                                           .Take(queryInput.PageSize)
                                           .ProjectTo<ShareInfoDto>(ObjectMapper.ConfigurationProvider)
                                           .ToListAsync();

            return new PageDto<ShareInfoDto>
            {
                TotalCount = totalCount,
                Data = shareInfoDtos,
                PageSize = queryInput.PageSize,
                PageIndex = queryInput.PageIndex,
            };
        }

        /// <summary>
        /// 根据id查询数据
        /// </summary>
        /// <param name="shareId"></param>
        /// <returns></returns>
        public async Task<ShareInfoAddOrUpdateInput> GetShareInfoByIdAsync(long shareId)
        {
            var share = await _shareInfoRepository.GetByIdAsync(shareId);

            return ObjectMapper.Map<ShareInfoAddOrUpdateInput>(share);
        }
        /// <summary>
        /// 添加/修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SaveShareInfoAsync(ShareInfoAddOrUpdateInput input)
        {
            if (input.ShareId == 0)
            {
                var shareInfo = ObjectMapper.Map<ShareInfo>(input);
                shareInfo.ShareId = SnowIdWorker.NextId();
                shareInfo.CreatedUserId = LoginUserId;
                shareInfo.CreatedUserName = LoginUserName;
                shareInfo.CreatedTime = DateTime.Now;

                await _shareInfoRepository.AddAsync(shareInfo);
            }
            else
            {
                var shareInfo = await _shareInfoRepository.GetByIdAsync(input.ShareId);
                if (shareInfo == null)
                {
                    throw new BusinessException("数据不存在");
                }
                if (!IsAdmin && shareInfo.CreatedUserId != LoginUserId)
                {
                    throw new BusinessException("无权限修改");
                }
                ObjectMapper.Map(input, shareInfo);

                _shareInfoRepository.Update(shareInfo);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="shareIds"></param>
        /// <returns></returns>
        public async Task DeletedShareInfoAsync(string shareIds)
        {
            var share = shareIds.Split(',').Select(t => Convert.ToInt64(t)).Distinct().ToList();

            var query = _shareInfoRepository.QueryWhere(t => share.Contains(t.ShareId), true);
            if (!IsAdmin)
            {
                query = query.Where(t => t.CreatedUserId == LoginUserId);
            }

            var delete = await query.ToListAsync();

            _shareInfoRepository.Delete(delete.ToArray());

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 发布/取消发布
        /// </summary>
        /// <param name="shareIds">,号拼接</param>
        /// <param name="status">0-取消发布 1-发布</param>
        /// <returns></returns>
        public async Task UpdateStatusShareInfoAsync(string shareIds, int status)
        {
            var share = shareIds.Split(',').Select(t => Convert.ToInt64(t)).Distinct().ToList();

            var query = _shareInfoRepository.QueryWhere(t => share.Contains(t.ShareId), true);
            if (!IsAdmin)
            {
                query = query.Where(t => t.CreatedUserId == LoginUserId);
            }

            var data = await query.ToListAsync();

            data.ForEach(t => t.Status = status);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 上一页/下一页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ShareInfoAddOrUpdateInput> ShowShareInfoDetailNextAsync(ShareInfoQueryInput input)
        {
            if (input.CurrentShareInfoId == null)
            {
                throw new BusinessException("参数错误");
            }
            var query = SearchQuery(input);

            ShareInfo? share = null;
            //上一页
            if (input.NextType == 1)
            {
                share = await query.Where(t => t.ShareId > input.CurrentShareInfoId.Value).LastOrDefaultAsync();
            }
            //下一页
            else if (input.NextType == 2)
            {
                share = await query.Where(t => t.ShareId < input.CurrentShareInfoId.Value).FirstOrDefaultAsync();
            }
            //当前页
            else if (input.NextType == 0)
            {
                share = await query.Where(t => t.ShareId == input.CurrentShareInfoId.Value).FirstOrDefaultAsync();
            }
            else
            {
                throw new BusinessException("参数错误");
            }

            if (share == null)
            {
                if (input.NextType == 1)
                    throw new BusinessException("已经是第一页了");
                else if (input.NextType == 2)
                    throw new BusinessException("已经是最后一页了");
                else if (input.NextType == 0)
                    throw new BusinessException("数据不存在");
                else
                    throw new BusinessException("参数不存在");
            }

            if (input.ReadCount)
            {
                share.ReadCount++;

                _shareInfoRepository.Update(share);

                await _unitOfWork.SaveChangesAsync();
            }

            return ObjectMapper.Map<ShareInfoAddOrUpdateInput>(share);
        }
    }
}
