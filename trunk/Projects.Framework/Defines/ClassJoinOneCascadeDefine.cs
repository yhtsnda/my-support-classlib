using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Projects.Framework
{
    public class ClassJoinOneCascadeDefine<TEntity, TJoin>
    {
        ClassJoinDefineMetadata joinMetadata;
        ClassJoinCascadeDefineMetadata cascadeMetadata;
        CascadeJoinOneProcessor<TEntity, TJoin> processor;
        bool inited;

        internal ClassJoinOneCascadeDefine(ClassJoinDefineMetadata joinMetadata)
        {
            this.joinMetadata = joinMetadata;
            this.cascadeMetadata = joinMetadata.JoinCascade;
            processor = new CascadeJoinOneProcessor<TEntity, TJoin>();
            cascadeMetadata.ProcessorHandler = GetProcessor;
        }

        IClassJoinCascadeProcessor GetProcessor()
        {
            if (!inited)
            {
                var pa = PropertyAccessorFactory.GetPropertyAccess(typeof(TEntity));

                processor.KeyFunc = (entity) => PropertyAccessorFactory.GetId(entity);
                processor.JoinKeyFunc = (join) => PropertyAccessorFactory.GetId(join);
                processor.JoinFunc = (join) => (TJoin)pa.GetGetter(joinMetadata.JoinName).Get(join);
                inited = true;
            }
            return processor;
        }

        public ClassJoinOneCascadeDefine<TEntity, TJoin> ForeignKey(Expression<Func<TJoin, object>> memberExpression)
        {
            var pa = PropertyAccessorFactory.GetPropertyAccess(typeof(TJoin));
            var member = ReflectionHelper.GetProperty(memberExpression.Body);
            processor.ForeignKeyAction = (join, value) => pa.GetSetter(member.Name).Set(join, value);
            return this;
        }
    }
}
