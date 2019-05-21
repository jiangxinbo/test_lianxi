using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace tb
{
    [BsonIgnoreExtraElements]
    public class UpdateItemLog
    {
        private string itemId;
        private List<Item> historyItem = new List<Item>();

        /// <summary>
        /// /历史数据
        /// </summary>
        public List<Item> HistoryItem { get => historyItem; set => historyItem = value; }
        /// <summary>
        /// 商品id
        /// </summary>
        public string ItemId { get => itemId; set => itemId = value; }
    }
}
