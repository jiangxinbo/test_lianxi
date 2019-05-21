using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tool;

namespace taobao.Tool
{
    public class FileManager
    {
        public static ParallelLoopResult SaveImageList(List<string>imglist,string sku,string name,string qianzhui="img_")
        {
            ParallelLoopResult result=Parallel.For(0, imglist.Count, (i) => {
                MemoryStream filestream = null;
                FileStream f = null;
                try
                {
                    filestream = Http.GetFile(imglist[i]);
                    if(filestream==null)
                    {
                        L.File.Warn("图片下载不下来" + imglist[i]);
                        return;
                    }
                    if(filestream.Length>1024*1024*2)
                    {
                        filestream = ImageMerge.MinImage(filestream, imglist[i], 20);
                        filestream = ImageMerge.YaSuoImage(filestream, imglist[i],98, 1024*2);
                    }
                    

                    f = new FileStream(Config.GetMakeImgPath(i, sku, name, imglist[i], qianzhui), FileMode.Create);
                    filestream.Position = 0;
                    byte[] rarbyte = new byte[filestream.Length];
                    filestream.Read(rarbyte, 0, (int)filestream.Length);
                    f.Write(rarbyte, 0, rarbyte.Length);
                }
                catch (Exception ex)
                {
                    L.File.WarnFormat("下载失败   sku={0}    url={1}    error={2}", sku, imglist[i], ex);
                    Console.Write("-" );
                }
                finally
                {
                    if (filestream != null)
                    {
                        filestream.Close();
                    }
                    if (f != null)
                    {
                        f.Flush();
                        f.Close();
                    }
                }
                Console.Write(".");
            });

            return result;
        }
    }
}
