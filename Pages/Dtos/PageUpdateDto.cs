using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Mall.Website.Application
{
    /// <summary>
    /// 页面
    /// </summary>
    [AutoMap(typeof(Page))]
    public  class PageUpdateDto : EntityDto, IValidate
    { /// <summary>
      /// 页面名称
      /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 页面类型(1,登录页2,首页3,详情页4,列表页5,自定义页)
        /// </summary>
        public int PageType { get; set; }

        /// <summary>
        /// 页面位置(1,电脑端页面2,移动端页面)
        /// </summary>
        public int PagePosition { get; set; }

        /// <summary>
        /// 页面地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 状态(-1,作废1,初始2,已发布)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 店铺Id
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// 预览地址
        /// </summary>
        public  string PreviewUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortNum { get; set; }
        
    }
}
