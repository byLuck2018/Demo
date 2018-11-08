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
using DMS.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFramework;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Newtonsoft.Json;
using DMS.Configuration;
using DMS.Mall.Website;
using DMS.Authorization.Users;
using DMS.Mall.Product.Application;
using Abp.Runtime.Caching;
using Abp.Auditing;
using System.Web;

namespace DMS.Mall.Website.Application
{
    public class PageChildElementAppService : AppServiceBase, IPageChildElementAppService
    {
        private readonly IPageChildElementRepository _pageChildElementRepository;
        private readonly UserManager _userManager;
        private readonly IProductAppService _productAppService;
        private readonly ICacheManager _cacheManager;

        public PageChildElementAppService(
            IPageChildElementRepository pageChildElementRepository, UserManager userManager, IProductAppService productAppService
            , ICacheManager cacheManager
            )
        {
            _pageChildElementRepository = pageChildElementRepository;
            _userManager = userManager;
            _productAppService = productAppService;
            _cacheManager = cacheManager;
        }

        #region 页面元素管理

        /// <summary>
        /// 根据查询条件获取页面元素分页列表
        [DisableAuditing]//不添加日志
        public async Task<PagedResultOutput<PageChildElementQueryDto>> GetPageChildElementQuery(GetPageChildElementQueryInput input)
        {
			if (input.MaxResultCount <= 0)
            {
                input.MaxResultCount = SettingManager.GetSettingValue<int>(MySettingProvider.QuestionsDefaultPageSize);
            }

            var query = _pageChildElementRepository.GetAll()
                //TODO:根据传入的参数添加过滤条件
                //.WhereIf(input.PageChildElementCategoryId.HasValue, m => m.PageChildElementCategoryId == input.PageChildElementCategoryId)
                .WhereIf(input.PageElementsId!=0, m => m.PageElementId==input.PageElementsId)
				  .WhereIf(!input.Keywords.IsNullOrWhiteSpace(), m => m.Name.Contains(input.Keywords)).OrderBy(input.Sorting);
			var totalCount = query.Count();
            var list = query.PageBy(input).ToList();
            List<PageChildElementQueryDto> PageElementlist = new List<PageChildElementQueryDto>();
            foreach (PageChildElementQueryDto item in list.MapTo<List<PageChildElementQueryDto>>())
            {
                User user = _userManager.Users.FirstOrDefault(u => u.Id == item.CreatorUserId.Value);
                item.UserName = user.UserName + "[" + item.CreationTime + "]" ;
                if (item.ProductId != null&& item.ProductId !=0)
                {
                    var product = await _productAppService.GetProduct(item.ProductId);
                    item.ProductName = product.Title;
                }
                item.Icon = "<i class=\"fa fa-arrows\"></i>";
                PageElementlist.Add(item);
            }
            return new PagedResultOutput<PageChildElementQueryDto>
            {
                TotalCount = totalCount,
                Items = PageElementlist
            };

        }

        /// <summary>
        /// 获取指定id的页面元素信息
        [DisableAuditing]
        public async Task<PageChildElementDto> GetPageChildElement(int id)
        {
            var entity = await _pageChildElementRepository.GetAsync(id);
            return entity.MapTo<PageChildElementDto>();
        }
        /// <summary>
        /// 首页图片轮播
        /// </summary>
        [DisableAuditing]
        public List<PageChildElementDto> GetPageChildElementSYLBList(int id)
        {
            var entity = _pageChildElementRepository.GetAll().Where(p => p.PageElementId == id).OrderBy(p => p.SortNum).ToList();
            return entity.MapTo<List<PageChildElementDto>>();
        }
        /// <summary>
        /// 首页广告图片
        /// </summary>
        [DisableAuditing]
        public List<PageChildElementDto> GetPageChildElementSYGGList(int id)
        {
            var entity = _pageChildElementRepository.GetAll().Where(p => p.PageElementId == id).OrderBy(p => p.SortNum).ToList();
            return entity.MapTo<List<PageChildElementDto>>();
        }
        [DisableAuditing]
        public List<PageChildElementDto> GetPageChildElementList()
        {
            var entity = _pageChildElementRepository.GetAll().ToList();
            return entity.MapTo<List<PageChildElementDto>>();
        }
        /// <summary>
        /// 新增或更改页面元素
        /// </summary>
        public async Task CreateOrUpdatePageChildElement(PageChildElementDto input,string img)
        {
            input.Url = HttpUtility.UrlDecode(img);
            if (input.Id == 0)
            {
                await CreatePageChildElement(input);
            }
            else
            {
                await UpdatePageChildElement(input);
            }
        }

        /// <summary>
        /// 新增页面元素
        /// </summary>
        public virtual async Task<PageChildElementDto> CreatePageChildElement(PageChildElementDto input)
        {
            //if (await _pageChildElementRepository.IsExistsPageChildElementByName(input.CategoryName))
            //{
            //    throw new UserFriendlyException(L("NameIsExists"));
            //}
            var entity = await _pageChildElementRepository.InsertAsync(input.MapTo<PageChildElement>());
            return entity.MapTo<PageChildElementDto>();
        }

        /// <summary>
        /// 更新页面元素
        /// </summary>
        public virtual async Task UpdatePageChildElement(PageChildElementDto input)
        {
            //if (await _pageChildElementRepository.IsExistsPageChildElementByName(input.CategoryName, input.Id))
            //{
            //    throw new UserFriendlyException(L("NameIsExists"));
            //}
            var entity = await _pageChildElementRepository.GetAsync(input.Id);
            var s= await _pageChildElementRepository.UpdateAsync(input.MapTo(entity));
        }

        /// <summary>
        /// 删除页面
        /// </summary>
        public async Task DeletePageChildElement(PageChildElementDto input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _pageChildElementRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 标签排序
        /// </summary>
        public async Task UpdateSortPageChildElement(string strSort)
        {
            string[] strSorts = strSort.Split(',');
            for (int i = 0; i < strSorts.Length - 1; i++)
            {
                var entity = await _pageChildElementRepository.GetAsync(Convert.ToInt32(strSorts[i]));
                entity.SortNum = i + 1;
                await _pageChildElementRepository.UpdateAsync(entity);
            }
        }
        #endregion

    }
}
