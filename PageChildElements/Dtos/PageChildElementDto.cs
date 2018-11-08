using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using DMS.Mall.Product;

namespace DMS.Mall.Website.Application
{
    /// <summary>
    /// 页面元素
    /// </summary>
    [AutoMap(typeof(PageChildElement))]
    public class PageChildElementDto : EntityDto, IValidate
    {
        /// <summary>
        /// 页面元素Id
        /// </summary>
        [Required]
        public int PageElementId { get; set; }

        /// <summary>
        /// 子元素名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public int? ProductId { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortNum { get; set; }



        /// <summary>
        /// 1:活动订单 2:音频订单 
        /// </summary>
        public  int ProductType
        {
            get;
            set;
        }


        #region 临时字段

        public decimal Price { get; set; }
       
        public int Count { get; set; }

        public decimal OriginalPrice { get; set; }

        public string AreaId3 { get; set; }


        public string AreaId2 { get; set; }


        public string AreaId1 { get; set; }

        /// <summary>
        /// 0 :线下活动 1:线上活动
        /// </summary>
        public int ActivityType { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 礼品
        /// </summary>
        public int GiftLevel { get; set; }

        /// <summary>
        /// 商铺Id
        /// </summary>
        public int StoreId { get; set; }

        #endregion

    }

    public class EvaluateEntity
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Pic { get; set; }
        public string Content { get; set; }
        public decimal Price { get; set; }
    }
}
