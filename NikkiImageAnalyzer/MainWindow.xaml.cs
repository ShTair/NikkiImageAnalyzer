using Patagames.Ocr;
using Patagames.Ocr.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NikkiImageAnalyzer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private unsafe void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var bmp = new Bitmap("test.png"))
            {
                var bd = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                var scan = (byte*)bd.Scan0;
                var w = bmp.Width;
                byte* Scan(int x, int y, int offset) => scan + (y * w + x) * 4 + offset;

                var st = 320;
                for (; st < 1280; st++)
                {
                    if (*Scan(390, st, 0) + *Scan(390, st, 1) + *Scan(390, st, 2) < 510) break;
                }

                bmp.UnlockBits(bd);

                //390,320

                var sr = st - 149;


                using (var api = OcrApi.Create())
                {
                    api.Init(Languages.English);
                    api.ReadConfigFiles("tessconf.txt");

                    for (int i = sr; i < 1280; i += 171)
                    {
                        using (var sp = new Bitmap(35, 20))
                        {
                            using (var g = Graphics.FromImage(sp))
                            {
                                DrowImage(g, bmp, 139, i, 35, 20);
                            }

                            sp.Save("test2.png", ImageFormat.Png);

                            string plainText = api.GetTextFromImage(sp).Trim();
                            Console.WriteLine(plainText);
                        }
                    }
                }
            }
        }

        private void DrowImage(Graphics dst, System.Drawing.Image src, int x2, int y2, int w, int h)
        {
            dst.DrawImage(src, new System.Drawing.Rectangle(0, 0, w, h), new System.Drawing.Rectangle(x2, y2, w, h), GraphicsUnit.Pixel);
        }
    }
}
