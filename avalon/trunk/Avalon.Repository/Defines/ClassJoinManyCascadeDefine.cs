using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Avalon.Framework
{
    public class ClassJoinManyCascadeDefine<TEntity, TJoin>
    {
        ClassJoinDefineMetadata joinMetadata;
        ClassJoinCascadeDefineMetadata cascadeMetadata;
        CascadeJoinManyProcessor<TEntity, TJoin> processor;
        bool inited;

        internal ClassJoinManyCascadeDefine(ClassJoinDefineMetadata joinMetadata)
        {
            this.joinMetadata = joinMetadata;
            this.cascadeMetadata = joinMetadata.JoinCascade;
            processor = new CascadeJoinManyProcessor<TEntity, TJoin>();
            cascadeMetadata.ProcessorHandler = GetProcessor;
        }

        IClassJoinCascadeProcessor GetProcessor()
        {
            if (!inited)
            {
                var ta = TypeAccessor.GetAccessor(typeof(TEntity));

                processor.KeyFunc = (entity) => EntityUtil.GetId(entity);
                processor.JoinKeyFunc = (join) => EntityUtil.GetId(join);
                processor.JoinFunc = (join) => (IList<TJoin>)ta.GetProperty(joinMetadata.JoinName, join);
                processor.SourceJoinFunc = (entity) => (IList<TJoin>)joinMetadata.DataProcesser.Process(entity);
                inited = true;
            }
            return processor;
        }

        public ClassJoinManyCascadeDefine<TEntity, TJoin> ForeignKey(Expression<Func<TJoin, object>> memberExpression)
        {
            var ta = TypeAccessor.GetAccessor(typeof(TJoin));

            var member = ReflectionHelper.GetProperty(memberExpression.Body);
            processor.ForeignKeyAction = (join, value) => ta.SetProperty(member.Name, join, value);
            return this;
        }

        public ClassJoinManyCascadeDefine<TEntity, TJoin> SortBy(Expression<Func<TJoin, object>> memberExpression)
        {
            var ta = TypeAccessor.GetAccessor(typeof(TJoin));

            var member = ReflectionHelper.GetProperty(memberExpression.Body);
            processor.JoinSortAction = (join, value) => ta.SetProperty(member.Name, join, value);
            return this;
        }
    }
}
