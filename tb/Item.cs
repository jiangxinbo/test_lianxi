using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using tb.Dal;

namespace tb
{
    public class Item
    {
        
        private string id;
        public Item()
        {
            id = ObjectId.GenerateNewId().ToString();
        }
        private string spid;
        private string _name;
        private string sku;
        private string price;
        private string stock;
        private List<string> headImgList=new List<string> ();
        private List<string> bodyImgList=new List<string> ();
        private string brandId; //品牌id
        private string categoryId;//类别
        private string warehouseId;//仓库
        private string status;//状态
        private string position;//位置
        private string detailInfo;//详细信息
        private string shippingFeeWay;
        private string shippingFree;
        private string zeroShippingFeeQty;
        private string fixedShippingFee;
        private string weight;
        private string validDate;
        private string nameAlias;
        private string description;
        private List<string> carouselImgs=new List<string>();//轮播图
        private List<string> carouselImages = new List<string>();//轮播图
        private string showCurrencyId; //货币类型
        private string type;
        private string exportPrice;
        private Brand brand=new Brand();//品牌
        private Category category=new  tb.Category();//类别
        private List<string> tags=new List<string> ();//标签
        private string url;
        private List<string> historyPrice = new List<string>();//历史价格

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get => id; set => id = value; }
        public string name { get => _name; set => _name = value; }
        public string Sku { get => sku; set => sku = value; }
        public string Zhongliang { get => weight; }
        public string Youxiaoqi
        {
            get
            {
                var time = UnixTimestampToDateTime(Convert.ToInt64(validDate));
                return  time.ToLongDateString();
            }

        }
        public string Fahuocang
        {
            get {
                switch(WarehouseId)
                {
                    case "1": return "新西兰仓";
                    case "2":return "澳洲仓";
                    default:return WarehouseId;
                }
            }
        }
        public string Price { get => price; set => price = value; }
        /// <summary>
        /// 库存
        /// </summary>
        public string Stock { get => stock; set => stock = value; }
        public List<string> HeadImgList { get => headImgList; set => headImgList = value; }
        public List<string> BodyImgList { get => bodyImgList; set => bodyImgList = value; }
        /// <summary>
        /// 品牌标识
        /// </summary>
        public string BrandId { get => brandId; set => brandId = value; }
        /// <summary>
        /// 类别ID
        /// </summary>
        public string CategoryId { get => categoryId; set => categoryId = value; }
        /// <summary>
        /// 货仓编号
        /// </summary>
        public string WarehouseId { get => warehouseId; set => warehouseId = value; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get => status; set => status = value; }
        /// <summary>
        /// 位置
        /// </summary>
        public string Position { get => position; set => position = value; }
        /// <summary>
        /// 详细信息
        /// </summary>
        public string DetailInfo { get => detailInfo; set => detailInfo = value; }
        /// <summary>
        /// 运费方式
        /// </summary>
        public string ShippingFeeWay { get => shippingFeeWay; set => shippingFeeWay = value; }
        /// <summary>
        /// 免运费
        /// </summary>
        public string ShippingFree { get => shippingFree; set => shippingFree = value; }
        /// <summary>
        /// 零运费数量
        /// </summary>
        public string ZeroShippingFeeQty { get => zeroShippingFeeQty; set => zeroShippingFeeQty = value; }
        /// <summary>
        /// 固定运费
        /// </summary>
        public string FixedShippingFee { get => fixedShippingFee; set => fixedShippingFee = value; }
        /// <summary>
        /// 重量
        /// </summary>
        public string Weight { get => weight; set => weight = value; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public string ValidDate { get => validDate; set => validDate = value; }
        /// <summary>
        /// 名字别名
        /// </summary>
        public string NameAlias { get => nameAlias; set => nameAlias = value; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get => description; set => description = value; }
        /// <summary>
        /// 轮播图
        /// </summary>
        public List<string> CarouselImgs { get => carouselImgs; set => carouselImgs = value; }
        /// <summary>
        /// 轮播图像
        /// </summary>
        public List<string> CarouselImages { get => carouselImages; set => carouselImages = value; }
        /// <summary>
        /// 显示的货币ID
        /// </summary>
        public string ShowCurrencyId { get => showCurrencyId; set => showCurrencyId = value; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get => type; set => type = value; }
        /// <summary>
        /// 出口价格
        /// </summary>
        public string ExportPrice { get => exportPrice; set => exportPrice = value; }
        /// <summary>
        /// 品牌信息
        /// </summary>
        public Brand Brand { get => brand; set => brand = value; }
        /// <summary>
        /// 类别信息
        /// </summary>
        public Category Category { get => category; set => category = value; }
        /// <summary>
        /// 标签
        /// </summary>
        public List<string> Tags { get => tags; set => tags = value; }
        /// <summary>
        /// 商品访问地址
        /// </summary>
        public string Url { get => url; set => url = value; }
        /// <summary>
        /// 货币名称
        /// </summary>
        public string Currency
        {
            get
            {
                switch (showCurrencyId)
                {
                    case "1":return "可能是美金";
                    case "2":return "人民币";
                    default:return showCurrencyId;
                }
            }
        }

        public string Spid { get => spid; set => spid = value; }
        public List<string> HistoryPrice { get => historyPrice; set => historyPrice = value; }

        public  DateTime UnixTimestampToDateTime(long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, new DateTime().Kind);
            return start.AddSeconds(timestamp);
        }

        public void check()
        {
            var filter = Builders<Item>.Filter.Eq("Spid", Spid);

            var itemlist = DB.db<Item>().FindSync(filter).ToList();
            if (itemlist.Count == 0)
            {
                DB.db<Item>().InsertOne(this);
            }
            else
            {
                this.price = "1";
                var old = itemlist[0];
                if (old.price != this.price)
                {

                    var filterup = Builders<UpdateItemLog>.Filter.Eq("ItemId",Convert.ToString( old.Id));
                    var historyList = DB.db<UpdateItemLog>().FindSync(filterup).ToList();
                    if (historyList.Count == 0)
                    {
                        UpdateItemLog uil = new UpdateItemLog();
                        uil.ItemId = old.id;
                        uil.HistoryItem.Add(old);
                        DB.db<UpdateItemLog>().InsertOne(uil);
                    }
                    else
                    {
                        UpdateItemLog uil = historyList[0];
                        uil.HistoryItem.Add(old);
                        ReplaceOneResult ror= DB.db<UpdateItemLog>().ReplaceOne(filterup, uil);
                        var count = ror.ModifiedCount;
                    }

                    
                    
                    Console.WriteLine("发送email通知 商品价格已被修改");
                    this.id = old.id;
                    this.update(this);
                }
            }
            
            
        }


        private bool update(Item it)
        {
            if(id==null)
            {
                return false;
            }
            else
            {
                var filter = Builders<Item>.Filter.Eq("Spid", it.Spid);
                var result = DB.db<Item>().ReplaceOne(filter, it);
                return result.ModifiedCount != 0;
            }
            
        }
        


    }
}
