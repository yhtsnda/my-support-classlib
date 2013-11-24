using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;

namespace Avalon.Framework.Querys
{
    public class ODataProcessor
    {
        public static IQuerySpecification<TFilter> Process<TFilter>(string queryString, ODataQueryValidator validator = null)
        {
            ODataQueryData queryData;
            return Process<TFilter>(queryString, out queryData, validator);
        }

        public static IQuerySpecification<TFilter> Process<TFilter>(string queryString, out ODataQueryData queryData, ODataQueryValidator validator = null)
        {
            queryData = ODataQueryData.Parse(queryString);
            if (validator != null)
                validator.Valid(queryData);

            var vistor = new ODataExpressionVisitor(queryData, QueryMetadataProvider.SpecificationProvider);
            return vistor.Process<TFilter>();
        }

        public static object Process<TFilter, TResult>(HttpContextBase context, ODataQueryValidator validator = null, Func<TResult, object> converter = null)
        {
            return Process<TFilter, TResult>(context.Server.UrlDecode(context.Request.Url.Query.TrimStart('?')), validator, null, converter);
        }

        public static object Process<TFilter, TResult>(HttpContextBase context, ODataQueryValidator validator = null, Func<IQuerySpecification<TFilter>, IQuerySpecification<TFilter>> specFilter = null, Func<TResult, object> converter = null)
        {
            return Process<TFilter, TResult>(context.Server.UrlDecode(context.Request.Url.Query.TrimStart('?')), validator, specFilter, converter);
        }

        public static object Process<TFilter, TResult>(string queryString, ODataQueryValidator validator = null, Func<IQuerySpecification<TFilter>, IQuerySpecification<TFilter>> specFilter = null, Func<TResult, object> converter = null)
        {
            ODataQueryData queryData;
            var spec = Process<TFilter>(queryString, out queryData, validator);
            if (specFilter != null)
                spec = specFilter(spec);

            if (queryData.InlineCount)
            {
                var paging = spec.ToPaging<TResult>();
                if (converter != null)
                    return new PagingResult<object>(paging.TotalCount, paging.Items.Select(o => converter(o)));
                return paging;
            }

            var list = spec.ToList<TResult>();
            if (converter != null)
                return list.Select(o => converter(o)).ToList();
            return list;
        }
    }
}
