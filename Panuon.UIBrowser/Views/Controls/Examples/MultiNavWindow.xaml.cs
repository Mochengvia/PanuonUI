/*==============================================================
*作者：ZEOUN
*时间：2018/11/16 9:32:52
*说明： 
*日志：2018/11/16 9:32:52 创建。
*==============================================================*/

using Panuon.UI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UIBrowser.Views.Control.Examples
{
    /// <summary>
    /// MultiNavWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MultiNavWindow : PUWindow
    {
        public MultiNavWindow()
        {
            InitializeComponent();
            LoadNavButtons();
            Result = 0;
        }

        #region Event
        public void LoadNavButtons()
        {
            var lbl1 = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = "",
                FontFamily = FindResource("IconFont") as FontFamily,
            };
            var lbl2 = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = "",
                FontFamily = FindResource("IconFont") as FontFamily,
            };
            AppendNavButton("H", new RoutedEventHandler((s, e) => { PUMessageBox.ShowDialog("你点击了第三个按钮!"); Result = 3; }));
            AppendNavButton(lbl1, new RoutedEventHandler((s, e) => { PUMessageBox.ShowDialog("你点击了第二个按钮!"); Result = 2; }));
            AppendNavButton(lbl2, new RoutedEventHandler((s, e) => { PUMessageBox.ShowDialog("你点击了第一个按钮!"); Result = 1; }));
        }
        #endregion
    }
}
