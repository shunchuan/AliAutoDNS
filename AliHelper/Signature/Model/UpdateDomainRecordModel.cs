using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliHelper.Signature.Model
{
    public class UpdateDomainRecordModel
    {
        public readonly string Action = "UpdateDomainRecord";

        /// <summary>
        /// 解析记录的ID，此参数在添加解析时会返回，在获取域名解析列表时会返回
        /// </summary>
        public string RecordId { set; get; }

        /// <summary>
        /// 主机记录，如果要解析@.exmaple.com，主机记录要填写"@”，而不是空
        /// </summary>
        public string RR { set; get; }

        /// <summary>
        /// 解析记录类型  参见解析记录类型格式 https://help.aliyun.com/document_detail/29805.html?spm=a2c4g.11186623.2.6.wQG32L 
        /// 目前仅支持A记录，此项目即为提供A记录动态改变的解决方案
        /// </summary>
        public string Type { set; get; }

        /// <summary>
        /// 记录值 IP地址
        /// </summary>
        public string Value { set; get; }

        /// <summary>
        /// 可选 生存时间，默认为600秒（10分钟） 按云解析产品级别 参考 https://help.aliyun.com/document_detail/29806.html?spm=a2c4g.11186623.2.7.7ozXuy
        /// </summary>
        public string TTL { set; get; }

        /// <summary>
        /// 可选 MX记录的优先级，取值范围[1,10]，记录类型为MX记录时，此参数必须
        /// </summary>
        public string Priority { set; get; }

        /// <summary>
        /// 可选 解析线路，默认为default。
        /// </summary>
        public string Line { set; get; }
    }
}
