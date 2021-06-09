using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joshua.PagedList
{
    public interface IPagedList<T> : IList<T>
    {
        int TotalCount { get; }
        int PageCount { get; }
        int PageSize { get; }
        int CurrentPage { get; }
        int StartPage { get; }
        int EndPage { get; }
        int PagerRange { get; }
    }
}