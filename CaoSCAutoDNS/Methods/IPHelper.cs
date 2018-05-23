using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Configuration;

namespace CaoSCAutoDNS.Methods
{
    public class IPHelper
    {
        public string getLocalIP(string apiUrl)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(apiUrl);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                responseString = responseString.Substring(1);
                responseString = responseString.Substring(0, responseString.Length - 1);
                return responseString;
            }catch(Exception ex)
            {
                Log.ConsoleWrite("获取公网IP失败！");
                Log.ConsoleWriteNoDate(ex.Message);
                return null;
            }
        }
    }
}
