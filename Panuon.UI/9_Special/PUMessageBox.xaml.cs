using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Panuon.UI
{
    /// <summary>
    /// PUMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class PUMessageBox : UI.PUWindow
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        private PUWindow _parentWindow;
        private PUMessageBox(string title, object content, bool showInTaskBar, AnimationStyles animateStyle)
        {
            InitializeComponent();
            Title = title;
            lblContent.Content = content;
            _parentWindow = GetOwnerWindow() ;
            _parentWindow.ShowCover = true;
            ShowInTaskbar = showInTaskBar;
            AnimationStyle = animateStyle;
            Owner = _parentWindow;
        }


        #region APIs

        /// <summary>
        /// 打开对话框（主窗体必须是PUWindow类型。），并打开遮罩层。
        /// </summary>
        /// <param name="content">要显示的内容。</param>
        /// <param name="title">标题内容。</param>
        /// <param name="showInTaskBar">是否在任务栏中显示，默认为True。</param>
        public static void ShowDialog(object content, string title = "提示", bool showInTaskBar = true, AnimationStyles animateStyle = AnimationStyles.Scale)
        {
            var mbox = new PUMessageBox(title, content, showInTaskBar, animateStyle);
            if (!showInTaskBar)
                mbox.ShowInTaskbar = false;
            mbox.ShowDialog();
        }

        #endregion

        #region Function
        private static PUWindow GetWindowFromHwnd(IntPtr hwnd)
        {
            var visual = HwndSource.FromHwnd(hwnd).RootVisual;
            return visual as PUWindow;
        }
        private static PUWindow GetOwnerWindow()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == null)
                return null;

            return GetWindowFromHwnd(hwnd);
        }

        #endregion

        private void PUButton_Click(object sender, RoutedEventArgs e)
        {
            _parentWindow.ShowCover = false;
            CloseWindow();
        }
    }
}
