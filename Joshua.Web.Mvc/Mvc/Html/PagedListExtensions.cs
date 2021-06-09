using Joshua.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Joshua.Web.Mvc.Mvc.Html
{

    public static class PagedListExtensions
    {
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int? page, int pageSize = 10,
            int pagerRange = 10)
        {
            return new PagedList<T>(source, page, pageSize, pagerRange);
        }

        public static IOrderedPagedList<T> ToOrderedPagedList<T>(this IQueryable<T> source, string orderBy,
            Order sortOrder = Order.Ascending, int? page = null, int pageSize = 10,
            int pagerRange = 10)
        {
            return new OrderedPagedList<T>(source, orderBy, sortOrder, page, pageSize, pagerRange);
        }

        public static MvcHtmlString PagerForModel<T>(this HtmlHelper<IOrderedPagedList<T>> html, string actionName,
            string controllerName, object htmlAttributes = null)
        {
            var ulPager = new TagBuilder("ul");
            ulPager.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            var model = html.ViewData.Model;

            var requestContext = html.ViewContext.RequestContext;
            var routeCollection = html.RouteCollection;

            if (model != null)
            {
                if (model.CurrentPage > 1)
                {
                    var first = HtmlHelper.GenerateLink(requestContext, routeCollection,
                        HttpUtility.HtmlDecode("&laquo;"), null, actionName, controllerName,
                        new RouteValueDictionary
                        {
                            {"sortCol", model.CurrentSort},
                            {"sortOrder", model.CurrentOrder},
                            {"pageSize", model.PageSize}
                        }, null);

                    var previous = HtmlHelper.GenerateLink(requestContext, routeCollection,
                        HttpUtility.HtmlDecode("&lsaquo;"), null, actionName, controllerName,
                        new RouteValueDictionary
                        {
                            {"page", (model.CurrentPage - 1)},
                            {"sortCol", model.CurrentSort},
                            {"sortOrder", model.CurrentOrder},
                            {"pageSize", model.PageSize}
                        }, null);

                    var liFirst = new TagBuilder("li") { InnerHtml = first };
                    var liPrev = new TagBuilder("li") { InnerHtml = previous };

                    ulPager.InnerHtml += liFirst.ToString();
                    ulPager.InnerHtml += liPrev.ToString();
                }

                for (var page = model.StartPage; page < (model.EndPage + 1); page++)
                {
                    var liPage = new TagBuilder("li");
                    if (page == model.CurrentPage) { liPage.AddCssClass("active"); }
                    var aPage = HtmlHelper.GenerateLink(requestContext, routeCollection, page.ToString(), null,
                        actionName, controllerName,
                        new RouteValueDictionary
                        {
                            {"page", page},
                            {"sortCol", model.CurrentSort},
                            {"sortOrder", model.CurrentOrder},
                            {"pageSize", model.PageSize }
                        }, null);
                    liPage.InnerHtml = aPage;
                    ulPager.InnerHtml += liPage.ToString();
                }

                if (model.CurrentPage < model.PageCount)
                {
                    var next = HtmlHelper.GenerateLink(requestContext, routeCollection,
                        HttpUtility.HtmlDecode("&rsaquo;"), null, actionName, controllerName,
                        new RouteValueDictionary
                        {
                            {"page", (model.CurrentPage + 1)},
                            {"sortCol", model.CurrentSort},
                            {"sortOrder", model.CurrentOrder},
                            {"pageSize", model.PageSize }
                        }, null);
                    var last = HtmlHelper.GenerateLink(requestContext, routeCollection,
                        HttpUtility.HtmlDecode("&raquo;"), null, actionName, controllerName,
                        new RouteValueDictionary
                        {
                            {"page", model.PageCount},
                            {"sortCol", model.CurrentSort},
                            {"sortOrder", model.CurrentOrder},
                            {"pageSize", model.PageSize }
                        }, null);

                    var liNext = new TagBuilder("li") { InnerHtml = next };
                    var liLast = new TagBuilder("li") { InnerHtml = last };

                    ulPager.InnerHtml += liNext.ToString();
                    ulPager.InnerHtml += liLast.ToString();
                }

                return new MvcHtmlString(ulPager.ToString());
            }

            return null;
        }

        public static MvcHtmlString SortableHeader<T>(this HtmlHelper<IOrderedPagedList<T>> html, string headerText,
            string sortValue, Order sortDirection, string actionName, string controllerName,
            object htmlAttributes = null)
        {
            var requestContext = html.ViewContext.RequestContext;
            var routeCollection = html.RouteCollection;

            var model = html.ViewData.Model;

            if (model != null)
            {
                var triangle = (model.CurrentSort == sortValue)
                    ? ((sortDirection == Order.Ascending)
                        ? HttpUtility.HtmlDecode("&#9662;")
                        : HttpUtility.HtmlDecode("&#9652;"))
                    : "";
                var aTag = HtmlHelper.GenerateLink(requestContext, routeCollection, headerText + " " + triangle, null,
                    actionName, controllerName,
                    new RouteValueDictionary
                    {
                        {"page", model.CurrentPage},
                        {"sortCol", sortValue},
                        {"sortOrder", sortDirection},
                        {"pageSize", model.PageSize }
                    }, null);

                return new MvcHtmlString(aTag);
            }

            return null;
        }

        public static MvcHtmlString SortableHeaderFor<TModel, TProperty>(this HtmlHelper<IOrderedPagedList<TModel>> html,
            Expression<Func<TModel, TProperty>> expression, string actionName, string controllerName,
            object htmlAttributes = null)
        {
            var model = html.ViewData.Model;

            var requestContext = html.ViewContext.RequestContext;
            var routeCollection = html.RouteCollection;

            if (model != null)
            {
                var metaData = ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>());
                var displayName = metaData.DisplayName ?? metaData.PropertyName;
                var sortOrder = (model.CurrentSort == metaData.PropertyName && model.CurrentOrder == Order.Ascending)
                    ? Order.Descending
                    : Order.Ascending;
                var triangle = (model.CurrentSort == metaData.PropertyName)
                    ? ((model.CurrentOrder == Order.Ascending)
                        ? HttpUtility.HtmlDecode("&#9652;")
                        : HttpUtility.HtmlDecode("&#9662;"))
                    : "";
                var aTag = HtmlHelper.GenerateLink(requestContext, routeCollection, displayName + " " + triangle, null,
                    actionName, controllerName,
                    new RouteValueDictionary
                    {
                        {"page", model.CurrentPage},
                        {"sortCol", metaData.PropertyName},
                        {"sortOrder", sortOrder},
                        {"pageSize", model.PageSize }
                    }, null);

                return new MvcHtmlString(aTag);
            }

            return null;
        }
    }
}
