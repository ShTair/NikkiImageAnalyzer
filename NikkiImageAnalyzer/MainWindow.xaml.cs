using Patagames.Ocr;
using Patagames.Ocr.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var bmp = new Bitmap(target))
            {
                using (var sp = new Bitmap(50, 1280))
                {
                    using (var g = Graphics.FromImage(sp))
                    {
                        DrowImage(g, bmp, )
                    }

                }
            }

            using (var api = OcrApi.Create())
            {
                api.Init(Languages.English);
                string plainText = api.GetTextFromImage();
                Console.WriteLine(plainText);
            }
        }

        private void DrowImage(Graphics dst, System.Drawing.Image src, int x1, int y1, int x2, int y2, int w, int h)
        {
            dst.DrawImage(src, new System.Drawing.Rectangle(x1, x2, w, h), new System.Drawing.Rectangle(x1, x2, w, h), GraphicsUnit.Pixel);
        }
    }
}
