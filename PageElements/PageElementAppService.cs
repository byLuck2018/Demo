using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DMS.Configuration;
using DMS.Authorization.Users;
using System.Web;
using Abp.Runtime.Caching;
using Abp.Auditing;

namespace DMS.Mall.Website.Application
{
    public class PageElementAppService : AppServiceBase, IPageElementAppService
    {
        private readonly IPageElementRepository _pageElementRepository;
        private readonly UserManager _userManager;
        private readonly ICacheManager _cacheManager;

        public PageElementAppService(
            IPageElementRepository pageElementRepository, UserManager userManager, ICacheManager cacheManager
            )
        {
            _pageElementRepository = pageElementRepository;
            _userManager = userManager;
            _cacheManager = cacheManager;
        }

        #region 页面元素管理

        /// <summary>
        /// 根据查询条件获取页面元素分页列表
        [DisableAuditing]//不添加日志
        public PagedResultOutput<PageElementQueryDto> GetPageElementQuery(GetPageElementQueryInput input)
        {
			if (input.MaxResultCount <= 0)
            {
                input.MaxResultCount = SettingManager.GetSettingValue<int>(MySettingProvider.QuestionsDefaultPageSize);
            }

            var query = _pageElementRepository.GetAll()
                //TODO:根据传入的参数添加过滤条件
                //.WhereIf(input.PageElementCategoryId.HasValue, m => m.PageElementCategoryId == input.PageElementCategoryId)
                .WhereIf(input.ElementType != 0, m => m.ElementType == input.ElementType)
                .WhereIf(input.PagesId != 0, m => m.PageId == input.PagesId)
                  .WhereIf(!input.Keywords.IsNullOrWhiteSpace(), m => m.Title.Contains(input.Keywords)).OrderBy(input.Sorting);
			var totalCount = query.Count();
            var list = query.ToList();

            List<PageElementQueryDto> PageElementlist = new List<PageElementQueryDto>();
            foreach (PageElementQueryDto item in list.MapTo<List<PageElementQueryDto>>())
            {
                if (item.LastModifierUserId != null)
                {
                    User user = _userManager.Users.FirstOrDefault(u => u.Id == item.LastModifierUserId.Value);
                    item.UserName = user.UserName+"["+ item.LastModificationTime+"]";
                }
                item.StatusName = AppEnum.GetBiStatus(item.Status);
                item.ElementTypeName = AppEnum.GetElementType(item.ElementType);
                item.Icon = "<i class=\"fa fa-arrows\"></i>";
                PageElementlist.Add(item);
            }
            return new PagedResultOutput<PageElementQueryDto>
            {
                TotalCount = totalCount,
                Items = PageElementlist
            };
        }

        /// <summary>
        /// 获取指定id的页面元素信息
        [DisableAuditing]
        public async Task<PageElementDto> GetPageElement(int id)
        {
            var entity = await _pageElementRepository.GetAsync(id);
            return entity.MapTo<PageElementDto>();
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="id">页面ID</param>
        /// <param name="type">元素类型</param>
        [DisableAuditing]
        public List<PageElementDto> GetPageElementSYList(int id,int type)
        {
            var entity = _pageElementRepository.GetAll().Where(pe => pe.PageId == id && pe.ElementType == type && pe.Status == 1).OrderBy(p => p.SortNum).ToList();
            return entity.MapTo<List<PageElementDto>>();
        }
        /// <summary>
        /// 新增或更改页面元素
        /// </summary>
        public async Task CreateOrUpdatePageElement(PageElementUpdateDto input)
        {
            input.CustomContent=HttpUtility.UrlDecode(input.CustomContent);
            if (input.Id == 0)
            {
                await CreatePageElement(input);
            }
            else
            {
                await UpdatePageElement(input);
            }
        }

        /// <summary>
        /// 新增页面元素
        /// </summary>
        public virtual async Task<PageElementDto> CreatePageElement(PageElementUpdateDto input)
        {
            //if (await _pageElementRepository.IsExistsPageElementByName(input.CategoryName))
            //{
            //    throw new UserFriendlyException(L("NameIsExists"));
            //}
            var entity = await _pageElementRepository.InsertAsync(input.MapTo<PageElement>());
            return entity.MapTo<PageElementDto>();
        }

        /// <summary>
        /// 更新页面元素
        /// </summary>
        public virtual async Task UpdatePageElement(PageElementUpdateDto input)
        {
            //if (await _pageElementRepository.IsExistsPageElementByName(input.CategoryName, input.Id))
            //{
            //    throw new UserFriendlyException(L("NameIsExists"));
            //}
            var entity = await _pageElementRepository.GetAsync(input.Id);
            await _pageElementRepository.UpdateAsync(input.MapTo(entity));
        }

        /// <summary>
        /// 删除页面
        /// </summary>
        public async Task DeletePageElement(PageElementDto input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _pageElementRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 标签排序
        /// </summary>
        public async Task UpdateSortPageElement(string strSort)
        {
            string[] strSorts = strSort.Split(',');
            for (int i = 0; i < strSorts.Length - 1; i++)
            {
                var entity = await _pageElementRepository.GetAsync(Convert.ToInt32(strSorts[i]));
                entity.SortNum = i + 1;
                await _pageElementRepository.UpdateAsync(entity);
            }
        }
        #endregion

    }
}
