using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Joshua.Web.Mvc.Html
{
    public static class PagerExtensions
    {
        public static MvcHtmlString PagerFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string actionName, string controllerName, object htmlAttributes = null)
        {
            var ulPager = new TagBuilder("ul");
            ulPager.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            var valueGetter = expression.Compile();
            var pager = valueGetter(helper.ViewData.Model) as Pager;

            var requestContext = helper.ViewContext.RequestContext;
            var routeCollection = helper.RouteCollection;

            if (pager != null)
            {
                if (pager.CurrentPage > 1)
                {
                    var first = HtmlHelper.GenerateLink(requestContext, routeCollection, HttpUtility.HtmlDecode("&laquo;"), null, actionName, controllerName, null, null);
                    var previous = HtmlHelper.GenerateLink(requestContext, routeCollection, HttpUtility.HtmlDecode("&lsaquo;"), null, actionName, controllerName, new RouteValueDictionary { { "page", (pager.CurrentPage - 1) } }, null);

                    var liFirst = new TagBuilder("li") { InnerHtml = first };
                    var liPrev = new TagBuilder("li") { InnerHtml = previous };

                    ulPager.InnerHtml += liFirst.ToString();
                    ulPager.InnerHtml += liPrev.ToString();
                }

                for (var page = pager.StartPage; page < (pager.EndPage + 1); page++)
                {
                    var liPage = new TagBuilder("li");
                    if (page == pager.CurrentPage) { liPage.AddCssClass("active"); }
                    var aPage = HtmlHelper.GenerateLink(requestContext, routeCollection, page.ToString(), null, actionName, controllerName, new RouteValueDictionary { { "page", page } }, null);
                    liPage.InnerHtml = aPage;
                    ulPager.InnerHtml += liPage.ToString();
                }

                if (pager.CurrentPage < pager.TotalPages)
                {
                    var next = HtmlHelper.GenerateLink(requestContext, routeCollection, HttpUtility.HtmlDecode("&rsaquo;"), null, actionName, controllerName, new RouteValueDictionary { { "page", (pager.CurrentPage + 1) } }, null);
                    var last = HtmlHelper.GenerateLink(requestContext, routeCollection, HttpUtility.HtmlDecode("&raquo;"), null, actionName, controllerName, new RouteValueDictionary { { "page", pager.TotalPages } }, null);

                    var liNext = new TagBuilder("li") { InnerHtml = next };
                    var liLast = new TagBuilder("li") { InnerHtml = last };

                    ulPager.InnerHtml += liNext.ToString();
                    ulPager.InnerHtml += liLast.ToString();
                }

                return new MvcHtmlString(ulPager.ToString());
            }

            return null;
        }
    }
}
