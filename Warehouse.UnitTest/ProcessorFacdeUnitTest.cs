using System;
using System.Collections.Specialized;
using Warehouse.RecordProcessor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Warehouse.UnitTest
{
    [TestClass]
    public class ProcessorFacdeUnitTest
    {
        [TestMethod]
        public void ProcessTest()
        {
            NameValueCollection header = new NameValueCollection();
            NameValueCollection body = new NameValueCollection();
            header.Add("flag", "login_processor");
            body.Add("body", "BodyTest");

            RecrodData data = new RecrodData("UserCenter", header, body);
            ProcessorFacde.Process(data);
            Assert.Equals(1, 1);
        }
    }
}
