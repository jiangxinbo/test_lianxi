using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using taobao;
using Tool;

namespace tb
{
    public class Class1
    {
        public static void r(string url)
        {
            run(url);

            ImageMerge.CompressImage(@"a.jpg", @"b.jpg", 100, 3000, true);
            
            runMin(url);
        }

        public static void run(string url)
        {
            MemoryStream filestream = null;
            FileStream f = null;

            try
            {
                filestream = Http.GetFile(url);
                
                //var tt = ImageMerge.MinImage(filestream, url);

                f = new FileStream("a.jpg", FileMode.Create);
                filestream.Position = 0;
                byte[] rarbyte = new byte[filestream.Length];
                filestream.Read(rarbyte, 0, (int)filestream.Length);
                f.Write(rarbyte, 0, rarbyte.Length);
            }
            catch (Exception ex)
            {
                Console.Write("-");
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
        }


        public static void runMin(string url)
        {
            MemoryStream filestream = null;
            FileStream f = null;

            try
            {
                filestream = Http.GetFile(url);

                filestream = ImageMerge.MinImage(filestream, url,70);

                f = new FileStream("c.jpg", FileMode.Create);
                filestream.Position = 0;
                byte[] rarbyte = new byte[filestream.Length];
                filestream.Read(rarbyte, 0, (int)filestream.Length);
                f.Write(rarbyte, 0, rarbyte.Length);
            }
            catch (Exception ex)
            {
                Console.Write("-");
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
        }
    }
}
