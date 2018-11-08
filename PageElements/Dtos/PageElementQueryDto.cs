using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;

namespace DMS.Mall.Website.Application
{
    [AutoMapFrom(typeof(PageElement))]
    public class PageElementQueryDto : FullAuditedEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        /// <summary>
        /// 页面Id
        /// </summary>
        public int PageId { get; set; }

        /// <summary>
        /// 元素类型(1,图片轮播2,排行3,自定义4,推荐5,导航6,广告图片)
        /// </summary>
        public int ElementType { get; set; }
        public string ElementTypeName { get; set; }

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
        /// 状态（1,有效-1,无效）
        /// </summary>
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string Icon { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortNum { get; set; }
        
        public string UserName { get; set; }
    }
}
