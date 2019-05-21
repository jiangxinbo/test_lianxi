using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using tb;
using Tool;

namespace taobao.Tool
{
    public class ShangPin
    {
        static int count = 0;
        public static int itemCount = 1;//商品数量
        public static string Search(string weburl,string sku)
        {
            weburl += sku;
            string htmlstr = null;
            try
            {
                count = 0;
                htmlstr = Http.Postget_String(weburl);
                if(htmlstr==null)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine(string.Format("第{0}次重连", ++count));
                    return Search(weburl, sku);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Thread.Sleep(1000);
                Console.WriteLine(string.Format("第{0}次重连", ++count));
                return Search(weburl,sku);
            }
            
            //if(htmlstr==null)
            //{
            //    Console.Write("*");
            //    Thread.Sleep(new Random().Next(1,3));
            //    return search(weburl, sku);
            //}
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlstr);

            //HtmlNode data = doc.DocumentNode.SelectSingleNode("//div[@class='searchProduct']");

            var itemList = doc.DocumentNode.SelectSingleNode("//div[@id='searchProduct']");
            string html = HttpUtility.HtmlDecode(itemList.InnerText);
            var jsonObj=JsonConvert.DeserializeObject<dynamic>(html);
            var t = jsonObj.data;
            if(t.Count==0)
            {
                L.File.WarnFormat("sku={0}   网址:{1} 没有找到商品",sku, weburl + sku);
            }
            foreach (var item in t)
            {
                Item tt = new Item();
                var info = item;
                L.File.Info(Convert.ToString(item));
                tt.Spid = info["id"];
                tt.Url = @"http://nzdb.dian.nz/product/detail?id=" + tt.Spid;
                tt.name = info["name"];
                tt.BrandId = info["brandId"];
                tt.CategoryId = info["categoryId"];
                tt.WarehouseId= info["warehouseId"];
                if (tt.WarehouseId != "1")
                {
                    continue;
                }
                tt.Status= info["status"];
                tt.Position= info["position"];
                tt.DetailInfo= info["detailInfo"];
                tt.Sku= info["sku"];
                tt.ShippingFeeWay= info["shippingFeeWay"];
                tt.FixedShippingFee = info["shippingFree"];
                tt.ZeroShippingFeeQty= info["zeroShippingFeeQty"];
                tt.FixedShippingFee= info["fixedShippingFee"];
                tt.Weight= info["weight"];
                tt.ValidDate= info["validDate"];
                tt.NameAlias= info["nameAlias"];
                tt.Description= info["description"];
                var carouselImgs = JsonConvert.DeserializeObject<dynamic>(Convert.ToString(info.carouselImgs));
                foreach (var i in carouselImgs)
                {
                    tt.CarouselImgs.Add(Convert.ToString(i));
                }
                //foreach(var i in JsonConvert.DeserializeObject<dynamic>(Convert.ToString(info.carouselImgs)))
                //{
                //    tt.CarouselImgs.Add(Convert.ToString(i));
                //}
                foreach(var i in JsonConvert.DeserializeObject<dynamic>(Convert.ToString(info["carouselImages"])))
                {
                    tt.CarouselImages.Add(Convert.ToString(i));
                    tt.HeadImgList.Add(Convert.ToString(i));
                }
                
                tt.ShowCurrencyId= info["showCurrencyId"];
                tt.Type= info["type"];
                tt.Price= info["price"];
                tt.ExportPrice= info["exportPrice"];
                tt.Stock= info["stock"];
                var b = info["brand"];
                tt.Brand.Id = b["id"];
                tt.Brand.Name = b["name"];
                tt.Brand.Status = b["status"];
                tt.Brand.Position = b["position"];
                tt.Brand.Description = b["description"];
                tt.Brand.LogoUrl = b["logoUrl"];
                tt.Brand.BackgroundUrl = b["backgroundUrl"];

                var c = info["category"];
                tt.Category.Id = c["id"];
                tt.Category.Name = c["name"];
                tt.Category.Status = c["status"];
                tt.Category.ParentId = c["parentId"];
                tt.Category.Position = c["position"];
                tt.Category.Remark = c["remark"];
                tt.Category.HotSearch = c["hotsearch"];
                tt.Category.ImageUrl = c["imageUrl"];
                tt.Category.ParentCategory = c["parentCategory"];
                tt.Category.ChildrenCategories = c["childrenCategories"];

                foreach (var i in JsonConvert.DeserializeObject<dynamic>(Convert.ToString(info["tags"])))
                {
                    tt.Tags.Add(Convert.ToString(i));
                }

                if(Config.useMongoDB)
                {
                    tt.check();
                }
                saveSearch(tt);

                Console.WriteLine("已处理商品:{0}    sku:{1}   url:{2}",itemCount, tt.Sku, tt.Url);
            }


            return "";
        }



        public static void saveSearch(Item info)
        {

            var charlist = Path.GetInvalidFileNameChars().ToList();
            charlist.Add('"');
            charlist.Add('<');
            charlist.Add('>');
            charlist.Add('|');
            charlist.Add('\0');
            charlist.Add(':');
            charlist.Add('*');
            charlist.Add('?');
            charlist.Add('\\');
            charlist.Add('/');

            foreach (var citem in charlist)
            {
                info.name = info.name.Replace(citem, '_');
                info.Sku= info.Sku.Replace(citem, '_');
            }


            string newdir = info.Sku; // string.Format("{0}_{1}", info.Sku, info.name);
            newdir=newdir.Replace(" ", "");
            if(newdir.Length>20)
            {
                newdir=newdir.Substring(0, 20);
            }
            List<string> strlist = new List<string>();
            strlist.Add(string.Format("{0}: {1}", "网址".PadRight(6), info.Url));
            strlist.Add(string.Format("{0}: {1} ({2})", "售价".PadRight(6), info.Price,info.Currency));
            strlist.Add(string.Format("{0}: {1}", "SKU".PadRight(6), info.Sku));
            strlist.Add(string.Format("{0}: {1} (g 单位应该都是克)", "重量".PadRight(6), info.Zhongliang));
            strlist.Add(string.Format("{0}: {1}", "库存".PadRight(6), info.Stock));
            strlist.Add(string.Format("{0}: {1}", "有效期".PadRight(4), info.Youxiaoqi));
            strlist.Add(string.Format("{0}: {1}", "发货仓".PadRight(4), info.Fahuocang));
            strlist.Add("-----------------------------------------------------------------------------------");
            strlist.Add(string.Format("{0}: {1}", "品牌名称".PadRight(2), info.Brand.Name));
            strlist.Add(string.Format("{0}: {1}", "品牌描述".PadRight(2), info.Brand.Description));
            strlist.Add(string.Format("{0}: {1}", "品牌背景图".PadRight(0), info.Brand.BackgroundUrl));
            strlist.Add(string.Format("{0}: {1}", "品牌logo".PadRight(2), info.Brand.LogoUrl));
            strlist.Add(string.Format("{0}: {1}", "品牌状态".PadRight(2), info.Brand.Status));
            strlist.Add("-----------------------------------------------------------------------------------");
            strlist.Add(string.Format("{0}: {1}", "分类名称".PadRight(2), info.Category.Name));
            strlist.Add(string.Format("{0}: {1}", "分类状态".PadRight(2), info.Category.Status));
            


            File.WriteAllLines(Config.GetItemInfoPath(info.name, newdir), strlist, Encoding.UTF8);


            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(info.DetailInfo);
            var itemList = doc.DocumentNode.SelectNodes("//img");
            if(itemList!=null)
            {
                foreach (var img in itemList)
                {
                    string imgurl = img.GetAttributeValue("src", null);
                    info.BodyImgList.Add(imgurl);
                }
            }
            

            ParallelLoopResult r1 = FileManager.SaveImageList(info.HeadImgList, newdir, info.Sku, "轮播");
            ParallelLoopResult r2 = FileManager.SaveImageList(info.BodyImgList, newdir, info.Sku, "详情");

            if(info.Brand.LogoUrl!=null && info.Brand.LogoUrl.Length > 0){
                ParallelLoopResult r3 = FileManager.SaveImageList(new List<string>() { info.Brand.LogoUrl }, newdir, info.Sku, "品牌logo");
            }
            if(info.Brand.BackgroundUrl!=null && info.Brand.BackgroundUrl.Length>0)
            {
                ParallelLoopResult r4 = FileManager.SaveImageList(new List<string>() { info.Brand.BackgroundUrl }, newdir, info.Sku, "品牌背景");
            }
        }








        public static  string zhuaqu(string weburl)
        {
            string htmlstr = Http.Postget_String(weburl);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlstr);
            HtmlNode itemInfo = doc.DocumentNode.SelectSingleNode("//div[@class='detail-col-right']");
            HtmlNode headImag = doc.DocumentNode.SelectSingleNode("//div[@class='detail-col-left']");
            string name = itemInfo.SelectSingleNode("h3").InnerText;

            foreach (var citem in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(citem, '_');
            }

            string price = itemInfo.SelectSingleNode("//span[@class='info-detail price']").InnerText;
            var info = itemInfo.SelectNodes("//span[@class='info-detail']");
            string sku = info[0].InnerText;
            string zhongliang = info[1].InnerText;
            string kucun = info[2].InnerText;
            string youxiaoqi = info[3].InnerText;
            string fahuocang = info[4].InnerText;
            string newdir = string.Format("{0} - {1}", sku, name);
            List<string> strlist = new List<string>();
            strlist.Add(string.Format("{0}: {1}", "售价".PadRight(6), price));
            strlist.Add(string.Format("{0}: {1}", "SKU".PadRight(6), sku));
            strlist.Add(string.Format("{0}: {1}", "重量".PadRight(6),zhongliang));
            strlist.Add(string.Format("{0}: {1}", "库存".PadRight(6), kucun));
            strlist.Add(string.Format("{0}: {1}", "有效期".PadRight(4), youxiaoqi));
            strlist.Add(string.Format("{0}: {1}", "发货仓".PadRight(4), fahuocang));

            File.WriteAllLines(Config.GetItemInfoPath(name, newdir), strlist, Encoding.UTF8);



            var heads = headImag.SelectNodes("//div[@id='select-icon']/ul/li");
            List<string> headimglist = new List<string>();
            foreach (var img in heads)
            {
                string imgstr = img.GetAttributeValue("style", null);
                imgstr = imgstr.Replace("background-image:url(", "");
                imgstr = imgstr.Remove(imgstr.Length - 1);
                headimglist.Add(imgstr);
            }
            HtmlNode bottomInfo = doc.DocumentNode.SelectSingleNode("//div[@class='detail-bottom-info']");
            var imgs = bottomInfo.SelectNodes("//div/div/p/img");
            List<string> imglist = new List<string>();
            if (imgs!=null)
            {
                
                foreach (var img in imgs)
                {
                    string imgurl = img.GetAttributeValue("src", null);
                    imglist.Add(imgurl);
                }
            }
            
            
            ParallelLoopResult r1= FileManager.SaveImageList(headimglist, newdir, name, "头部 ");
            ParallelLoopResult r2 = FileManager.SaveImageList(imglist, newdir, name, "详情 ");
            return name;
        }
    }
}
