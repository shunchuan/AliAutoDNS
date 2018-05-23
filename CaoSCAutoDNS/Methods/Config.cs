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
    }
}
