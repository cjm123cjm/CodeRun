using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos.Outputs.App;

namespace CodeRun.Services.IService.Interfaces.App
{
    public interface IAppCarouselService
    {
        /// <summary>
        /// 加载轮播图数据
        /// </summary>
        /// <returns></returns>
        Task<List<AppCarouselDto>> LoadAppCarouselListAsync();

        /// <summary>
        /// 添加/修改轮播图
        /// </summary>
        /// <param name="appCarousel"></param>
        /// <returns></returns>
        Task SaveAppCarouselAsync(AppCarouselAddOrUpdateInput appCarousel);

        /// <summary>
        /// 删除轮播图
        /// </summary>
        /// <param name="carouselId"></param>
        /// <returns></returns>
        Task DeletedAppCarouselAsync(long carouselId);

        /// <summary>
        /// 修改排序(上移下移)
        /// </summary>
        /// <param name="carouselIds"></param>
        /// <returns></returns>
        Task ChangeAppCarouselSortAsync(string carouselIds);
    }
}
