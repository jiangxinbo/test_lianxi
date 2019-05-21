using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using taobao;
using taobao.Tool;
using tb.Dal;
using Tool;

namespace tb
{
    class Program
    {

        static void get(string sku)
        {
            try
            {
                string name = ShangPin.Search("http://nzdb.dian.nz/search/result?keyword=", sku);
            }
            catch (Exception ex)
            {
                L.File.ErrorFormat("sku={0}  错误={1}", sku, ex);
            }
            finally
            {
                ShangPin.itemCount++;
            }
            
            
        }


        static void Main(string[] args)
        {
            //var url = @"http://pic1.win4000.com/wallpaper/c/58995e3288662.jpg";
            //url = @"http://auhdev-10054974.file.myqcloud.com/product/zfOeR651NxD9MEEFyUrVex5a.png";
            //Class1.r(url);

            //Console.ReadKey();
            List<string> urllist = new List<string>();
            foreach (string row in File.ReadLines(@"tb.txt"))
            {
                string id = row.Trim();
                if (id.Length > 2)
                {
                    urllist.Add(id);
                }
            }

            Console.WriteLine("通过tb.txt 检测到sku数量为: " + urllist.Count);

            for(int iq=0;iq<urllist.Count;iq++)
            {
                get(urllist[iq]);
            }

           

            //for (int i = 0; i < urllist.Count; i++)
            //{
            //    try
            //    {
            //        string name = ShangPin.zhuaqu("http://nzdb.dian.nz/product/detail?id=" + urllist[i]);
            //        Console.WriteLine(name + "   抓取完成");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //        Thread.Sleep(2);

            //    }

            //}



            Console.WriteLine("操作已完成,按回车打开文件夹，按其它任意键返回");
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                {
                    Config.openCurrentPath();
                }
                else
                {
                    System.Diagnostics.Process.GetCurrentProcess().Close();
                    Environment.Exit(0);
                    break;
                }


            }
            return;

        }
    }
}
