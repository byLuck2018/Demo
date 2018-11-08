using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DMS.Mall.Website.Application
{
    [AutoMapFrom(typeof(PageChildElement))]
    public class PageChildElementQueryDto : EntityDto
    {
        /// <summary>
        /// 页面元素Id
        /// </summary>
        public int PageElementId { get; set; }
        /// <summary>
        /// 页面子元素名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string LinkUrl { get; set; }

        public int ProductId
        {
            get;
            set;
        }
        public string ProductName
        {
            get;
            set;
        }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortNum { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
       
        public long? CreatorUserId { get; set; }
        public string UserName { get; set; }
        public string Icon { get; set; }
    }
}
