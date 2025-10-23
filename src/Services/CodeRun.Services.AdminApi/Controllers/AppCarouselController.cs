using CodeRun.Services.AdminApi.CustomerPolicy;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Enums;
using CodeRun.Services.IService.Interfaces.App;
using Microsoft.AspNetCore.Mvc;

namespace CodeRun.Services.AdminApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppCarouselController : ControllerBase
    {
        private readonly IAppCarouselService _appCarouselService;

        public AppCarouselController(IAppCarouselService appCarouselService)
        {
            _appCarouselService = appCarouselService;
        }

        /// <summary>
        /// 加载轮播图数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize(PermissionCodeEnum.app_carousel_list)]
        public async Task<ResponseDto> LoadAppCarouselList()
        {
            var data = await _appCarouselService.LoadAppCarouselListAsync();

            return new ResponseDto(data);
        }

        /// <summary>
        /// 添加/修改 轮播图
        /// </summary>
        /// <param name="appCarousel"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.app_carousel_edit)]
        public async Task<ResponseDto> SaveAppCarousel(AppCarouselAddOrUpdateInput appCarousel)
        {
            await _appCarouselService.SaveAppCarouselAsync(appCarousel);

            return new ResponseDto();
        }

        /// <summary>
        /// 删除轮播图
        /// </summary>
        /// <param name="carouselId"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.app_carousel_edit)]
        public async Task<ResponseDto> DeletedAppCarousel(long carouselId)
        {
            await _appCarouselService.DeletedAppCarouselAsync(carouselId);

            return new ResponseDto();
        }

        /// <summary>
        /// 修改排序(上移下移)
        /// </summary>
        /// <param name="carouselIds"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.app_carousel_edit)]
        public async Task<ResponseDto> ChangeAppCarouselSort(string carouselIds)
        {
            await _appCarouselService.ChangeAppCarouselSortAsync(carouselIds);

            return new ResponseDto();
        }
    }
}
