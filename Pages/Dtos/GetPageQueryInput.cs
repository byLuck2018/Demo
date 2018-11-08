
















using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;


using DMS.Mall.Website;

namespace DMS.Mall.Website.Application
{
    public class GetPageQueryInput  : IInputDto, IPagedResultRequest, ISortedResultRequest
    {
        //DOTO:在这里增加查询参数
		[Range(0, 1000)]
        public int MaxResultCount { get; set; }

        public int SkipCount { get; set; }
        public int PageType { get; set; }
        public int StoreId { get; set; }
        public int Status { get; set; }

        public string Sorting { get; set; }

        public string Keywords { get; set; }

        //public void AddValidationErrors(List<ValidationResult> results)
        //{
        //    var validSortingValues = new[] { "CreationTime DESC", "VoteCount DESC", "ViewCount DESC", "AnswerCount DESC" };

        //    if (!Sorting.IsIn(validSortingValues))
        //    {
        //        results.Add(new ValidationResult("Sorting is not valid. Valid values: " + string.Join(", ", validSortingValues)));
        //    }
        //}



        /// <summary>
        /// 页面分类
        /// </summary>
        //public Guid? PageCategoryId { get; set; }



    }


    public class GetRecommendQueryInput : IInputDto, IPagedResultRequest
    {
        //DOTO:在这里增加查询参数
        [Range(0, 1000)]
        public int MaxResultCount { get; set; }

        public int SkipCount { get; set; }

        public string Sorting { get; set; }

        public int Status { get; set; }

        public string CityId { get; set; }

        public string Token { get; set; }

        public int StoreId { get; set; }

    }
}
