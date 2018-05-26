using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using AliHelper.Signature.Extends;
using System.Net;

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

        public static string GetURLEncoder(string strs)
        {
            string aaa = "我爱你";
            // StringBuilder sb = new StringBuilder();
            ////return Uri.EscapeDataString(str);
            //return Uri.EscapeUriString(strs);
            return Uri.EscapeDataString(strs);
            for (var index = 0; index < aaa.Length; index++)
            {
                Console.Write(aaa[index]);
            }
            foreach (var str in strs)
            {

            }

            return "";
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

        /// <summary>
        /// 区分大小写排序
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static Dictionary<string, string> AsciiDictionarySort(Dictionary<string, string> dict)
        {
            Dictionary<string, string> asciiDic = new Dictionary<string, string>();
            string[] arrKeys = dict.Keys.ToArray();
            Array.Sort(arrKeys, string.CompareOrdinal);
            foreach (var key in arrKeys)
            {
                string value = dict[key];
                asciiDic.Add(key, value);
            }
            return asciiDic;
        }
    }
}
