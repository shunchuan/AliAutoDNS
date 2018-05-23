using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliHelper.Signature.Model
{
    public class CommonParameter
    {
        /// <summary>
        /// 阿里云颁发给用户的访问服务所用的密钥ID
        /// </summary>
        public string AccessKeyId { set; get; }

        /// <summary>
        /// 返回值的类型，支持JSON与XML。默认为XML
        /// </summary>
        public string Format { set; get; }

        /// <summary>
        /// 签名结果串
        /// </summary>
        public string Signature { set; get; }

        /// <summary>
        /// 签名方式，目前支持HMAC-SHA1
        /// </summary>
        public string SignatureMethod { set; get; }

        /// <summary>
        /// 唯一随机数，用于防止网络重放攻击。用户在不同请求间要使用不同的随机数值
        /// </summary>
        public string SignatureNonce { set; get; }

        /// <summary>
        /// 签名算法版本，目前版本是1.0
        /// </summary>
        public string SignatureVersion { set; get; }

        /// <summary>
        /// 请求的时间戳。日期格式按照ISO8601标准表示，并需要使用UTC时间。
        /// 格式为YYYY-MM-DDThh:mm:ssZ 例如，2015-01-09T12:00:00Z（为UTC时间2015年1月9日12点0分0秒）
        /// </summary>
        public string Timestamp { set; get; }

        /// <summary>
        /// API版本号，为日期形式：YYYY-MM-DD，本版本对应为2015-01-09
        /// </summary>
        public string Version { set; get; }

    }
}
