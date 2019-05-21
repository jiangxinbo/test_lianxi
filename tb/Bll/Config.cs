using System;
using System.IO;
using System.Threading;

namespace taobao
{
    public class Config
    {

        public static bool useMongoDB = false;
        /// <summary>
        /// 同时可运行的任务
        /// </summary>
        public static SemaphoreSlim TaskRun;


        /// <summary>
        /// 当前正在运行的任务数量
        /// </summary>
        public static int currentRunTaskCount = 0;

        /// <summary>
        /// 用于避免频繁访问被封ip
        /// </summary>
        public static SemaphoreSlim WebTimeSpan = new SemaphoreSlim(1, 1);

        static Config()
        {

            Img_path = "淘宝小卖铺";

        }

        public static string ProcessDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory; //return AppContext.BaseDirectory;
            }
        }

        private static int task_count;
        /// <summary>
        /// 任务数量
        /// </summary>
        public static int Task_count { get { return task_count; }
            set {
                task_count = value;
                TaskRun = new SemaphoreSlim(value, value);
            } }
        /// <summary>
        /// 开始页数
        /// </summary>
        public static int Start_numb { get; set; }

        /// <summary>
        /// 防止被封休眠毫秒数
        /// </summary>
        public static int WebSleep { get; set; }

        /// <summary>
        /// 结束页数
        /// </summary>
        public static int End_numb { get; set; }
        /// <summary>
        /// 网页地址
        /// </summary>
        public static string Url { get; set; }
        /// <summary>
        /// 网页内容类型
        /// </summary>
        public static int TypeId { get; set; }


        public static string Uname { get; set; }

        public static string Pwd { get; set; }

        


        /// <summary>
        /// 图片根目录
        /// </summary>
        public static string Img_path { get; set; }




        /// <summary>
        /// 文件存放地址
        /// </summary>
        /// <param name="index">该商品的第几张图片</param>
        /// <param name="sku">商品唯一编号</param>
        /// <param name="title">图片原始名字 例如:gwsf.jpg</param>
        /// <returns></returns>
        public static string GetMakeImgPath(int index, string newdir, string sku,string filename,string qianzhui="img_")
        {
            string extension = Path.GetExtension(filename);
            if(extension.Length==0)
            {
                Console.WriteLine(filename);
            }
            string fname = string.Format("{0}_{1}_{2}", sku, qianzhui,index);
            string filePath= Path.Combine(Img_path, string.Format("{0}/{1}{2}", newdir, fname, extension));

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            return filePath;

        }

        public static string GetItemInfoPath(string name,string newdir)
        {
            string filePath = Path.Combine(Img_path, string.Format("商品/{0}.txt", newdir));

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            return filePath;
        }

        ///// <summary>
        ///// 文件存放地址
        ///// </summary>
        ///// <param name="size"></param>
        ///// <param name="typeid"></param>
        ///// <param name="title"></param>
        ///// <returns></returns>
        //public static string GetMakeTorrentPath(string sku)
        //{
        //    string filePath= Path.Combine(Img_path, string.Format("shang_pin/{0}/shangpin.txt", sku));
        //    if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        //    {
        //        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        //    }
        //    return filePath;
        //}



        public static void openCurrentPath()
        {
            string path=Path.Combine(Config.ProcessDirectory, Config.Img_path);
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            System.Diagnostics.Process.Start("explorer", path);

        }

        


    }
}
