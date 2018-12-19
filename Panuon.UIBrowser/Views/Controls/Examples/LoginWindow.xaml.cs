/*==============================================================
*作者：ZEOUN
*时间：2018/11/16 14:11:41
*说明： 
*日志：2018/11/16 14:11:41 创建。
*==============================================================*/

using Panuon.UI;
using System.Windows;

namespace Panuon.UIBrowser.Views.Control.Examples
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : PUWindow
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
