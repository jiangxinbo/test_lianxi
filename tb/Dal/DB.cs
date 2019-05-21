using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace tb.Dal
{
    public class DB
    {
        private const string _MongoDbConnectionStr = "mongodb://127.0.0.1/TaoBao";
        //private static IMongoCollection<PlayerInfo> db;
        static DB()
        {
            //db = GetCollection<PlayerInfo>();
        }

        public static IMongoCollection<T> db<T>()
        {
            return GetCollection<T>();
        }

        private static IMongoCollection<T> GetCollection<T>(string collectionName = null)
        {
            MongoUrl mongoUrl = new MongoUrl(_MongoDbConnectionStr);
            var mongoClient = new MongoClient(mongoUrl);
            var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
            return database.GetCollection<T>(collectionName ?? typeof(T).Name);
        }
    }
}
