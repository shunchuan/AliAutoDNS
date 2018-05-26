using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliHelper.Signature.Extends;
using AliHelper.Signature.Model;

namespace AliHelper.Signature.Methods
{
    /// <summary>
    /// 详见文档 https://help.aliyun.com/document_detail/29747.html?spm=a2c4g.11186623.2.3.7Ak6kX 签名机制
    /// </summary>
    public class Do_Signature
    {
        /// <summary>
        /// 根据公共参数、请求API参数和请求方法，获取GetSignature，用于获取Signature
        /// </summary>
        /// <param name="parameter">公共参数类</param>
        /// <param name="dictAPI">API请求参数类</param>
        /// <param name="HTTPMethod">请求方法</param>
        /// <returns></returns>
        public string GetStringToSign(CommonParameter parameter, Dictionary<string, string> dictAPI, string HTTPMethod)
        {
            var dicParameter = parameter.ToDictionary();
            var aaa = dicParameter.Concat(dictAPI);
            //拼接并排序的胡字符串
            //var dict = dicParameter.Concat(dictAPI).OrderBy(i => i.Key ).ToDictionary(i => i.Key,item=>item.Value);
            var dict = Common.AsciiDictionarySort(dicParameter.Concat(dictAPI).ToDictionary(i => i.Key, item => item.Value));
            var CanonicalizedQueryString =dict.ToNotNullKeyValueEncoderString("=", "&");
            StringBuilder sb = new StringBuilder();
            sb.Append(HTTPMethod);
            sb.Append("&");
            sb.Append(Methods.Common.GetURLEncoder("/"));
            sb.Append("&");
            sb.Append(Methods.Common.GetURLEncoder(CanonicalizedQueryString));
            return sb.ToString();
        }

        public string strASCLLToString(string str)
        {
            var sb = new StringBuilder();
            var bt = Encoding.ASCII.GetBytes(str);
            foreach(var b in bt)
            {
                sb.Append(b.ToString());
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取Signature
        /// </summary>
        /// <param name="StringToSign">加密的字符串</param>
        /// <param name="Secret">密钥</param>
        /// <returns></returns>
        public string GetSignature(string StringToSign, string Secret)
        {
            byte[] hmac = Encrypt.HMAC_SHA1(StringToSign, Secret);
            return Convert.ToBase64String(hmac);
        }


        /// <summary>
        /// 获取api需要的后面的请求字符串
        /// </summary>
        /// <param name="AccessKeySecret"></param>
        /// <param name="parameter"></param>
        /// <param name="dictAPI"></param>
        /// <returns></returns>
        public string GetRFC3986Uri( string AccessKeyId , string AccessKeySecret, Dictionary<string, string> dictAPI)
        {
            CommonParameter parameter = GetCommonParameter();
            parameter.AccessKeyId = AccessKeyId;

            var StringToSign = GetStringToSign(parameter, dictAPI, "GET");
            var Signature = GetSignature(StringToSign, AccessKeySecret+ "&");
            parameter.Signature = Signature;

            var dicParameter = parameter.ToDictionary();
            var dict = dicParameter.Concat(dictAPI).OrderBy(i => i.Key).ToDictionary(i => i.Key, item => item.Value);
            return dict.ToNotNullKeyValueEncoderString("=", "&");
        }

        public CommonParameter GetCommonParameter()
        {
            return new CommonParameter
            {
                Format = "JSON",
                Version = "2015-01-09",
                SignatureMethod = "HMAC-SHA1",
                Timestamp = Common.GetNowISO8601Date(),
                SignatureVersion = "1.0",
                SignatureNonce = Common.GetRandomNumber().ToString(),
            };
        }
    }
}
