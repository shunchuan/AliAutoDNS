using AliHelper.Signature.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliHelper.Signature.Extends
{
    public static class ParameterUtil
    {
        /// <summary>
        /// 获取Signature前的数据完整性验证
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public static bool ValidateBefSignature(this CommonParameter parameter, out string errmsg)
        {
            errmsg = "";
            if (String.IsNullOrEmpty(parameter.AccessKeyId) || String.IsNullOrWhiteSpace(parameter.AccessKeyId))
            {
                errmsg = "AccessKeyId 不存在";
            }
            else if (String.IsNullOrEmpty(parameter.Format) || String.IsNullOrWhiteSpace(parameter.Format))
            {
                errmsg = "Format 不存在";
            }
            else if (String.IsNullOrEmpty(parameter.SignatureMethod) || String.IsNullOrWhiteSpace(parameter.SignatureMethod))
            {
                errmsg = "SignatureMethod 不存在";
            }
            else if (String.IsNullOrEmpty(parameter.SignatureVersion) || String.IsNullOrWhiteSpace(parameter.SignatureVersion))
            {
                errmsg = "SignatureVersion 不存在";
            }

            else if (String.IsNullOrEmpty(parameter.Timestamp) || String.IsNullOrWhiteSpace(parameter.Timestamp))
            {
                errmsg = "Timestamp 不存在";
            }
            else if (String.IsNullOrEmpty(parameter.Version) || String.IsNullOrWhiteSpace(parameter.Version))
            {
                errmsg = "Version 不存在";
            }

            if (String.IsNullOrEmpty(errmsg) || String.IsNullOrWhiteSpace(errmsg))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 转为为Dictionary<string,string>类型
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary(this CommonParameter parameter) => new Dictionary<string, string>
            {
                {"AccessKeyId",parameter.AccessKeyId },
                {"Format",parameter.Format },
                {"Signature",parameter.Signature },
                {"SignatureMethod",parameter.SignatureMethod },
                {"SignatureNonce",parameter.SignatureNonce },
                {"SignatureVersion",parameter.SignatureVersion },
                { "Timestamp",parameter.Timestamp },
                {"Version",parameter.Version }
            };

        /// <summary>
        /// 将Dictionary<string, string>转换为字符串，过滤空值
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="keyValueFillStr">key和value之间拼接的间隔字符串</param>
        /// <param name="itemFillStr">每条拼接的间隔字符串</param>
        /// <returns></returns>
        public static string ToNotNullString(this Dictionary<string, string> dict,string keyValueFillStr="" ,string itemFillStr="")
        {
            StringBuilder sb = new StringBuilder();
            int index = 0;
            foreach(var key in dict.Keys)
            {
                if (!String.IsNullOrWhiteSpace(dict[key]))
                {
                    sb.Append(index == 0 ? key + keyValueFillStr + dict[key] : itemFillStr + key + keyValueFillStr + dict[key]);
                    index++;
                }
            }
            return sb.ToString();
        }

        public static string ToNotNullKeyValueEncoderString(this Dictionary<string, string> dict, string keyValueFillStr = "", string itemFillStr = "")
        {
            StringBuilder sb = new StringBuilder();
            int index = 0;
            var keyValue = "";
            var value = "";
            foreach (var key in dict.Keys)
            {
                if (!String.IsNullOrWhiteSpace(dict[key]))
                {
                    keyValue = AliHelper.Signature.Methods.Common.GetURLEncoder(key);
                    value = AliHelper.Signature.Methods.Common.GetURLEncoder(dict[key]);
                    sb.Append((index == 0 ? "" : itemFillStr) + keyValue + keyValueFillStr + value);
                    index++;
                }
            }
            return sb.ToString();
        }
    }
}
