using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WPF_Sample.View
{
    /// <summary>
    /// ProgressWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ProgressWindow : Window
    {
        public ProgressWindow()
        {
            InitializeComponent();
        }

        private async void OnStartClick(object sender, RoutedEventArgs e)
        {
            try
            {
                btn.IsEnabled = false;

                // UIスレッドで実行したい処理を定義
                var p = new Progress<int>(i => progress.Value = i);

                await Task.Run(() => DoWork(p));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btn.IsEnabled = true;
            }
        }

        /// <summary>
        /// 非同期処理
        /// </summary>
        /// <param name="p"></param>
        private void DoWork(IProgress<int> p)
        {
            for (int i = 1; i <= 100; i++)
            {
                System.Threading.Thread.Sleep(100);

                // Progressの処理を実行
                p.Report(i);
            }
        }

    }
}
