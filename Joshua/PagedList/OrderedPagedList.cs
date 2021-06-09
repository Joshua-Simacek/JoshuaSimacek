using System.Collections.Generic;
using System.Linq;
using Joshua.Linq;

namespace Joshua.PagedList
{
    public class OrderedPagedList<T> : PagedList<T>, IOrderedPagedList<T>{

        public string CurrentSort { get; }
        public Order CurrentOrder { get; }

        public OrderedPagedList(IQueryable<T> source, string currentOrderBy = null, Order currentOrder = Order.Ascending, int? page = null, int pageSize = 10, int pagerRange = 10)
            :base(source.OrderBy(currentOrderBy, currentOrder), page, pageSize, pagerRange)
        {
            CurrentSort = currentOrderBy;
            CurrentOrder = currentOrder;
        }
    }
}
