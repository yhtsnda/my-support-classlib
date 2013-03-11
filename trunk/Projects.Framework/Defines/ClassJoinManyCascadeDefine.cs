using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Projects.Framework
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
                var pa = PropertyAccessorFactory.GetPropertyAccess(typeof(TEntity));

                processor.KeyFunc = (entity) => PropertyAccessorFactory.GetId(entity);
                processor.JoinKeyFunc = (join) => PropertyAccessorFactory.GetId(join);
                processor.JoinFunc = (join) => (IList<TJoin>)pa.GetGetter(joinMetadata.JoinName).Get(join);
                processor.SourceJoinFunc = (entity) => (IList<TJoin>)joinMetadata.DataProcesser.Process(entity);
                inited = true;
            }
            return processor;
        }

        public ClassJoinManyCascadeDefine<TEntity, TJoin> ForeignKey(Expression<Func<TJoin, object>> memberExpression)
        {
            var pa = PropertyAccessorFactory.GetPropertyAccess(typeof(TJoin));
            var member = ReflectionHelper.GetProperty(memberExpression.Body);
            processor.ForeignKeyAction = (join, value) => pa.GetSetter(member.Name).Set(join, value);
            return this;
        }

        public ClassJoinManyCascadeDefine<TEntity, TJoin> SortBy(Expression<Func<TJoin, object>> memberExpression)
        {
            var pa = PropertyAccessorFactory.GetPropertyAccess(typeof(TJoin));
            var member = ReflectionHelper.GetProperty(memberExpression.Body);
            processor.JoinSortAction = (join, value) => pa.GetSetter(member.Name).Set(join, value);
            return this;
        }
    }
}
