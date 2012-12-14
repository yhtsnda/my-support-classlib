using System;
using Projects.Accesses.Builders;
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
            Assert.AreEqual(a, b);
        }
    }
}
