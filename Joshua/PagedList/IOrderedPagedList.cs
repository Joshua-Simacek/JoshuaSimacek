using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joshua.PagedList
{
    public interface IOrderedPagedList<T> : IPagedList<T>
    {
        string CurrentSort { get; }
        Order CurrentOrder { get; }
    }
}
