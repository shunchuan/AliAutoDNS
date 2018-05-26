using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaoSCAutoDNS.Methods
{
    public class Config
    {
        /// <summary>
        /// 获取本地公网IP的接口地址
        /// </summary>
        public string GetPublicNetIPUrl { set; get; }

        /// <summary>
        /// 阿里云api接口地址
        /// </summary>
        public string AliAPIUrl { set; get; }

        public string AccessKeyId { set; get; }

        public string AccessKeySecret { set; get; }

        /// <summary>
        /// 需要设置的解析域名
        /// </summary>
        public string SetDNSDomainName { set; get; }

        /// <summary>
        /// 需要设置的域名主机记录
        /// </summary>
        public string SetDNSHostRecord { set; get; }

        /// <summary>
        /// 系统休眠时间 毫秒
        /// </summary>
        public int SystemSleepTime { set; get; }

        /// <summary>
        /// 域名解析TTL
        /// </summary>
        public string DomainNameTTL { set; get; }

        /// <summary>
        /// 之前已生效的公网IP
        /// </summary>
        public string LastPublicNetIP { get; set; }

        /// <summary>
        /// 当前公网IP
        /// </summary>
        public string NowPublicNetIP { set; get; }

        /// <summary>
        /// 解析线路
        /// </summary>
        public string DNSLine { set; get; }

        /// <summary>
        /// 当前支持的解析类型
        /// </summary>
        public readonly string SupportDNSType = "A";

        /// <summary>
        /// 解析记录ID，此参数在添加解析时会返回，在获取域名解析列表时会返回
        /// </summary>
        public string DNSRecordId { set; get; }

        /// <summary>
        /// 是否开启刷新本地解析记录，如果开启，可防止程序运行期间解析记录在其他地方修改
        /// </summary>
        public bool RefreshRun { set; get; }

        /// <summary>
        /// 如果开启刷新本地解析记录，则多少个间隔刷新一次， 即刷新时间间隔为 RefreshTimes * SystemSleepTime
        /// </summary>
        public int RefreshTimes { set; get; }

        /// <summary>
        /// 是否开机自启动
        /// </summary>
        public bool BootFromBoot { set; get; }
    }
}
