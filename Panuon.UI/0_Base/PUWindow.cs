using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Panuon.UI
{
    public class PUWindow : Window
    {
        #region Identify
        private PUButton _btnClose;
        #endregion

        static PUWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUWindow), new FrameworkPropertyMetadata(typeof(PUWindow)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Loaded += delegate
            {
                if (AnimateIn)
                    OnBeginLoadStoryboard();
                else
                    OnSkipLoadStoryboard();
            };
            var grdNavbar = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(this,0), 0), 0), 0) as Grid;
            grdNavbar.MouseLeftButtonDown += delegate
            {
                this.DragMove();
            };
            _btnClose = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(grdNavbar, 1), 2) as PUButton;
        }

        private bool _animateOutHandle = true;
        protected override void OnClosing(CancelEventArgs e)
        {
            if(AnimateOut && _animateOutHandle)
            {
                OnBeginCloseStoryboard();
                _animateOutHandle = false;
                DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.4) };
                timer.Tick += delegate { Close(); timer.Stop(); };
                timer.Start();

                e.Cancel = true;
            }
            base.OnClosing(e);
        }
        #region RoutedEvent
        /// <summary>
        /// 使用动画打开窗体。
        /// </summary>
        internal static readonly RoutedEvent BeginLoadStoryboardEvent = EventManager.RegisterRoutedEvent("BeginLoadStoryboard", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<PUWindow>), typeof(PUWindow));
        internal event RoutedPropertyChangedEventHandler<PUWindow> BeginLoadStoryboard
        {
            add { AddHandler(BeginLoadStoryboardEvent, value); }
            remove { RemoveHandler(BeginLoadStoryboardEvent, value); }
        }
        internal void OnBeginLoadStoryboard()
        {
            RoutedPropertyChangedEventArgs<PUWindow> arg = new RoutedPropertyChangedEventArgs<PUWindow>(null, this, BeginLoadStoryboardEvent);
            RaiseEvent(arg);
        }

        /// <summary>
        /// 不使用动画打开窗体。
        /// </summary>
        internal static readonly RoutedEvent SkipLoadStoryboardEvent = EventManager.RegisterRoutedEvent("SkipLoadStoryboard", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<PUWindow>), typeof(PUWindow));
        internal event RoutedPropertyChangedEventHandler<PUWindow> SkipLoadStoryboard
        {
            add { AddHandler(SkipLoadStoryboardEvent, value); }
            remove { RemoveHandler(SkipLoadStoryboardEvent, value); }
        }
        internal void OnSkipLoadStoryboard()
        {
            RoutedPropertyChangedEventArgs<PUWindow> arg = new RoutedPropertyChangedEventArgs<PUWindow>(null, this, SkipLoadStoryboardEvent);
            RaiseEvent(arg);
        }

        /// <summary>
        /// 使用动画关闭窗体。
        /// </summary>
        internal static readonly RoutedEvent BeginCloseStoryboardEvent = EventManager.RegisterRoutedEvent("BeginCloseStoryboard", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<PUWindow>), typeof(PUWindow));
        internal event RoutedPropertyChangedEventHandler<PUWindow> BeginCloseStoryboard
        {
            add { AddHandler(BeginCloseStoryboardEvent, value); }
            remove { RemoveHandler(BeginCloseStoryboardEvent, value); }
        }
        internal void OnBeginCloseStoryboard()
        {
            RoutedPropertyChangedEventArgs<PUWindow> arg = new RoutedPropertyChangedEventArgs<PUWindow>(null, this, BeginCloseStoryboardEvent);
            RaiseEvent(arg);
        }
       
        #endregion

        #region Property

        /// <summary>
        /// 打开遮罩层，默认值为False。
        /// </summary>
        public bool IsCoverMaskShow
        {
            get { return (bool)GetValue(IsCoverMaskShowProperty); }
            set { SetValue(IsCoverMaskShowProperty, value); }
        }
        public static readonly DependencyProperty IsCoverMaskShowProperty = 
            DependencyProperty.Register("IsCoverMaskShow", typeof(bool), typeof(PUWindow), new PropertyMetadata(false));

        /// <summary>
        /// 显示延迟，默认值为False。
        /// </summary>
        public bool AllowShowDelay
        {
            get { return (bool)GetValue(AllowShowDelayProperty); }
            set { SetValue(AllowShowDelayProperty, value); }
        }
        public static readonly DependencyProperty AllowShowDelayProperty = 
            DependencyProperty.Register("AllowShowDelay", typeof(bool), typeof(PUWindow), new PropertyMetadata(false));

        /// <summary>
        /// 隐藏所有的控制栏右侧按钮。
        /// </summary>
        public Visibility NavButtonVisibility
        {
            get { return (Visibility)GetValue(NavButtonVisibilityProperty); }
            set { SetValue(NavButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty NavButtonVisibilityProperty = 
            DependencyProperty.Register("NavButtonVisibility", typeof(Visibility), typeof(PUWindow), new PropertyMetadata(Visibility.Visible));


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
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public static readonly DependencyProperty HeaderProperty = 
            DependencyProperty.Register("Header", typeof(object), typeof(PUWindow), new PropertyMetadata());

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
        public Brush NavbarBackground
        {
            get { return (Brush)GetValue(NavbarBackgroundProperty); }
            set { SetValue(NavbarBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavbarBackgroundProperty =
            DependencyProperty.Register("NavbarBackground", typeof(Brush), typeof(PUWindow), new PropertyMetadata(new SolidColorBrush(Colors.White)));

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
        public static readonly DependencyProperty NavButtonWidthProperty = 
            DependencyProperty.Register("NavButtonWidth", typeof(double), typeof(PUWindow), new PropertyMetadata((double)40));

        /// <summary>
        /// 是否打开遮罩层并显示等待控件。
        /// </summary>
        public bool IsAwaitShow
        {
            get { return (bool)GetValue(IsAwaitShowProperty); }
            set { SetValue(IsAwaitShowProperty, value); }
        }

        public static readonly DependencyProperty IsAwaitShowProperty =
            DependencyProperty.Register("IsAwaitShow", typeof(bool), typeof(PUWindow));




        #endregion

        #region Internal APIs

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
            window.Close();
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
