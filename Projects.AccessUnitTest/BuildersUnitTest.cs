using System;
using System.Diagnostics;
using System.Linq.Expressions;
using Projects.Framework.Specification;
using Projects.Accesses.Builders;
using Projects.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Projects.AccessUnitTest
{
    [TestClass]
    public class BuildersUnitTest
    {
        [TestMethod]
        public void WhereClauseBuilderTest()
        {
            bool a = true, b = true;
            var spec = CreateSpecification();
            var builder = CreateWhereClauseBuilder<TestUser>();
            var result = builder.BuildWhereClause(spec.Where(item => item.UserName == "test").CriteriaExpression);
            Trace.WriteLine(result.WhereClause);
            Assert.AreEqual(a, b);
        }

        private ISpecification<TestUser> CreateSpecification()
        {
            ISpecificationProvider provider = new IBatisNetSpecificationProvider();
            return provider.CreateSpecification<TestUser>();
        }

        private WhereClauseBuilder<T> CreateWhereClauseBuilder<T>() where T : class, new()
        {
            DefaultStorageMappingResolver<T> resolver = new DefaultStorageMappingResolver<T>();
            return new GeneralWhereClauseBuilder<T>(resolver);
        }
    }

    [TableName("Users", ContainSchema = true, SchemaName = "dbo")]
    public class TestUser
    {
        [ColumnName("AutoCode", IsAutoIdentity = true)]
        public int UserId { get; set; }

        [ColumnName("UserName")]
        public string UserName { get; set; }
    }
}
