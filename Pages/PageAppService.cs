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
using DMS.Mall.Store.Application;
using System.Web;
using Abp.Runtime.Caching;
using DMS.Caching;
using Abp.Auditing;
using Abp.UI;
using DMS.Mall.Activity.Application;
using DMS.Mall.CrmBuyers;

namespace DMS.Mall.Website.Application
{
    public class PageAppService : AppServiceBase, IPageAppService
    {
        private readonly IPageRepository _pageRepository;
        private readonly UserManager _userManager;
        private readonly IStoreAppService _storeAppService;
        private readonly ICacheManager _cacheManager;

        private readonly IActivityAppService _activityAppService;

        public ICacheService CacheService { get; set; }

        private readonly ICrmBuyerRepository _crmBuyerRepository;

        public PageAppService(
            IPageRepository pageRepository, UserManager userManager, IStoreAppService storeAppService, ICacheManager cacheManager
            , IActivityAppService activityAppService,
            ICrmBuyerRepository crmBuyerRepository
            )
        {
            _pageRepository = pageRepository;
            _userManager = userManager;
            _storeAppService = storeAppService;
            _cacheManager = cacheManager;
            _activityAppService = activityAppService;
        }

        #region 页面管理

        /// <summary>
        /// 根据查询条件获取页面分页列表
        [DisableAuditing]//不添加日志
        public PagedResultOutput<PageQueryDto> GetPageQuery(GetPageQueryInput input)
        {
			if (input.MaxResultCount <= 0)
            {
                input.MaxResultCount = SettingManager.GetSettingValue<int>(MySettingProvider.QuestionsDefaultPageSize);
            }

            var query = _pageRepository.GetAll()
                //TODO:根据传入的参数添加过滤条件
                .WhereIf(input.Status!=0, m => m.Status == input.Status)
                .WhereIf(input.PageType!=0, m => m.PageType==input.PageType)
                .WhereIf(input.StoreId != 0, m => m.StoreId == input.StoreId)
                  .WhereIf(!input.Keywords.IsNullOrWhiteSpace(), m => m.Name.Contains(input.Keywords)).OrderBy(input.Sorting);
			var totalCount = query.Count();
            var list = query.PageBy(input).ToList();
            List<PageQueryDto> Pagelist = new List<PageQueryDto>();
            foreach (PageQueryDto item in list.MapTo<List<PageQueryDto>>())
            {
                User user = _userManager.Users.FirstOrDefault(u => u.Id == item.CreatorUserId.Value);
                item.UserName = user.UserName;
                if (item.StoreId != null && item.StoreId != "0")
                {
                    item.StoreId = _storeAppService.GetStores(Convert.ToInt32(item.StoreId)).Title;
                }
                item.PageTypeName = AppEnum.GetPageType(item.PageType);
                item.StatusName=AppEnum.GetPageStatus(item.Status);
                item.PagePositionName = AppEnum.GetPagePosition(item.PagePosition);
                Pagelist.Add(item);
            }
            return new PagedResultOutput<PageQueryDto>
            {
                TotalCount = totalCount,
                Items = Pagelist
            };

        }

        /// <summary>
        /// 获取指定id的页面信息
        [DisableAuditing]
        public async Task<PageDto> GetPage(int id)
        {
            var entity = await CacheService.GetCachedEntityAsync<Page>(id);
            return entity.MapTo<PageDto>();
        }
        [DisableAuditing]
        public List<PageDto> GetCachePageList()
        {
            return CacheService.GetCachedList<Page>().MapTo<List<PageDto>>();
        }
        [DisableAuditing]
        public List<PageDto> GetPageList()
        {
            var entity =  _pageRepository.GetAll();
            return entity.MapTo<List<PageDto>>();
        }
        /// <summary>
        /// 页面
        /// </summary>
        /// <param name="type">页面类型</param>
        [DisableAuditing]
        public  List<PageDto> GetPageTypeList(int type,int StoreId)
        {
            var entity = GetCachePageList().Where(p => p.PageType == type && p.StoreId == StoreId && p.Status == 2).OrderBy(p=>p.SortNum).ToList(); 
            return entity.MapTo<List<PageDto>>();
        }
        [DisableAuditing]
        public List<PageDto> GetPageList(int StoreId)
        {
            var entity = GetCachePageList().Where(p => p.StoreId == StoreId && p.Status == 2).OrderBy(p => p.SortNum).ToList();
            

            return entity.MapTo<List<PageDto>>();
        }
        /// <summary>
        /// 获取指定id的页面信息
        /// </summary>
        /// <param name="type">页面类型</param>
        [DisableAuditing]
        public List<PageDto> GetPageIdList(int id, int StoreId)
        {
            var entity = GetCachePageList().Where(p => p.Id==id&& p.StoreId == StoreId && p.Status == 2).OrderBy(p => p.SortNum).ToList();
            return entity.MapTo<List<PageDto>>();
        }
        /// <summary>
        /// 新增或更改页面
        /// </summary>
        public async Task CreateOrUpdatePage(PageUpdateDto input)
        {
         
            input.Remarks = HttpUtility.UrlDecode(input.Remarks);
            if (input.Id == 0)
            {
                var entity = await CreatePage(input);
            }
            else
            {
                await UpdatePage(input);
            }
        }

        /// <summary>
        /// 新增页面
        /// </summary>
        public virtual async Task<Page> CreatePage(PageUpdateDto input)
        {
            if (input.PageType != 5)
            {
                var PageList = _pageRepository.GetAll().Where(p => p.PageType == input.PageType && p.StoreId == input.StoreId&&p.PagePosition==input.PagePosition).ToList();
                foreach (var item in PageList)
                {
                    if (item.Status == 2 && input.Status == 2)
                        throw new UserFriendlyException("该页面已有发布页面！");
                }
            }
            var entity = await _pageRepository.InsertAsync(input.MapTo<Page>());
           // CacheService.Set(input.Id, input.MapTo(entity));
            return entity;
        }

        /// <summary>
        /// 更新页面
        /// </summary>
        public virtual async Task UpdatePage(PageUpdateDto input)
        {
            if (input.PageType != 5)
            {
                var PageList = _pageRepository.GetAll().Where(p => p.PageType == input.PageType && p.StoreId == input.StoreId && p.PagePosition == input.PagePosition).ToList();
                foreach (var item in PageList)
                {
                    if (item.Status == 2 && item.Id != input.Id && input.Status == 2)
                        throw new UserFriendlyException("该页面已有发布页面！");
                }
            }
            var entity = await _pageRepository.GetAsync(input.Id);
            
            await _pageRepository.UpdateAsync(input.MapTo(entity));

            //await CurrentUnitOfWork.SaveChangesAsync();
            //entity = await _pageRepository.GetAsync(input.Id);

            CacheService.Set(input.Id, input.MapTo(entity));
        }

        /// <summary>
        /// 删除页面
        /// </summary>
        public async Task DeletePage(PageDto  input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _pageRepository.DeleteAsync(input.Id);
          //  CacheService.Remove<int, Page>(input.Id);
        }
        public void SetPageCache(string type)
        {
            CacheService.Set("", _pageRepository.GetAll().ToList(), type);
        }

        public void SetRecommendCache(string type)
        {
            CacheService.Set("", GetPageElementDtos(), "GetIndexRecommend");

           // CacheService.Set("", new List<PageChildElementDto>(), "GetIndexRecommend");
            //  var childListNew = _cacheManager.GetCache("GetIndexRecommend").Get("", c =>
            //{
            //    return GetPageElementDtos();
            //}).MapTo<List<PageChildElementDto>>();
        }

        #endregion

        #region 前端需要用的api

        /// <summary>
        /// 获取店铺首页的内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]//不添加日志
        public PageDto GetPageForIndex(GetPageQueryInput input)
        {
            var page = this.GetCachePageList();

            page = page.Where(p => p.PageType == (int)AppEnum.PageType.HomePage && p.StoreId == Convert.ToInt32(input.StoreId) && p.PagePosition == 2 && p.Status == (int)AppEnum.PageStatus.Show).OrderBy(p => p.SortNum).ToList();

            if (page.Count <= 0)
                throw new UserFriendlyException("");

            return page[0];

        }

        /// <summary>
        /// 获取所有首页推荐位数据
        /// </summary>
        /// <returns></returns>
        public List<PageChildElementDto> GetPageElementDtos()
        {

            //GetIndexRecommend
           // var pageChildElementDtos = CacheService.GetCachedList<            
            //List<PageChildElementDto>>().MapTo<List<PageChildElementDto>>();

            //var pageChildElementDtos = _cacheManager.GetCache("GetIndexRecommend").Get().MapTo<List<PageChildElementDto>>();
                //.To<PageChildElementDto>();
                
                //as List<PageChildElementDto>;
               // .MapTo<List<PageChildElementDto>>().ToList();

            //if(pageChildElementDtos!=null)
            //{
            //    return pageChildElementDtos;
            //}

            var page = this.GetCachePageList();
            page = page.Where(p => p.PageType == (int)AppEnum.PageType.HomePage && p.PagePosition == 2 && p.Status == (int)AppEnum.PageStatus.Show).OrderBy(p => p.SortNum).ToList();

            if (page.Count <= 0)
                throw new UserFriendlyException("");

            List<PageChildElementDto> childList = new List<PageChildElementDto>();

            //找出推荐位对应商品基本价格

            List<int> productList = new List<int>();

            foreach (var pageItem in page)
            {
                if (pageItem.PageElementItem != null)
                {
                    foreach (var item in pageItem.PageElementItem)
                    {
                        if (item.ElementType == (int)AppEnum.ElementType.Recommend)
                        {
                            if (item.PageChildElementItem != null)
                            {
                                foreach (var iit in item.PageChildElementItem)
                                {
                                    if (iit.ProductType == (int)AppEnum.TradeItemType.Activity && iit.ProductId != 0)
                                    {
                                        productList.Add(iit.ProductId.Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }


            if (productList.Count() > 0)
            {
                var acList = _activityAppService.GetActivityList(productList);

                foreach (var pageItem in page)
                {

                    if (pageItem.PageElementItem != null)
                    {
                        foreach (var item in pageItem.PageElementItem)
                        {
                            if (item.ElementType == (int)AppEnum.ElementType.Recommend)
                            {
                                if (item.PageChildElementItem != null)
                                {
                                    foreach (var iit in item.PageChildElementItem)
                                    {
                                        if (iit.ProductType == (int)AppEnum.TradeItemType.Activity && iit.ProductId != 0)
                                        {
                                            var acDto = acList.Find(c => c.Id == iit.ProductId);
                                            if (acDto != null)
                                            {
                                                iit.Price = acDto.Price;
                                                iit.OriginalPrice = acDto.OriginalPrice;
                                                iit.Count = acDto.BuyedQty;
                                                iit.ActivityType = acDto.ActivityType;
                                                iit.AreaId3 = acDto.AreaId3;
                                                iit.AreaId2 = acDto.AreaId2;
                                                iit.KeyWord = acDto.KeyWord;
                                                iit.GiftLevel = acDto.GiftLevel;
                                                iit.StoreId = acDto.StoreId;
                                                childList.Add(iit);
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }

            CacheService.Set("", childList.ToList(), "GetIndexRecommend");

            return childList;

        }

        /// <summary>
        /// 获取所有店铺首页的内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]//不添加日志
        public PagedResultOutput<PageChildElementDto> GetPageForIndexRecommend(GetRecommendQueryInput input)
        {
            List<PageChildElementDto> childList = null;
            var childListNew=_cacheManager.GetCache("GetIndexRecommend").Get("", c =>
            {
                return GetPageElementDtos();
            }).MapTo<List<PageChildElementDto>>();

            
            if (input.MaxResultCount <= 0)
            {
                input.MaxResultCount = SettingManager.GetSettingValue<int>(MySettingProvider.QuestionsDefaultPageSize);
            }
            if (input.StoreId == 4 || input.StoreId == 0)
            {
                childList = childListNew.Where(c => c.ActivityType == 1 || (c.ActivityType == 0 && ((!string.IsNullOrEmpty(c.AreaId2) && input.CityId.ToLower() == c.AreaId2.ToLower()) || input.CityId.ToLower() == c.AreaId3.ToLower()))).ToList();
            }
            else
            {
                childList = childListNew.Where(c => c.StoreId == input.StoreId).ToList();
            }
            var totalCount = childList.Count();
            var list = childList.ToList();
            List<PageChildElementDto> articleList = new List<PageChildElementDto>();
            articleList = childList.MapTo<List<PageChildElementDto>>().Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            return new PagedResultOutput<PageChildElementDto>
            {
                TotalCount = totalCount,
                Items = articleList
            };

        }


        #endregion

    }
}
