

















using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace DMS.Mall.Website.Application
{
    public interface IPageAppService : IApplicationService
    {
        #region 页面管理

        /// <summary>
        /// 根据查询条件获取页面分页列表
        /// </summary>
        PagedResultOutput<PageQueryDto> GetPageQuery(GetPageQueryInput input);


        /// <summary>
        /// 获取指定id的页面信息
        /// </summary>
        Task<PageDto> GetPage(int id);
        List<PageDto> GetCachePageList();
        List<PageDto> GetPageList();
        /// <summary>
        /// 页面
        /// </summary>
        /// <param name="type">页面类型</param>
        /// <returns></returns>
        List<PageDto> GetPageTypeList(int type, int StoreId);
        List<PageDto> GetPageList(int StoreId);
        List<PageDto> GetPageIdList(int id, int StoreId);
        /// <summary>
        /// 新增或更改页面
        /// </summary>
        Task CreateOrUpdatePage(PageUpdateDto input);

        /// <summary>
        /// 新增页面
        /// </summary>
        Task<Page> CreatePage(PageUpdateDto input);

        /// <summary>
        /// 更新页面
        /// </summary>
        Task UpdatePage(PageUpdateDto input);

        /// <summary>
        /// 删除页面
        /// </summary>
        Task DeletePage(PageDto input);
        void SetPageCache(string type);


        PageDto GetPageForIndex(GetPageQueryInput input);


        PagedResultOutput<PageChildElementDto> GetPageForIndexRecommend(GetRecommendQueryInput input);

        List<PageChildElementDto> GetPageElementDtos();

        void SetRecommendCache(string type);
        #endregion

    }
}
