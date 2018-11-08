using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DMS.Mall.Website.Application
{
    [AutoMapFrom(typeof(Page))]
    public class PageQueryDto : EntityDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 页面类型(1,登录页2,首页3,详情页4,列表页5,自定义页)
        /// </summary>
        public int PageType { get; set; }
        public string PageTypeName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 页面位置(1,电脑端页面2,移动端页面)
        /// </summary>
        public int PagePosition { get; set; }
        public string PagePositionName { get; set; }

        /// <summary>
        /// 状态(-1,作废1,初始2,已发布)
        /// </summary>
        public int Status { get; set; }
        public string StatusName { get; set; }

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

        /// <summary>
        /// 店铺
        /// </summary>
        public virtual string StoreId
        {
            get;
            set;
        }
        /// <summary>
        /// 预览地址
        /// </summary>
        public string PreviewUrl
        {
            get;
            set;
        }
    }
}
