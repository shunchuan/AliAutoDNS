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
using Microsoft.Win32;

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
                Log.ConsoleWrite("初始化应用程序...");
                var config = InitConfig();
                AutoRun(config);
                Log.ConsoleWrite("公网IP自动解析程序已启动，正在运行...");
                LoopRun(config);
            }
            catch (ArgumentNullException ex)
            {
                Log.ConsoleWrite(ex.Message);
                Log.ConsoleWriteNoDate("系统已停止,按任意键退出...");
            }
            Console.Read();            
        }

        /// <summary>
        /// 执行循环
        /// </summary>
        /// <param name="config">配置文件</param>
        private void LoopRun(Config config)
        {
            var times = 0;
            while (true)
            {
                times++;
                // 每十次清空一次数据，防止手动在阿里云平台修改记录
                if (times > config.RefreshTimes)
                {
                    if (config.RefreshRun)
                    {
                        config.NowPublicNetIP = "";
                        config.LastPublicNetIP = "";
                        config.DNSRecordId = "";
                    }
                    times = 0;
                }
                try
                {
                    Thread.Sleep(config.SystemSleepTime);
                    IPHelper iPHelper = new IPHelper();
                    config.NowPublicNetIP = iPHelper.getLocalIP(config.GetPublicNetIPUrl);
                    if (string.IsNullOrWhiteSpace(config.NowPublicNetIP))
                    {
                        // 未获取到IP，则继续
                        continue;
                    }
                    if (config.NowPublicNetIP == config.LastPublicNetIP)
                    {
                        // 如果与之前IP相同，则不调用阿里云接口检查是否需要更新
                        continue;
                    }
                    var domainRecordsResult = GetDescribeDomainRecords(config);
                    var checkStatus = CheckAddOrUpdate(config, domainRecordsResult);
                    if (checkStatus == AddOrUpdate.STAY)
                    {
                        config.LastPublicNetIP = config.NowPublicNetIP;
                        continue;
                    }
                    else if (checkStatus == AddOrUpdate.ADD)
                    {
                        AddDomainRecord(config);
                        config.LastPublicNetIP = config.NowPublicNetIP;
                    }
                    else if (checkStatus == AddOrUpdate.UPDATE)
                    {
                        UpdateDomainRecord(config);
                        config.LastPublicNetIP = config.NowPublicNetIP;
                    }
                    Log.ConsoleWrite("解析记录成功！" +
                            "解析地址：" + config.SetDNSHostRecord + "." + config.SetDNSDomainName + ";" +
                            "当前IP：" + config.LastPublicNetIP
                            );
                    Log.ConsoleWriteNoDate("继续监视中...");
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    Log.ConsoleWrite("请求接口失败");
                    Log.ConsoleWriteNoDate(ex.Message);
                }
                catch (Exception ex)
                {
                    Log.ConsoleWrite("未知错误");
                    Log.ConsoleWriteNoDate(ex.Message);
                }
            }
        }

        private void AutoRun(Config config)
        {
            var ApplicationInfo = AppDomain.CurrentDomain.SetupInformation;

            var ApplicationName = ApplicationInfo.ApplicationName;
            //获取程序路径
            var execPath = ApplicationInfo.ApplicationBase+ ApplicationName;

            bool isexc = false;
            try
            {

                RegistryKey RKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                //设置自启的程序叫获取目录下的程序名字
                string[] ar = RKey.GetValueNames();
                foreach (string st in ar)
                {
                    if (st.Equals(ApplicationName))
                    {
                        isexc = true;
                    }
                }
                if (!isexc && config.BootFromBoot)
                {
                    //设置开机自启
                    RKey.SetValue(ApplicationName, execPath);
                    Log.ConsoleWrite("已设置开机自启...");
                }
                else if (isexc && !config.BootFromBoot)
                {
                    // 取消开机自启
                    RKey.DeleteValue(ApplicationName);
                    Log.ConsoleWrite("已关闭开机自启...");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 获取解析记录列表字符串
        /// </summary>
        /// <param name="config"></param>
        public string GetDescribeDomainRecords(Config config)
        {
            // 不做分页，取最大值500
            var pageSize = "500";
            DescribeDomainRecordsModel model = new DescribeDomainRecordsModel();
            model.DomainName = config.SetDNSDomainName;
            model.RRKeyWord = config.SetDNSHostRecord;
            model.PageSize = pageSize;
            var uri = API.GetDescribeDomainRecords(config.AccessKeyId, config.AccessKeySecret, model);
            var url = config.AliAPIUrl + uri;
            return HttpUtility.Get(url);
        }

        /// <summary>
        /// 新增解析记录
        /// </summary>
        /// <param name="config"></param>
        /// <returns>请求错误会触发HttpRequestException </returns>
        public bool AddDomainRecord(Config config)
        {
            AddDomainRecordModel model = new AddDomainRecordModel
            {
                DomainName=config.SetDNSDomainName,
                RR = config.SetDNSHostRecord,
                Type=config.SupportDNSType,
                Value=config.NowPublicNetIP,
                TTL=config.DomainNameTTL,
                Line=config.DNSLine
            };
            var uri = API.GetAddDomainRecordUrl(config.AccessKeyId, config.AccessKeySecret, model);
            var url = config.AliAPIUrl + uri;
            var result= HttpUtility.Get(url);
            config.DNSRecordId = JsonConvert.DeserializeObject<dynamic>(result)["RecordId"].ToString();
            return true;
        }

        /// <summary>
        /// 修改解析记录
        /// </summary>
        /// <param name="config"></param>
        /// <returns>请求错误会触发HttpRequestException </returns>
        public bool UpdateDomainRecord(Config config)
        {
            var model = new UpdateDomainRecordModel
            {
                RecordId=config.DNSRecordId,
                RR = config.SetDNSHostRecord,
                Type = config.SupportDNSType,
                Value = config.NowPublicNetIP,
                TTL = config.DomainNameTTL,
                Line = config.DNSLine
            };
            var uri = API.GetUpdateDomainRecordUrl(config.AccessKeyId, config.AccessKeySecret, model);
            var url = config.AliAPIUrl + uri;
            var result = HttpUtility.Get(url);
            config.DNSRecordId = JsonConvert.DeserializeObject<dynamic>(result)["RecordId"].ToString();
            return true;
        }



        /// <summary>
        /// 检查返回的获取解析列表是否为空或解析记录是否不同
        /// 目前支持主机记录、TTL和Line线路
        /// </summary>
        /// <param name="config"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private AddOrUpdate CheckAddOrUpdate(Config config, string result)
        {
            var json = JsonConvert.DeserializeObject<dynamic>(result);
            if (int.Parse(json["TotalCount"].ToString()) == 0)
            {
                return AddOrUpdate.ADD;
            }
            var domainRecords = json["DomainRecords"]["Record"];
            if (domainRecords.Count == 0)
            {
                return AddOrUpdate.ADD;
            }
            foreach (var record in domainRecords)
            {
                // 如果当前解析记录已存在并且与当前循环记录不同，则跳过
                if(!string.IsNullOrWhiteSpace(config.DNSRecordId) && config.DNSRecordId!= record["RecordId"].ToString())
                {
                    continue;
                }
                // 如果解析主机不相同，则跳过
                if (record["RR"].ToString() != config.SetDNSHostRecord)
                {
                    continue;
                }
                config.DNSRecordId = record["RecordId"].ToString();

                //以阿里云服务器设置为准，如果暂停状态，需手动到平台更新
                //// 如果当前记录为停止状态，则更新
                //if (record["Status"].ToString().ToUpper() == "DISABLE")
                //    return AddOrUpdate.DELETE;
                if (record["Type"].ToString() != config.SupportDNSType)
                {
                    throw new FormatException("不支持的解析类型！");
                }
                if (record["Value"].ToString() == config.NowPublicNetIP &&
                    record["TTL"].ToString() == config.DomainNameTTL &&
                    record["Line"].ToString() == config.DNSLine
                    )
                    return AddOrUpdate.STAY;
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
                AliAPIUrl = ConfigurationManager.AppSettings["AliAPIUrl"],
                AccessKeyId = ConfigurationManager.AppSettings["AccessKeyId"],
                AccessKeySecret = ConfigurationManager.AppSettings["AccessKeySecret"],
                SetDNSDomainName = ConfigurationManager.AppSettings["SetDNSDomainName"],
                SetDNSHostRecord = ConfigurationManager.AppSettings["SetDNSHostRecord"],
                SystemSleepTime = Int32.Parse(ConfigurationManager.AppSettings["SystemSleepTime"] ?? "600"),
                DomainNameTTL = ConfigurationManager.AppSettings["DomainNameTTL"],
                LastPublicNetIP = "127.0.0.1",
                DNSLine = ConfigurationManager.AppSettings["DNSLine"],
                RefreshRun = Boolean.Parse(ConfigurationManager.AppSettings["RefreshRun"]),
                BootFromBoot = Boolean.Parse(ConfigurationManager.AppSettings["BootFromBoot"]),
                RefreshTimes = int.Parse(ConfigurationManager.AppSettings["RefreshTimes"])
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
            /// <summary>
            /// 执行新增
            /// </summary>
            ADD,
            /// <summary>
            /// 执行更新
            /// </summary>
            UPDATE,
            /// <summary>
            /// 执行保留不操作
            /// </summary>
            STAY,
            /// <summary>
            /// 执行删除 
            /// </summary>
            DELETE
        }
    }
}
