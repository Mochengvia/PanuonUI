/*==============================================================
*作者：ZEOUN
*时间：2018/11/16 14:51:55
*说明： 
*日志：2018/11/16 14:51:55 创建。
*==============================================================*/


using Panuon.UI;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Panuon.UIBrowser.Views.Control.Examples
{
    /// <summary>
    /// ChatWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChatWindow : PUWindow
    {
        #region Identity
        private double _height = 40;
        #endregion

        public ChatWindow()
        {
            InitializeComponent();
            Init();
        }

        #region Event
        private async void btnSend_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (tbInput.Text == "")
                return;
            stkMain.Children.Add(GetBubbleGrid(tbInput.Text));
            tbInput.Text = "";
            await Task.Delay(500);
            stkMain.Children.Add(GetBubbleGrid("❤爱你哟！", true, true));
            svMain.ScrollToBottom();
        }

        private void btnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Function
        public void Init()
        {
            stkMain.Children.Add(GetBubbleGrid("欢迎使用PanuonUI ！", true, true));
        }

        private Grid GetBubbleGrid(string content, bool isTopOne = false, bool isSystemMsg = false)
        {
            var grd = new Grid()
            {
                Margin = isTopOne ? new System.Windows.Thickness(0, 20, 0, 0) : new System.Windows.Thickness(0, 10, 0, 0),
                MinHeight = _height,
                Opacity = 0,
            };

            var img = new Image()
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Panuon.UIBrowser;component/Resources/head_img.jpg")),
                Height = _height - 10,
                Width = _height - 10,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = isSystemMsg ? System.Windows.HorizontalAlignment.Left : System.Windows.HorizontalAlignment.Right,
                Margin = isSystemMsg ? new System.Windows.Thickness(5, 0, 0, 0) : new System.Windows.Thickness(0, 0, 10, 0),
            };

            grd.Children.Add(img);

            var text = new TextBlock()
            {
                TextWrapping = System.Windows.TextWrapping.Wrap,
                Text = content,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
            };

            var bubble = new PUBubble()
            {
                Margin = isSystemMsg ? new System.Windows.Thickness(_height + 5, 0, 0, 0) : new System.Windows.Thickness(0, 0, _height + 10, 0),
                AnglePosition = isSystemMsg ? AnglePositions.Left : AnglePositions.Right,
                Content = text,
                MinHeight = _height - 6,
                BorderCornerRadius = new System.Windows.CornerRadius(3),
                HorizontalAlignment = isSystemMsg ? System.Windows.HorizontalAlignment.Left : System.Windows.HorizontalAlignment.Right,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Background = new SolidColorBrush(((Color)ColorConverter.ConvertFromString("#FF49A9C0"))),
                CoverBrush = new SolidColorBrush(((Color)ColorConverter.ConvertFromString("#CC49A9C0"))),
                Padding = new System.Windows.Thickness(10, 0, 10, 0),
            };
            grd.Children.Add(bubble);

            var anima = new DoubleAnimation()
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(0.4),
            };

            var anima2 = new ThicknessAnimation()
            {
                To = isTopOne ? new System.Windows.Thickness(0, 10, 0, 0) : new System.Windows.Thickness(0, 0, 0, 0),
                Duration = TimeSpan.FromSeconds(0.2),
            };
            grd.BeginAnimation(OpacityProperty, anima);
            grd.BeginAnimation(MarginProperty, anima2);
            return grd;
        }
        #endregion


    }
}
