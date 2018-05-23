using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliHelper.Signature.Model;
using AliHelper.Signature.Methods;

namespace AliHelper
{
    public class API
    {
        /// <summary>
        /// 返回获取解析记录列表的请求字符串
        /// </summary>
        /// <returns></returns>
        public static string GetDescribeDomainRecords(string AccessKeyId, string AccessKeySecret, DescribeDomainRecordsModel describeDomainRecordsModel)
        {
            Do_Signature do_Signature = new Do_Signature();
            var dicDescribeDomainRecords = Common.ToDictionary(describeDomainRecordsModel);
            return do_Signature.GetRFC3986Uri(AccessKeyId, AccessKeySecret, dicDescribeDomainRecords);
        }
    }
}
