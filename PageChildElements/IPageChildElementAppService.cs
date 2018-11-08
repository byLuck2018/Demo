

















using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace DMS.Mall.Website.Application
{
    public interface IPageChildElementAppService : IApplicationService
    {
        #region 页面元素管理

        /// <summary>
        /// 根据查询条件获取页面元素分页列表
        /// </summary>
         Task<PagedResultOutput<PageChildElementQueryDto>> GetPageChildElementQuery(GetPageChildElementQueryInput input);


        /// <summary>
        /// 获取指定id的页面元素信息
        /// </summary>
        Task<PageChildElementDto> GetPageChildElement(int id);
        /// <summary>
        /// 首页图片轮播
        /// </summary>
        /// <returns></returns>
        List<PageChildElementDto> GetPageChildElementSYLBList(int id);
        /// <summary>
        /// 首页广告图片
        /// </summary>
        /// <returns></returns>
        List<PageChildElementDto> GetPageChildElementSYGGList(int id);
        List<PageChildElementDto> GetPageChildElementList();
        /// <summary>
        /// 新增或更改页面元素
        /// </summary>
        Task CreateOrUpdatePageChildElement(PageChildElementDto input, string img);

        /// <summary>
        /// 新增页面元素
        /// </summary>
        Task<PageChildElementDto> CreatePageChildElement(PageChildElementDto input);

        /// <summary>
        /// 更新页面元素
        /// </summary>
        Task UpdatePageChildElement(PageChildElementDto input);

        Task DeletePageChildElement(PageChildElementDto input);
        Task UpdateSortPageChildElement(string strSort);

        #endregion

    }
}
