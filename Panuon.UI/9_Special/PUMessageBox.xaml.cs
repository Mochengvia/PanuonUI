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
        private PUMessageBox(string title, string content,bool isConfirm, bool showInTaskBar, AnimationStyles animateStyle)
        {
            InitializeComponent();
            Title = title;
            txtContent.Text = content;
            _parentWindow = GetOwnerWindow() ;
            if(_parentWindow != null)
                _parentWindow.IsCoverMaskShow =  true;
            if(isConfirm)
            {
                groupTip.Visibility = Visibility.Collapsed;
                groupConfirm.Visibility = Visibility.Visible;
            }
            ShowInTaskbar = showInTaskBar;
            AnimationStyle = animateStyle;
            if (_parentWindow != null)
                Owner = _parentWindow;
        }
        #region APIs
        /// <summary>
        /// 打开一个消息提示对话框，并打开父窗体的遮罩层。
        /// </summary>
        /// <param name="content">要显示的内容。</param>
        /// <param name="title">标题内容。</param>
        /// <param name="showInTaskBar">是否在任务栏中显示，默认为True。</param>
        public static void ShowDialog(string content, string title = "提示", bool showInTaskBar = true, AnimationStyles animateStyle = AnimationStyles.Scale)
        {
            var mbox = new PUMessageBox(title, content,false, showInTaskBar, animateStyle);
            if (!showInTaskBar)
                mbox.ShowInTaskbar = false;
            mbox.ShowDialog();
        }

        /// <summary>
        /// 打开一个消息确认对话框，并打开父窗体的遮罩层。
        /// </summary>
        /// <param name="content">要显示的内容。</param>
        /// <param name="title">标题内容。</param>
        /// <param name="showInTaskBar">是否在任务栏中显示，默认为True。</param>
        public static bool? ShowConfirm(string content, string title = "提示", bool showInTaskBar = true, AnimationStyles animateStyle = AnimationStyles.Scale)
        {
            var mbox = new PUMessageBox(title, content,true, showInTaskBar, animateStyle);
            if (!showInTaskBar)
                mbox.ShowInTaskbar = false;
            mbox.ShowDialog();
            return mbox.DialogResult;
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
            _parentWindow.IsCoverMaskShow = false;
            CloseWindow();
        }

        private void PUButtonYes_Click(object sender, RoutedEventArgs e)
        {
            _parentWindow.IsCoverMaskShow = false;
            DialogResult = true;
            CloseWindow();
        }

        private void PUButtonNo_Click(object sender, RoutedEventArgs e)
        {
            _parentWindow.IsCoverMaskShow = false;
            DialogResult = false;
            CloseWindow();
        }
    }
}
