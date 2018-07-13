using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Panuon.UI
{
    public class PUWindow : Window
    {
        static PUWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUWindow), new FrameworkPropertyMetadata(typeof(PUWindow)));
        }

        private PUButton _btnClose;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var grdNavbar = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(this, 0), 0), 0), 0) as Grid;
            grdNavbar.MouseLeftButtonDown += delegate
            {
                this.DragMove();
            };
            _btnClose = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(grdNavbar, 1), 2) as PUButton;

        }

        #region Property

        /// <summary>
        /// 打开遮罩层，默认值为False。
        /// </summary>
        public bool ShowCover
        {
            get { return (bool)GetValue(ShowCoverProperty); }
            set { SetValue(ShowCoverProperty, value); }
        }
        public static readonly DependencyProperty ShowCoverProperty = DependencyProperty.Register("ShowCover", typeof(bool), typeof(PUWindow), new PropertyMetadata(false));

        /// <summary>
        /// 显示延迟，默认值为False。
        /// </summary>
        public bool ShowDelay
        {
            get { return (bool)GetValue(ShowDelayProperty); }
            set { SetValue(ShowDelayProperty, value); }
        }
        public static readonly DependencyProperty ShowDelayProperty = DependencyProperty.Register("ShowDelay", typeof(bool), typeof(PUWindow), new PropertyMetadata(false));



        /// <summary>
        /// 隐藏所有的控制栏右侧按钮。
        /// </summary>
        public bool HideNavButtons
        {
            get { return (bool)GetValue(HideNavButtonsProperty); }
            set { SetValue(HideNavButtonsProperty, value); }
        }
        public static readonly DependencyProperty HideNavButtonsProperty = DependencyProperty.Register("HideNavButtons", typeof(bool), typeof(PUWindow), new PropertyMetadata(false));


        /// <summary>
        /// 窗体打开时使用动画，默认值为true。
        /// </summary>
        public bool AnimateIn
        {
            get { return (bool)GetValue(AnimateInProperty); }
            set { SetValue(AnimateInProperty, value); }
        }
        public static readonly DependencyProperty AnimateInProperty = DependencyProperty.Register("AnimateIn", typeof(bool), typeof(PUWindow), new PropertyMetadata(true));

        /// <summary>
        /// 窗体关闭时使用动画，默认值为true。
        /// </summary>
        public bool AnimateOut
        {
            get { return (bool)GetValue(AnimateOutProperty); }
            set { SetValue(AnimateOutProperty, value); }
        }
        public static readonly DependencyProperty AnimateOutProperty = DependencyProperty.Register("AnimateOut", typeof(bool), typeof(PUWindow), new PropertyMetadata(true));

        /// <summary>
        /// 动画类型，默认值为Gradual（一个从上到下的渐变显示）。
        /// </summary>
        public AnimationStyles AnimationStyle
        {
            get { return (AnimationStyles)GetValue(AnimationStyleProperty); }
            set { SetValue(AnimationStyleProperty, value); }
        }
        public static readonly DependencyProperty AnimationStyleProperty = DependencyProperty.Register("AnimationStyle", typeof(AnimationStyles), typeof(PUWindow), new PropertyMetadata(AnimationStyles.Scale));

        /// <summary>
        /// 窗体圆角大小，默认值为0。
        /// </summary>
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(PUWindow), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// 左上角标题，默认值为null。
        /// <para>当该属性为null时，将采用Title属性来填充左上角标题。</para>
        /// </summary>
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(PUWindow), new PropertyMetadata(null));

        /// <summary>
        /// 左上角图标，默认值为null。
        /// </summary>
        public new object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public new static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(PUWindow), new PropertyMetadata(null));

        /// <summary>
        /// 控制栏背景色，默认值为White（白色）。
        /// </summary>
        public SolidColorBrush NavbarBackground
        {
            get { return (SolidColorBrush)GetValue(NavbarBackgroundProperty); }
            set { SetValue(NavbarBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavbarBackgroundProperty = DependencyProperty.Register("NavbarBackground", typeof(SolidColorBrush), typeof(PUWindow), new PropertyMetadata(new SolidColorBrush(Colors.White)));


        /// <summary>
        /// 控制栏高度，默认值为30。
        /// </summary>
        public double NavbarHeight
        {
            get { return (double)GetValue(NavbarHeightProperty); }
            set { SetValue(NavbarHeightProperty, value); }
        }
        public static readonly DependencyProperty NavbarHeightProperty = DependencyProperty.Register("NavbarHeight", typeof(double), typeof(PUWindow), new PropertyMetadata((double)30));

        /// <summary>
        /// 控制按钮高度，默认值为30。
        /// </summary>
        public double NavButtonHeight
        {
            get { return (double)GetValue(NavButtonHeightProperty); }
            set { SetValue(NavButtonHeightProperty, value); }
        }
        public static readonly DependencyProperty NavButtonHeightProperty = DependencyProperty.Register("NavButtonHeight", typeof(double), typeof(PUWindow), new PropertyMetadata((double)30));

        /// <summary>
        /// 控制按钮宽度，默认值为40。
        /// </summary>
        public double NavButtonWidth
        {
            get { return (double)GetValue(NavButtonWidthProperty); }
            set { SetValue(NavButtonWidthProperty, value); }
        }
        public static readonly DependencyProperty NavButtonWidthProperty = DependencyProperty.Register("NavButtonWidth", typeof(double), typeof(PUWindow), new PropertyMetadata((double)40));

        #endregion

        #region Command
        /// <summary>
        /// 关闭命令
        /// </summary>
        public ICommand CloseCommand
        {
            get
            { return _closeCommad; }
        }
        private ICommand _closeCommad = new CloseWindowCommand();

        /// <summary>
        /// 最大化命令
        /// </summary>
        public ICommand MaxCommand
        {
            get
            { return _maxCommand; }
        }
        private ICommand _maxCommand = new MaxWindowCommand();

        /// <summary>
        /// 最小化命令
        /// </summary>
        public ICommand MinCommand
        {
            get
            { return _minCommand; }
        }
        private ICommand _minCommand = new MinWindowCommand();

        #endregion

        public enum AnimationStyles
        {
            /// <summary>
            /// 缩放。
            /// </summary>
            Scale = 0,
            /// <summary>
            /// 一个从上到下的渐变显示。
            /// </summary>
            Gradual = 1,
            /// <summary>
            /// 渐入渐出。
            /// </summary>
            Fade = 2
        }

        #region APIs
        public void CloseWindow()
        {
            if (!AnimateOut)
                Close();
            else
            {
                _btnClose.RaiseEvent(new RoutedEventArgs(PUButton.ClickEvent, _btnClose));
                DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.4) };
                timer.Tick += delegate { Close(); timer.Stop(); };
                timer.Start();
            }
        }
        #endregion

    }

    public class CloseWindowCommand : ICommand
    {
        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var window = (parameter as PUWindow);
            if (!window.AnimateOut)
                window.Close();
            else
            {
                DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.4) };
                timer.Tick += delegate { window.Close(); timer.Stop(); };
                timer.Start();
            }
        }
    }
    public class MaxWindowCommand : ICommand
    {
        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var window = (parameter as PUWindow);
            if (window.WindowState == WindowState.Maximized)
                window.WindowState = WindowState.Normal;
            else
                window.WindowState = WindowState.Maximized;
        }
    }
    public class MinWindowCommand : ICommand
    {
        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            (parameter as PUWindow).WindowState = WindowState.Minimized;
        }
    }

}
