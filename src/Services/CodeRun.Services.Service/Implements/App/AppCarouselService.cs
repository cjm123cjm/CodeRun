using CodeRun.Services.Domain.CustomerException;
using CodeRun.Services.Domain.Entities.App;
using CodeRun.Services.Domain.IRepository.App;
using CodeRun.Services.Domain.UnitOfWork;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos.Outputs.App;
using CodeRun.Services.IService.Interfaces.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeRun.Services.Service.Implements.App
{
    public class AppCarouselService : ServiceBase, IAppCarouselService
    {
        private readonly IAppCarouselRepository _appCarouselRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AppCarouselService(
            IAppCarouselRepository appCarouselRepository,
            IUnitOfWork unitOfWork)
        {
            _appCarouselRepository = appCarouselRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 加载轮播数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<AppCarouselDto>> LoadAppCarouselListAsync()
        {
            var data = await _appCarouselRepository.Query().AsNoTracking().OrderBy(t => t.Sort).ToListAsync();

            return ObjectMapper.Map<List<AppCarouselDto>>(data);
        }

        /// <summary>
        ///  添加/修改轮播图
        /// </summary>
        /// <param name="appCarousel"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SaveAppCarouselAsync(AppCarouselAddOrUpdateInput appCarousel)
        {
            if (appCarousel.CarouselId == 0)
            {
                var carousel = ObjectMapper.Map<AppCarousel>(appCarousel);
                //排序
                carousel.Sort = (await _appCarouselRepository.MaxSortAsync()) + 1;

                await _appCarouselRepository.AddAsync(carousel);
            }
            else
            {
                var carousel = await _appCarouselRepository.GetByIdAsync(appCarousel.CarouselId);
                if (carousel == null)
                {
                    throw new BusinessException("数据不存在");
                }
                ObjectMapper.Map(appCarousel, carousel);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 删除轮播图
        /// </summary>
        /// <param name="carouselId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task DeletedAppCarouselAsync(long carouselId)
        {
            var carousel = await _appCarouselRepository.GetByIdAsync(carouselId);
            if (carousel == null)
            {
                throw new BusinessException("数据不存在");
            }

            _appCarouselRepository.Delete(carousel);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 修改排序(上移下移)
        /// </summary>
        /// <param name="carouselIds"></param>
        /// <returns></returns>
        public async Task ChangeAppCarouselSortAsync(string carouselIds)
        {
            var carouselLongId = carouselIds.Split(',').Select(t => Convert.ToInt64(t)).ToList();

            var carousels = await _appCarouselRepository.QueryWhere(t => carouselLongId.Contains(t.CarouselId), true).ToListAsync();

            int index = 1;
            foreach (var item in carousels)
            {
                item.Sort = index;
                index++;
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
