using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Panuon.UI
{
    /// <summary>
    /// PUMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class PUMessageBox : UI.PUWindow
    {
        #region Identity
        RoutedEventHandler _cancel;

        static PUMessageBox _instance;
        #endregion

        private PUMessageBox(string title, string content, bool isConfirm, bool showInTaskBar, AnimationStyles animateStyle)
        {
            InitializeComponent();
            Title = title;
            txtContent.Text = content;
            if (isConfirm)
            {
                groupTip.Visibility = Visibility.Collapsed;
                groupConfirm.Visibility = Visibility.Visible;
            }
            ShowInTaskbar = showInTaskBar;
            AnimationStyle = animateStyle;
        }
        #region APIs
        /// <summary>
        /// 打开一个消息提示对话框，并打开父窗体的遮罩层。
        /// </summary>
        /// <param name="content">要显示的内容。</param>
        /// <param name="title">标题内容。</param>
        /// <param name="buttons">按钮内容，默认为“好”</param>
        /// <param name="showInTaskBar">是否在任务栏中显示，默认为True。</param>
        public static void ShowDialog(string content, string title = "提示", Buttons buttons = Buttons.Sure, bool showInTaskBar = true, AnimationStyles animateStyle = AnimationStyles.Scale)
        {
            var mbox = new PUMessageBox(title, content, false, showInTaskBar, animateStyle);
            mbox.CheckButtonContent(buttons);
            if (!showInTaskBar)
                mbox.ShowInTaskbar = false;
            mbox.ShowDialog();
        }

        /// <summary>
        /// 打开一个消息确认对话框，并打开父窗体的遮罩层。
        /// </summary>
        /// <param name="content">要显示的内容。</param>
        /// <param name="title">标题内容。</param>
        /// <param name="buttons">按钮内容，默认为“是/否”</param>
        /// <param name="showInTaskBar">是否在任务栏中显示，默认为True。</param>
        public static bool? ShowConfirm(string content, string title = "提示", Buttons buttons = Buttons.YesOrNo, bool showInTaskBar = true, AnimationStyles animateStyle = AnimationStyles.Scale)
        {
            var mbox = new PUMessageBox(title, content, true, showInTaskBar, animateStyle);
            mbox.CheckButtonContent(buttons);
            if (!showInTaskBar)
                mbox.ShowInTaskbar = false;
            mbox.ShowDialog();
            return mbox.DialogResult;
        }


        /// <summary>
        /// 打开一个等待界面，并打开父窗体的遮罩层。该界面将以Show的方式打开，但用户不能使用Alt+F4强制关闭此页面。若要关闭此界面，请调用PUMessageBox.CloseAwait()方法。
        /// </summary>
        /// <param name="content">要显示的内容</param>
        public static void ShowAwait(string content)
        {
            var mbox = new PUMessageBox("", "", false, false, AnimationStyles.Scale);
            mbox.AllowForcingClose = false;
            _instance = mbox;
            mbox.txtAwait.Text = content;
            mbox.btnOK.IsEnabled = false;
            mbox.CheckButtonContent(Buttons.Cancel);
            mbox.Topmost = true;
            mbox.loading.IsRunning = true;
            mbox.grdAwait.Visibility = Visibility.Visible;
            mbox.Show();
        }

        /// <summary>
        /// 打开一个等待界面，并打开父窗体的遮罩层。该界面将以Show的方式打开，但用户不能使用Alt+F4强制关闭此页面。若要关闭此界面，请调用PUMessageBox.CloseAwait()方法。
        /// </summary>
        /// <param name="content">要显示的内容</param>
        /// <param name="cancelCallback">若允许用户取消等待，则必须指定点击取消按钮后的后续处理。用户点击了取消按钮，该窗体需要您手动关闭。若不指定后续处理，取消按钮将被禁用。</param>
        public static void ShowAwait(string content, RoutedEventHandler cancelCallback = null)
        {
            var mbox = new PUMessageBox("", "", false, false, AnimationStyles.Scale);
            mbox.AllowForcingClose = false;
            _instance = mbox;
            mbox.txtAwait.Text = content;
            mbox.CheckButtonContent(Buttons.Cancel);
            mbox._cancel = cancelCallback;
            mbox.Topmost = true;
            mbox.loading.IsRunning = true;
            mbox.grdAwait.Visibility = Visibility.Visible;
            mbox.Show();
        }

        /// <summary>
        /// 打开一个等待界面，并打开父窗体的遮罩层。该界面将以Show的方式打开，但用户不能使用Alt+F4强制关闭此页面。若要关闭此界面，请调用PUMessageBox.CloseAwait()方法。
        /// </summary>
        /// <param name="content">要显示的内容</param>
        /// <param name="title">标题内容。</param>
        /// <param name="cancelCallback">若允许用户取消等待，则必须指定点击取消按钮后的后续处理。用户点击了取消按钮，该窗体需要您手动关闭。若不指定后续处理，取消按钮将被禁用。</param>
        public static void ShowAwait(string content, string title = "提示", RoutedEventHandler cancelCallback = null, AnimationStyles animateStyle = AnimationStyles.Scale)
        {
            var mbox = new PUMessageBox(title, "", false, false, animateStyle);
            mbox.AllowForcingClose = false;
            _instance = mbox;
            mbox.CheckButtonContent(Buttons.Cancel);
            mbox._cancel = cancelCallback;
            mbox.Topmost = true;
            mbox.grdAwait.Visibility = Visibility.Visible;
            mbox.loading.IsRunning = true;
            mbox.txtAwait.Text = content;
            mbox.Show();
        }

        /// <summary>
        /// 尝试关闭最后打开的一个等待界面。
        /// 若要在其关闭之后立即打开另一个PUMessageBox，请使用另一个重载方法，或等待400ms后再打开。
        /// </summary>
        public static void CloseAwait()
        {
            if (_instance != null)
            {
                _instance.Closed += delegate
                {
                    _instance = null;
                };

                _instance.Close();
            }
        }

        /// <summary>
        /// 尝试关闭最后打开的一个等待界面。
        /// 若要关闭之后立即打开另一个PUMessageBox，请指定关闭事件后的回调处理。
        /// <param name="closedCallback"></param>
        public static void CloseAwait(EventHandler closedCallback)
        {
            if (_instance != null)
            {
                _instance.Closed += delegate
                {
                    _instance = null;
                    closedCallback(null, null);
                };

                _instance.Close();
            }
        }
        #endregion

        #region Sys

        private void CheckButtonContent(Buttons buttons)
        {
            switch (buttons)
            {
                case Buttons.Sure:
                    btnOK.Content = "好";
                    break;
                case Buttons.Yes:
                    btnOK.Content = "是";
                    break;
                case Buttons.OK:
                    btnOK.Content = "确定";
                    break;
                case Buttons.Cancel:
                    btnOK.Content = "取消";
                    break;
                case Buttons.YesOrNo:
                    BtnYes.Content = "是";
                    BtnNo.Content = "否";
                    break;
                case Buttons.YesOrCancel:
                    BtnYes.Content = "是";
                    BtnNo.Content = "取消";
                    break;
                case Buttons.OKOrCancel:
                    BtnYes.Content = "确定";
                    BtnNo.Content = "取消";
                    break;
                case Buttons.AcceptOrRefused:
                    BtnYes.Content = "接受";
                    BtnNo.Content = "拒绝";
                    break;
                case Buttons.AcceptOrCancel:
                    BtnYes.Content = "接受";
                    BtnNo.Content = "取消";
                    break;

            }
        }

        private void PUButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cancel != null)
            {
                txtAwait.Text = "正在取消";
                btnOK.IsEnabled = false;
                loading.IsRunning = false;
                _cancel(null, null);
            }
            else
            {
                Close();
            }
        }

        private void PUButtonYes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void PUButtonNo_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        #endregion

    }
}
