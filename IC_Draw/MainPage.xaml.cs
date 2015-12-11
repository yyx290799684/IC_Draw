using IC_Draw.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace IC_Draw
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        static int finshnum = 300; //总人数
        //List<string> usenum = new List<string>();//已经抽到过的人
        static bool isdorand = false;
        public ObservableCollection<NumList> numlist = new ObservableCollection<NumList>();

        public MainPage()
        {
            this.InitializeComponent();
            numListView.ItemsSource = numlist;
            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize.Width > 600)
                    historyLine.Y2 = e.NewSize.Height;
            };
        }

        private void startbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (finshnumTextBox.Text != "")
                    finshnum = int.Parse(finshnumTextBox.Text);
            }
            catch (Exception) {}
          
            isdorand = true;
            doRand();
            finshbutton.IsEnabled = true;
            startbutton.IsEnabled = false;
        }

        private async void doRand()
        {
            Random r = new Random();
            var numlistarray = numlist.ToList();
            while (isdorand)
            {
                string temp = await getRand(r);
                NumList s = numlistarray.Find(p => p.num.Equals(temp));
                Debug.WriteLine(s);
                if (s == null)
                {
                    numTextBlock1.Text = temp.Substring(0, 1);
                    numTextBlock2.Text = temp.Substring(1, 1);
                    numTextBlock3.Text = temp.Substring(2, 1);
                }
                await Task.Delay(50);
            }
            numlist.Insert(0,new NumList { num = numTextBlock1.Text + numTextBlock2.Text + numTextBlock3.Text });

        }

        private void finshbutton_Click(object sender, RoutedEventArgs e)
        {
            isdorand = false;
            //numlist.Add(new NumList { num = numTextBlock1.Text + numTextBlock2.Text + numTextBlock3.Text });
            finshbutton.IsEnabled = false;
            startbutton.IsEnabled = true;
        }



        public static async Task<string> getRand(Random r)
        {
            return await Task.Run(() =>
           {
               int temp = r.Next(1, MainPage.finshnum);
               if (temp < 10)
                   return "00" + temp.ToString();
               else if (temp < 100)
                   return "0" + temp.ToString();
               else
                   return temp.ToString();
           });
        }


    }
}
