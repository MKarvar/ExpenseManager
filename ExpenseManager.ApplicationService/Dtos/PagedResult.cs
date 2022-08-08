using System;
using System.Collections.Generic;

namespace ExpenseManager.ApplicationService.Dtos
{
    public class PagedResult<TEntity> where TEntity : class
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }
        public IEnumerable<TEntity> DataList { get; set; }

        public PagedResult(int pageSize, int pageNumber)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
        public PagedResult(int pageSize, int pageNumber, IEnumerable<TEntity> dataList)
            : this( pageSize, pageNumber)
        {
            DataList = dataList;
        }

        public PagedResult(int pageSize, int pageNumber, IEnumerable<TEntity> dataList, int totalRecords)
            : this(pageSize, pageNumber, dataList)
        {
            TotalRecords = totalRecords;
        }
    }
}
