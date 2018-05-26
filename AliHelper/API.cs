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
            var model = Common.ToDictionary(describeDomainRecordsModel);
            return GetUrlAPI(AccessKeyId, AccessKeySecret, model);
        }

        /// <summary>
        /// 获取添加解析记录的请求字符串
        /// </summary>
        /// <param name="AccessKeyId"></param>
        /// <param name="AccessKeySecret"></param>
        /// <param name="addDomainRecordModel"></param>
        /// <returns></returns>
        public static string GetAddDomainRecordUrl(string AccessKeyId, string AccessKeySecret, AddDomainRecordModel addDomainRecordModel)
        {
            var model = Common.ToDictionary(addDomainRecordModel);
            return GetUrlAPI(AccessKeyId, AccessKeySecret, model);
        }

        /// <summary>
        /// 获取更新解析记录的请求字符串
        /// </summary>
        /// <param name="AccessKeyId"></param>
        /// <param name="AccessKeySecret"></param>
        /// <param name="updateDomainRecordModel"></param>
        /// <returns></returns>
        public static string GetUpdateDomainRecordUrl(string AccessKeyId, string AccessKeySecret, UpdateDomainRecordModel updateDomainRecordModel)
        {
            var model = Common.ToDictionary(updateDomainRecordModel);
            return GetUrlAPI(AccessKeyId, AccessKeySecret, model);
        }

        /// <summary>
        /// 获取Url
        /// </summary>
        /// <param name="AccessKeyId"></param>
        /// <param name="AccessKeySecret"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private static string GetUrlAPI(string AccessKeyId, string AccessKeySecret,Dictionary<string,string> model)
        {
            Do_Signature do_Signature = new Do_Signature();
            return do_Signature.GetRFC3986Uri(AccessKeyId, AccessKeySecret, model);
        }
    }
}
