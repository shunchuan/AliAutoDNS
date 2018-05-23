using AliHelper;
using AliHelper.Signature.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Codeplex.Data;

namespace CaoSCAutoDNS.Methods
{
    public class ToDo
    {
        private static string nowIp = "127.0.0.1";

        /// <summary>
        /// 自动修改DNS解析系统运行
        /// </summary>
        public void Run()
        {
            try
            {
                var config = InitConfig();
                while (true)
                {
                    try
                    {
                        Thread.Sleep(config.SystemSleepTime);
                        IPHelper iPHelper = new IPHelper();
                        var ip = iPHelper.getLocalIP(config.GetPublicNetIPUrl);
                        if (string.IsNullOrWhiteSpace(ip))
                        {
                            continue;
                        }
                        if (ip != nowIp)
                        {
                            var DomainRecords = GetDescribeDomainRecords(config);
                            CheckAddOrUpdate(config,DomainRecords);
                        }

                    }
                    catch (System.Net.Http.HttpRequestException ex)
                    {
                        Log.ConsoleWrite("请求接口失败");
                        Log.ConsoleWriteNoDate(ex.Message);

                    }

                }
            }
            catch (ArgumentNullException ex)
            {
                Log.ConsoleWrite(ex.Message);
                Log.ConsoleWriteNoDate("系统已停止...");
            }
        }

        /// <summary>
        /// 获取解析记录列表字符串
        /// </summary>
        /// <param name="config"></param>
        public string GetDescribeDomainRecords(Config config)
        {
            DescribeDomainRecordsModel model = new DescribeDomainRecordsModel();
            model.DomainName = config.SetDNSDomainName;
            model.RRKeyWord = config.SetDNSHostRecord;
            var uri = API.GetDescribeDomainRecords(config.AccessKeyId, config.AccessKeySecret, model);
            var url = "https://alidns.aliyuncs.com/?" + uri;

            return HttpUtility.Get(url);
        }

        private AddOrUpdate CheckAddOrUpdate(Config config, string result)
        {
            var jjson= JsonConvert.DeserializeObject<dynamic>(result);
            jjson["DomainRecords"].Count;
            var dynamicJson = DynamicJson.Parse(result);
            //if (dynamicJson.TotalCount == 0)
            //    return AddOrUpdate.ADD;
            var Record = dynamicJson.DomainRecords.Record;
            if( Record.Length > 0)
            {
                return AddOrUpdate.UPDATE;
            }
            return AddOrUpdate.ADD;
        }
        /// <summary>
        /// 初始化系统参数，有字段为空则抛出ArgumentNullException异常
        /// </summary>
        /// <returns></returns>
        private Config InitConfig()
        {
            Config config = new Config
            {
#pragma warning disable CS0618 // 类型或成员已过时
                GetPublicNetIPUrl = ConfigurationManager.AppSettings["GetPublicNetIPUrl"],
                AccessKeyId = ConfigurationManager.AppSettings["AccessKeyId"],
                AccessKeySecret = ConfigurationManager.AppSettings["AccessKeySecret"],
                SetDNSDomainName = ConfigurationManager.AppSettings["SetDNSDomainName"],
                SetDNSHostRecord = ConfigurationManager.AppSettings["SetDNSHostRecord"],
                SystemSleepTime = Int32.Parse(ConfigurationManager.AppSettings["SystemSleepTime"] ?? "600"),
                DomainNameTTL = ConfigurationManager.AppSettings["DomainNameTTL"],
#pragma warning restore CS0618 // 类型或成员已过时
            };

            if (string.IsNullOrWhiteSpace(config.GetPublicNetIPUrl))
            {
                throw new ArgumentNullException("Config.GetPublicNetIPUrl", "GetPublicNetIPUrl不能为空");
            }

            if (string.IsNullOrWhiteSpace(config.AccessKeyId))
            {
                throw new ArgumentNullException("Config.AccessKeyId", "AccessKeyId不能为空");
            }
            if (string.IsNullOrWhiteSpace(config.AccessKeySecret))
            {
                throw new ArgumentNullException("Config.AccessKeySecret", "AccessKeySecret不能为空");
            }
            if (string.IsNullOrWhiteSpace(config.SetDNSDomainName))
            {
                throw new ArgumentNullException("Config.SetDNSDomainName", "SetDNSDomainName不能为空");
            }
            if (string.IsNullOrWhiteSpace(config.SetDNSHostRecord))
            {
                throw new ArgumentNullException("Config.SetDNSHostRecord", "SetDNSHostRecord不能为空");
            }
            if (string.IsNullOrWhiteSpace(config.DomainNameTTL))
            {
                throw new ArgumentNullException("Config.DomainNameTTL", "DomainNameTTL不能为空");
            }
            return config;
        }

        /// <summary>
        /// 执行解析记录新增或更新
        /// </summary>
        private enum AddOrUpdate
        {
            ADD,
            UPDATE
        }
    }
}
