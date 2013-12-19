using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;

namespace Avalon.Framework.Querys
{
    public class ODataProcessor
    {
        public static object Process<TFilter, TResult>(HttpContextBase context, ODataQueryValidator validator = null, Func<TResult, object> converter = null, Func<IQuerySpecification<TFilter>, IQuerySpecification<TFilter>> specFilter = null)
        {
            var datas = new NameValueCollection(context.Request.QueryString);
            datas.Add(context.Request.Form);
            return Process<TFilter, TResult>(datas, validator, converter, specFilter);
        }

        public static object Process<TFilter, TResult>(NameValueCollection datas, ODataQueryValidator validator = null, Func<TResult, object> converter = null, Func<IQuerySpecification<TFilter>, IQuerySpecification<TFilter>> specFilter = null)
        {
            ODataQueryData queryData = ODataQueryData.Parse(datas);
            if (validator != null)
                validator.Valid(queryData);

            var vistor = new ODataExpressionVisitor(queryData, QueryMetadataProvider.SpecificationProvider);
            var spec = vistor.Process<TFilter>();

            if (specFilter != null)
                spec = specFilter(spec);

            if (queryData.Count)
                return spec.Count();

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
