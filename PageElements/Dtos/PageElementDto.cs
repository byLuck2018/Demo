using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System.Collections.Generic;

namespace DMS.Mall.Website.Application
{
    /// <summary>
    /// 页面元素
    /// </summary>
    [AutoMap(typeof(PageElement))]
    public class PageElementDto : EntityDto, IValidate
    {
        /// <summary>
        /// 页面Id
        /// </summary>
        [Required]
        public int PageId { get; set; }

        /// <summary>
        /// 元素名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 元素类型(1,图片轮播2,排行3,自定义4,推荐5,导航6,广告图片)
        /// </summary>
        public int ElementType { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 排行显示数量
        /// </summary>
        public int RankingNum { get; set; }

        /// <summary>
        /// 默认显示
        /// </summary>
        public int RankingDisplay { get; set; }

        /// <summary>
        /// 自定义标题
        /// </summary>
        public int CustomTitle { get; set; }

        /// <summary>
        /// 自定义内容
        /// </summary>
        public string CustomContent { get; set; }

        /// <summary>
        /// 状态（1,有效-1,无效）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 网站名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortNum { get; set; }

        public virtual List<PageChildElementDto> PageChildElementItem
        {
            get;
            set;
        }
    }
}
