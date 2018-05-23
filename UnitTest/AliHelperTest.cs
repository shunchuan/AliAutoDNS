using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AliHelper;
using AliHelper.Signature.Model;

namespace UnitTest
{
    [TestClass]
    public class AliHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void GetDescribeDomainRecordsTest()
        {
            DescribeDomainRecordsModel model = new DescribeDomainRecordsModel();
            var AccessKeyId = "123456";
            var AccessKeySecret = "123456789";
            var uri = API.GetDescribeDomainRecords(AccessKeyId, AccessKeySecret, model);
            var url = "https://alidns.aliyuncs.com/?" + uri;
        }
    }
}
