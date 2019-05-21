using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace taobao
{
    public class Http
    {
        private static HttpClient client;
        //private static CookieContainer cc = new CookieContainer();

        static Http()
        {
            newclient();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        

        private static void newclient()
        {
            if (client == null)
            {
                

            var handler = new HttpClientHandler(){AutomaticDecompression =System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate };
                client = new HttpClient(handler);
                client.Timeout = new TimeSpan(0, 0, 10);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
                client.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                client.DefaultRequestHeaders.Add("Referer", @"http://nzdb.dian.nz/product/detail?id=1331659");
                client.DefaultRequestHeaders.Add("Accept-Encoding","gzip, deflate");
                client.DefaultRequestHeaders.Add("Accept-Language","zh-CN,zh;q=0.9,en;q=0.8");
                client.DefaultRequestHeaders.Add("Connection","keep-alive");
                client.DefaultRequestHeaders.Add("Host","nzdb.dian.nz");
                //client.DefaultRequestHeaders.Add("If-None-Match", "W/'13 - t + KxOemSNSYX4zVl6diNsA'");
                client.DefaultRequestHeaders.Add("X-Requested-With"," XMLHttpRequest");
                //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            }
        }

        public static MemoryStream GetFile(string url)
        {
            try
            {
                var data = new Http_Client().get(url);

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("url:{0} , error:{1}", url,ex.Message));

                return null;
            }
        }

       
        

        
        /// <summary>
        /// 请求网站内容
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="timeout">默认超时时间60秒</param>
        /// <returns>返回网页数据</returns>
        public static string Postget_String(string url,int postgetcount=1)
        {
            string result = null;
            //Stopwatch sw = new Stopwatch();
            try
            {
                
                var response = client.GetAsync(url).Result;
                response.Content.Headers.ContentType.CharSet = "utf-8";
                
                result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception )
            {

            }
            finally
            {
                

            }
            return result;
        }

        public static int getTotalPage(int typeid)
        {
            string url = Config.Url + "/thread0806.php?fid=" + Config.TypeId + "&search=&page=1";
            string htmlstr = Http.Postget_String(url);
            if (htmlstr == null) return 0;
            htmlstr = htmlstr.Replace("\r\n\r\n\t\t↑1\r\n\r\n\t\r\n\r\n\t", "");
            htmlstr = htmlstr.Replace("\r\n\r\n\t\t↑2\r\n\r\n\t\r\n\r\n\t", "");
            htmlstr = htmlstr.Replace("\r\n\r\n\t\t↑3\r\n\r\n\t\r\n\r\n\t", "");
            htmlstr = htmlstr.Replace("\r\n\t\r\n\t", "");
            htmlstr = htmlstr.Replace("\r\n\t", "");
            htmlstr = htmlstr.Replace("\r\n\r\n\t", "");
            htmlstr = htmlstr.Replace("[ <span", "<span");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlstr);
            HtmlNode view = doc.GetElementbyId("main");
            if (view == null)
            {

                return 0;
            }
            HtmlNode c_main = view.SelectSingleNode("//a[@class='w70']/input");
            if (c_main == null)
            {

                return 0;
            }
            var min_max = c_main.Attributes["value"].Value.Split('/');
            return int.Parse(min_max[1]);
        }

    }
}
