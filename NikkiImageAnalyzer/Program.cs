using Patagames.Ocr;
using Patagames.Ocr.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace NikkiImageAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = args[0];
            var targets = Directory.EnumerateFiles(path, "*.png");

            var c = 0;

            using (var api = OcrApi.Create())
            {
                api.Init(Languages.English);
                api.ReadConfigFiles("TessConf.txt");

                foreach (var item in targets)
                {
                    using (var src = new Bitmap(item))
                    {
                        var bmps = GetImages(src);
                        foreach (var tg in bmps)
                        {
                            using (tg)
                            {
                                tg.Save($"test\\test_{c++}.png");
                                var text = api.GetTextFromImage(tg).Trim();
                                Console.WriteLine(text);
                                break;
                            }
                        }
                    }
                }
            }

            Console.ReadLine();
        }

        private static IEnumerable<Bitmap> GetImages(Bitmap src)
        {
            unsafe int GetStart()
            {
                var bd = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                var scan = (byte*)bd.Scan0;
                var w = src.Width;
                byte* Scan(int x, int y, int offset) => scan + (y * w + x) * 4 + offset;

                var s = 600;

                bool f = false;
                var st = 320;
                for (; st < 1280; st++)
                {
                    var v = *Scan(390, st, 0) + *Scan(390, st, 1) + *Scan(390, st, 2);

                    if (v > s)
                    {
                        f = true;
                    }
                    else if (f)
                    {
                        if (*Scan(390, st + 4, 0) + *Scan(390, st + 4, 1) + *Scan(390, st + 4, 2) > s) break;
                        f = false;
                    }
                }

                //for (int y = 0; y < src.Height; y++)
                //{
                //    for (int x = 0; x < src.Width; x++)
                //    {
                //        var v = *Scan(x, y, 0) + *Scan(x, y, 1) + *Scan(x, y, 2);
                //        if (v > s) *Scan(x, y, 0) = *Scan(x, y, 1) = *Scan(x, y, 2) = 255;
                //        else *Scan(x, y, 0) = *Scan(x, y, 1) = *Scan(x, y, 2) = 0;
                //    }
                //}
                src.UnlockBits(bd);

                //src.Save("test2.png");

                return st;
            }

            var sr = GetStart() - 149;
            for (int i = sr; i < 1250; i += 171)
            {
                var sp1 = new Bitmap(40, 20);
                using (var g = Graphics.FromImage(sp1))
                {
                    DrowImage(g, src, 134, i, 40, 20);
                }

                yield return sp1;

                var sp2 = new Bitmap(40, 20);
                using (var g = Graphics.FromImage(sp2))
                {
                    DrowImage(g, src, 433, i, 40, 20);
                }

                yield return sp2;
            }
        }

        private static void DrowImage(Graphics dst, Bitmap src, int x2, int y2, int w, int h)
        {
            dst.DrawImage(src, new Rectangle(0, 0, w, h), new Rectangle(x2, y2, w, h), GraphicsUnit.Pixel);
        }
    }
}
