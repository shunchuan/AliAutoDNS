using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using AliHelper.Signature.Extends;

namespace AliHelper.Signature.Methods
{
    public class Common
    {
        /// <summary>
        /// 返回ISO8601标准格式时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>2015-01-09T12:00:00Z</returns>
        public static string GetISO8601Date(DateTime dateTime)
        {
            string formart = "yyyy-MM-ddTHH:mm:ssZ";
            return dateTime.ToString(formart);
        }

        /// <summary>
        /// 返回当前时间的ISO8601标准格式时间
        /// </summary>
        /// <returns>2015-01-09T12:00:00Z</returns>
        public static string GetNowISO8601Date()
        {
            DateTime dateTime = DateTime.UtcNow;
            string formart = "yyyy-MM-ddTHH:mm:ssZ";
            return dateTime.ToString(formart);
        }

        public static string GetURLEncoder(string str)
        {
           return Uri.EscapeDataString(str);
        }

        /// <summary>
        /// 将结构体转换为Dictionary
        /// 只作为Model的对象使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary<T>(T source) where T : class
        {
            try
            {
                var serialize = JsonConvert.SerializeObject(source);
                //return (Dictionary<string, string>)JsonConvert.DeserializeObject(serialize);
                var dict = new Dictionary<string, string>();
                JsonConvert.PopulateObject(serialize, dict);
                return dict;
            } catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 差生随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomNumber()
        {
            int rtn = 0;
            Random r = new Random();
            byte[] buffer = Guid.NewGuid().ToByteArray();
            int iSeed = BitConverter.ToInt32(buffer, 0);
            r = new Random(iSeed);
            rtn = r.Next(1000000000, 2147483647);
            return rtn;
        }
    }
}
