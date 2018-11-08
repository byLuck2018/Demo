

















using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace DMS.Mall.Website.Application
{
    public interface IPageElementAppService : IApplicationService
    {
        #region 页面元素管理

        /// <summary>
        /// 根据查询条件获取页面元素分页列表
        /// </summary>
        PagedResultOutput<PageElementQueryDto> GetPageElementQuery(GetPageElementQueryInput input);


        /// <summary>
        /// 获取指定id的页面元素信息
        /// </summary>
        Task<PageElementDto> GetPageElement(int id);
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">元素类型</param>
        /// <returns></returns>
        List<PageElementDto> GetPageElementSYList(int id,int type);
        /// <summary>
        /// 新增或更改页面元素
        /// </summary>
        Task CreateOrUpdatePageElement(PageElementUpdateDto input);

        /// <summary>
        /// 新增页面元素
        /// </summary>
        Task<PageElementDto> CreatePageElement(PageElementUpdateDto input);

        /// <summary>
        /// 更新页面元素
        /// </summary>
        Task UpdatePageElement(PageElementUpdateDto input);

        Task DeletePageElement(PageElementDto input);
        Task UpdateSortPageElement(string strSort);

        #endregion

    }
}
