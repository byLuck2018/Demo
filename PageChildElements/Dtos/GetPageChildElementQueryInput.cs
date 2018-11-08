
















using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;


using DMS.Mall.Website;

namespace DMS.Mall.Website.Application
{
    public class GetPageChildElementQueryInput  : IInputDto, IPagedResultRequest, ISortedResultRequest, ICustomValidate
    {
        //DOTO:在这里增加查询参数
		[Range(0, 1000)]
        public int MaxResultCount { get; set; }

        public int SkipCount { get; set; }
        public int PageElementsId { get; set; }

        public string Sorting { get; set; }

        public string Keywords { get; set; }

        public void AddValidationErrors(List<ValidationResult> results)
        {
            var validSortingValues = new[] { "SortNum", "VoteCount DESC", "ViewCount DESC", "AnswerCount DESC" };

            if (!Sorting.IsIn(validSortingValues))
            {
                results.Add(new ValidationResult("Sorting is not valid. Valid values: " + string.Join(", ", validSortingValues)));
            }
        }



        /// <summary>
        /// 页面元素分类
        /// </summary>
        //public Guid? PageChildElementCategoryId { get; set; }



    }
}
