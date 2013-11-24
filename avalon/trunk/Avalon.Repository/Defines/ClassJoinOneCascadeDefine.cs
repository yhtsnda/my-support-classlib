using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Avalon.Framework
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
                var pa = TypeAccessor.GetAccessor(typeof(TEntity));

                processor.KeyFunc = (entity) => EntityUtil.GetId(entity);
                processor.JoinKeyFunc = (join) => EntityUtil.GetId(join);
                processor.JoinFunc = (join) => (TJoin)pa.GetProperty(joinMetadata.JoinName, join);
                inited = true;
            }
            return processor;
        }

        public ClassJoinOneCascadeDefine<TEntity, TJoin> ForeignKey(Expression<Func<TJoin, object>> memberExpression)
        {
            var pa = TypeAccessor.GetAccessor(typeof(TJoin));
            var member = ReflectionHelper.GetProperty(memberExpression.Body);
            processor.ForeignKeyAction = (join, value) => pa.SetProperty(member.Name, join, value);
            return this;
        }
    }
}
