using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Avalon.Framework.Querys
{
    public abstract class QueryView
    {
        QueryViewDefine viewDefine;

        public QueryViewDefine From<TEntity>(Expression<Func<TEntity>> alias)
        {
            viewDefine = new QueryViewDefine();
            viewDefine.Metadata = new QueryViewMetadata(this.GetType(), typeof(TEntity), alias);
            QueryMetadataProvider.Register(viewDefine.Metadata);
            return viewDefine;
        }

        public QueryViewEntityDefine<TEntity> Define<TEntity>()
        {
            if (viewDefine == null)
                throw new QueryDefineException("视图 {0} 尚未定义, 请使用 QueryView.From 进行定义。", this.GetType().FullName);

            var define = new QueryViewEntityDefine<TEntity>(viewDefine.Metadata);
            QueryMetadataProvider.Register(define.Metadata);
            return define;
        }
    }
}
