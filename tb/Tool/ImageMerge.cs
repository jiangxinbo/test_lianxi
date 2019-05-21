using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Tool
{
    public class ImageMerge
    {
        /// <summary>
        /// 合并图片
        /// </summary>
        /// <param name="streams">图片流</param>
        /// <param name="minWidth">图片最小宽度</param>
        /// <returns></returns>
        public static  MemoryStream Merge(List<Stream> streams,int minWidth=1500)
        {
            List<Image> imglist = new List<Image>();
            int maxWidth = minWidth;
            foreach (var streamitem in streams)
            {
                Image img = Image.FromStream(streamitem);
                maxWidth = maxWidth < img.Width ? img.Width : maxWidth;
                imglist.Add(img);
            }
            int drawX = 0;//绘图高度
            int drawY = 0;//绘图宽度
            int rowheight = 0; //当前行高
            foreach (var item in imglist)
            {
                if (drawX + item.Width <= maxWidth)
                {
                    drawX += item.Width;
                    if (item.Height > rowheight)
                        rowheight = item.Height;
                }
                else
                {
                    drawY = rowheight;
                    rowheight += item.Height;
                    drawX = item.Width;
                }
            }
            Bitmap b = new Bitmap(maxWidth, rowheight);
            var g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

            drawX = 0;//绘图高度
            drawY = 0;//绘图宽度
            rowheight = 0; //当前最大行高
            foreach (var item in imglist)
            {
                if (drawX + item.Width <= maxWidth)
                {
                    //g.DrawImage(item, drawX, drawY, item.Width, item.Height);
                    g.DrawImageUnscaled(item, drawX, drawY, item.Width, item.Height);
                    drawX += item.Width;
                    if (item.Height > rowheight)
                        rowheight = item.Height;
                }
                else
                {
                    drawY = rowheight;
                    rowheight += item.Height;
                    drawX = 0;
                    g.DrawImage(item, drawX, drawY, item.Width, item.Height);
                    drawX += item.Width;
                }
            }
            var myImageCodecInfo = GetEncoderInfo("image/jpeg");
            // 基于GUID创建一个Encoder对象
            var myEncoder = Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            MemoryStream stream = new MemoryStream();
            try
            {
                b.Save(stream, myImageCodecInfo, myEncoderParameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine("合并图片错误"+ex.Message);
                b.Dispose();
                g.Dispose();
                throw;
            }
            finally
            {
                b.Dispose();
                g.Dispose();
            }
            b.Dispose();
            g.Dispose();
            return stream;
        }

        public static MemoryStream Merge(List<Image> imglist, int minWidth = 1500)
        {
            int maxWidth = minWidth;
            foreach (var img in imglist)
            {
                maxWidth = maxWidth < img.Width ? img.Width : maxWidth;
            }
            int drawX = 0;//绘图高度
            int drawY = 0;//绘图宽度
            int rowheight = 0; //当前行高
            foreach (var item in imglist)
            {
                if (drawX + item.Width <= maxWidth)
                {
                    drawX += item.Width;
                    if (item.Height > rowheight)
                        rowheight = item.Height;
                }
                else
                {
                    drawY = rowheight;
                    rowheight += item.Height;
                    drawX = item.Width;
                }
            }
            Bitmap b = new Bitmap(maxWidth, rowheight);
            var g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

            drawX = 0;//绘图高度
            drawY = 0;//绘图宽度
            rowheight = 0; //当前最大行高
            foreach (var item in imglist)
            {
                if (drawX + item.Width <= maxWidth)
                {
                    g.DrawImage(item, drawX, drawY, item.Width, item.Height);
                    //g.DrawImageUnscaled(item, drawX, drawY, item.Width, item.Height);
                    drawX += item.Width;
                    if (item.Height > rowheight)
                        rowheight = item.Height;
                }
                else
                {
                    drawY = rowheight;
                    rowheight += item.Height;
                    drawX = 0;
                    g.DrawImage(item, drawX, drawY, item.Width, item.Height);
                    drawX += item.Width;
                }
            }
            var myImageCodecInfo = GetEncoderInfo("image/jpeg");
            // 基于GUID创建一个Encoder对象
            //用于Quality参数类别。
            var myEncoder = Encoder.Quality;

            //创建一个EncoderParameters对象。
            // EncoderParameters对象有一个EncoderParameter数组
            //对象 在这种情况下，只有一个
            //数组中的EncoderParameter对象。
            var myEncoderParameters = new EncoderParameters(1);
            //将位图保存为质量级别为100的JPEG文件。
            var myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            MemoryStream stream = new MemoryStream();
            b.Save(stream, myImageCodecInfo, myEncoderParameters);
            b.Dispose();
            g.Dispose();
            return stream;
        }

        /// <summary>
        /// 合并后保存图片
        /// </summary>
        /// <param name="imglist">图片列表</param>
        /// <param name="fileName">完整文件名</param>
        /// <param name="minWidth">最小宽度</param>
        public static void MergeToSaveFile(List<Image> imglist,string fileName, int minWidth = 1500)
        {
            Image img = Image.FromStream(Merge(imglist, minWidth));
            img.Save(fileName);
            //img.Dispose();
        }


        public static MemoryStream MinImage(Stream stream, string name, long zhiliang = 100)
        {
            MemoryStream newImageStream = new MemoryStream();
            var contentTypDict = new Dictionary<string, string> {
                {"jpg","image/jpeg"},
                {"jpeg","image/jpeg"},
                {"jpe","image/jpeg"},
                {"png","image/png"},
                {"gif","image/gif"},
                {"ico","image/x-ico"},
                {"tif","image/tiff"},
                {"tiff","image/tiff"},
                {"fax","image/fax"},
                {"wbmp","image//vnd.wap.wbmp"},
                {"rp","image/vnd.rn-realpix"}
            };
            using (Image originImage = Image.FromStream(stream))
            {
                using (var bitmap = new Bitmap(originImage.Width, originImage.Height))
                {
                    using (var graphic = Graphics.FromImage(bitmap))
                    {
                        //graphic.Clear(Color.WhiteSmoke);
                        //graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        //graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                        graphic.InterpolationMode = InterpolationMode.High;
                        graphic.SmoothingMode = SmoothingMode.HighQuality;
                        graphic.CompositingQuality = CompositingQuality.HighQuality;
                        graphic.DrawImage(originImage, 0, 0, originImage.Width, originImage.Height);
                        using (var encoderParameters = new EncoderParameters(1))
                        {
                            using (var encoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, zhiliang))
                            {
                                var mimeType = "image/jpeg";
                                var imgTypeSplit = name.Split('.');
                                var imgType = imgTypeSplit[imgTypeSplit.Length - 1].ToLower();
                                //未知的图片类型
                                if (!contentTypDict.ContainsKey(imgType))
                                {
                                    mimeType = "image/jpeg";
                                }
                                else
                                {
                                    mimeType = contentTypDict[imgType];
                                }

                                encoderParameters.Param[0] = encoderParameter;
                                bitmap.Save(newImageStream,
                                            ImageCodecInfo.GetImageEncoders().Where(x => x.MimeType.Equals(mimeType, StringComparison.OrdinalIgnoreCase)).FirstOrDefault(),
                                            encoderParameters);
                                //if(newImageStream.Length>1024*2*1024)
                                //{
                                //    zhiliang= zhiliang-2;
                                //    return MinImage(stream, name, zhiliang);
                                //}
                            }
                        }
                    }
                }
            }
            return newImageStream;
        }

        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="sFile">Stream</param>
        /// <param name="name">图片名称或图片网址</param>
        /// <param name="flag">压缩质量（数字越小压缩率越高）1-100</param>
        /// <param name="size">压缩后图片的最大大小 k</param>
        /// <returns></returns>
        public static MemoryStream YaSuoImage(Stream sFile, string name, int flag = 100, int size = 1024*3)
        {
            var contentTypDict = new Dictionary<string, string> {
                {"jpg","image/jpeg"},
                {"jpeg","image/jpeg"},
                {"jpe","image/jpeg"},
                {"png","image/png"},
                {"gif","image/gif"},
                {"ico","image/x-ico"},
                {"tif","image/tiff"},
                {"tiff","image/tiff"},
                {"fax","image/fax"},
                {"wbmp","image//vnd.wap.wbmp"},
                {"rp","image/vnd.rn-realpix"}
            };
            var mimeType = "image/jpeg";
            var imgTypeSplit = name.Split('.');
            var imgType = imgTypeSplit[imgTypeSplit.Length - 1].ToLower();
            //未知的图片类型
            if (!contentTypDict.ContainsKey(imgType))
            {
                mimeType = "image/jpeg";
            }
            else
            {
                mimeType = contentTypDict[imgType];
            }

            Image iSource = Image.FromStream(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int dHeight = iSource.Height;
            int dWidth = iSource.Width;
            int sW = 0, sH = 0;
            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);
            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();

            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            MemoryStream newImageStream = new MemoryStream();
            try
            {
                ImageCodecInfo jpegICIinfo = ImageCodecInfo.GetImageEncoders().Where(x => x.MimeType.Equals(mimeType, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (jpegICIinfo != null)
                {
                    ob.Save(newImageStream, jpegICIinfo, ep);//dFile是压缩后的新路径
                    
                    if (newImageStream.Length > 1024 * size)
                    {
                        flag = flag - 2;
                        return YaSuoImage(sFile, name, flag, size);
                    }
                }
                else
                {
                    ob.Save(newImageStream, tFormat);
                }
                return newImageStream;
            }
            catch
            {
                return newImageStream;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }


        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="sFile">原图片地址</param>
        /// <param name="dFile">压缩后保存图片地址</param>
        /// <param name="flag">压缩质量（数字越小压缩率越高）1-100</param>
        /// <param name="size">压缩后图片的最大大小</param>
        /// <param name="sfsc">是否是第一次调用</param>
        /// <returns></returns>
        public static bool CompressImage(string sFile, string dFile, int flag = 1100, int size = 300, bool sfsc = true)
        {
            //如果是第一次调用，原始图像的大小小于要压缩的大小，则直接复制文件，并且返回true
            FileInfo firstFileInfo = new FileInfo(sFile);
            if (sfsc == true && firstFileInfo.Length < size * 1024)
            {
                firstFileInfo.CopyTo(dFile,true);
                return true;
            }
            Image iSource = Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int dHeight = iSource.Height ;
            int dWidth = iSource.Width;
            int sW = 0, sH = 0;
            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);
            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();

            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;

            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                    FileInfo fi = new FileInfo(dFile);
                    if (fi.Length > 1024 * size)
                    {
                        flag = flag - 2;
                        CompressImage(sFile, dFile, flag, size, false);
                    }
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }



       

    }
}
