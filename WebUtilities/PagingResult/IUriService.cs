using ExpenseManager.ApplicationService.Queries;
using System;

namespace WebUtilities.PagingResult
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationQuery quey, string route);
    }
}
