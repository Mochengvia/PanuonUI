using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Panuon.UI
{
    public class PUWindow : Window
    {
        #region Import
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);
        #endregion

        #region Identify
        private enum ResizeDirection
        {
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            TopRight = 5,
            Bottom = 6,
            BottomLeft = 7,
            BottomRight = 8,
        }

        private HwndSource _hwndSource;

        private PUButton _btnClose;

        private StackPanel _stkNav;

        private bool _animateOutHandle = true;

        private bool? _dialogResult;

        private bool _needToSetDialogResult;

        #endregion

        #region Constructor
        static PUWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUWindow), new FrameworkPropertyMetadata(typeof(PUWindow)));
        }

        /// <summary>
        /// 初始化一个窗体实例。
        /// 若您期望从当前窗体中连续Show出多个窗体，请务必分别指定这些子窗体的Owner，否则将出现显示问题。
        /// </summary>
        public PUWindow()
        {
            try
            {
                Owner = GetOwnerWindow();
            }
            catch (Exception ex) { }

            PreviewMouseMove += OnPreviewMouseMove;
        }
        #endregion

        #region Sys
        public override void OnApplyTemplate()
        {
            var auto = Owner;
            Loaded += delegate
            {
                if (AllowAutoCoverMask && (Owner as PUWindow) != null)
                {
                    (Owner as PUWindow).IsCoverMaskShow = true;
                }
                if (AnimateIn)
                    OnBeginLoadStoryboard();
                else
                    OnSkipLoadStoryboard();

                if (!AllowForcingClose)
                {
                    DisableAltF4(this);
                }
            };

            var grdResize = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(this, 0), 1) as Grid;
            if (grdResize != null)
            {
                foreach (Rectangle resizeRectangle in grdResize.Children)
                {
                    resizeRectangle.PreviewMouseDown += ResizeRectangle_PreviewMouseDown;
                    resizeRectangle.MouseMove += ResizeRectangle_MouseMove;
                }
            }

            var grdNavbar = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(this, 0), 0), 0), 0), 0) as Grid;
            _stkNav = VisualTreeHelper.GetChild(grdNavbar, 1) as StackPanel;
            grdNavbar.MouseLeftButtonDown += delegate
            {
                this.DragMove();
            };
            _btnClose = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(grdNavbar, 1), 2) as PUButton;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if(_animateOutHandle)
                base.OnClosing(e);

            if (e.Cancel)
                return;

            if (AllowAutoCoverMask && (Owner as PUWindow) != null)
            {
                var owner = Owner as PUWindow;
                if (owner.OwnedWindows.Count == 1)
                {
                    owner.IsCoverMaskShow = false;
                }
                else
                {
                    var closeMask = true;
                    foreach (var window in owner.OwnedWindows)
                    {
                        var puwindow = window as PUWindow;
                        if (puwindow != null)
                        {
                            if (puwindow.AllowAutoCoverMask)
                            {
                                closeMask = true;
                                break;
                            }
                        }
                    }
                    if (closeMask)
                        owner.IsCoverMaskShow = false;
                }
            }

            if (AnimateOut && _animateOutHandle)
            {
                if (IsModal(this))
                {
                    _dialogResult = DialogResult;
                    _needToSetDialogResult = true;
                }
                OnBeginCloseStoryboard();
                _animateOutHandle = false;
                DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.4) };
                timer.Tick += delegate { Close(); timer.Stop(); };
                timer.Start();

                e.Cancel = true;
                return;
            }
            if (AnimateOut)
            {
                if (_needToSetDialogResult)
                {
                    DialogResult = _dialogResult;
                }
            }
        }

        private void ResizeRectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;

            if (rectangle != null)
            {
                switch (rectangle.Name)
                {
                    case "Top":
                        Cursor = Cursors.SizeNS;
                        ResizeWindow(ResizeDirection.Top);
                        break;
                    case "Bottom":
                        Cursor = Cursors.SizeNS;
                        ResizeWindow(ResizeDirection.Bottom);
                        break;
                    case "Left":
                        Cursor = Cursors.SizeWE;
                        ResizeWindow(ResizeDirection.Left);
                        break;
                    case "Right":
                        Cursor = Cursors.SizeWE;
                        ResizeWindow(ResizeDirection.Right);
                        break;
                    case "TopLeft":
                        Cursor = Cursors.SizeNWSE;
                        ResizeWindow(ResizeDirection.TopLeft);
                        break;
                    case "TopRight":
                        Cursor = Cursors.SizeNESW;
                        ResizeWindow(ResizeDirection.TopRight);
                        break;
                    case "BottomLeft":
                        Cursor = Cursors.SizeNESW;
                        ResizeWindow(ResizeDirection.BottomLeft);
                        break;
                    case "BottomRight":
                        Cursor = Cursors.SizeNWSE;
                        ResizeWindow(ResizeDirection.BottomRight);
                        break;
                    default:
                        break;
                }
            }
        }

        private void ResizeWindow(ResizeDirection direction)
        {
            SendMessage(_hwndSource.Handle, 0x112, (IntPtr)(61440 + direction), IntPtr.Zero);
        }

        protected override void OnInitialized(EventArgs e)
        {
            SourceInitialized += MainWindow_SourceInitialized;

            base.OnInitialized(e);
        }

        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            _hwndSource = (HwndSource)PresentationSource.FromVisual(this);
        }

        protected void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (ResizeMode != ResizeMode.CanResize)
                return;

            if (Mouse.LeftButton != MouseButtonState.Pressed)
                Cursor = Cursors.Arrow;
        }

        private void ResizeRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;

            if (rectangle != null)
            {
                switch (rectangle.Name)
                {
                    case "Top":
                        Cursor = Cursors.SizeNS;
                        break;
                    case "Bottom":
                        Cursor = Cursors.SizeNS;
                        break;
                    case "Left":
                        Cursor = Cursors.SizeWE;
                        break;
                    case "Right":
                        Cursor = Cursors.SizeWE;
                        break;
                    case "TopLeft":
                        Cursor = Cursors.SizeNWSE;
                        break;
                    case "TopRight":
                        Cursor = Cursors.SizeNESW;
                        break;
                    case "BottomLeft":
                        Cursor = Cursors.SizeNESW;
                        break;
                    case "BottomRight":
                        Cursor = Cursors.SizeNWSE;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Alt + F4 无效化处理
        /// </summary>
        private static Action DisableAltF4(Window window)
        {
            var source = HwndSource.FromHwnd(new WindowInteropHelper(window).Handle);
            source.AddHook(DisableAltF4WndHookProc);
            return () => source.RemoveHook(DisableAltF4WndHookProc);
        }

        /// <summary>
        /// Alt + F4无效化窗口消息
        /// </summary>
        private static IntPtr DisableAltF4WndHookProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_SYSKEYDOWN = 0x0104;
            const int VK_F4 = 0x73;

            if (msg == WM_SYSKEYDOWN && wParam.ToInt32() == VK_F4)
            {
                handled = true;
            }

            return IntPtr.Zero;
        }

        #endregion

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
        /// 获取或设置窗体的返回结果，不会对前端显示造成影响。默认值为Null
        /// </summary>
        public object Result
        {
            get { return (object)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register("Result", typeof(object), typeof(PUWindow));



        /// <summary>
        ///获取或设置是否打开窗体的遮罩层。默认值为False。
        /// </summary>
        public bool IsCoverMaskShow
        {
            get { return (bool)GetValue(IsCoverMaskShowProperty); }
            set { SetValue(IsCoverMaskShowProperty, value); }
        }
        public static readonly DependencyProperty IsCoverMaskShowProperty =
            DependencyProperty.Register("IsCoverMaskShow", typeof(bool), typeof(PUWindow));

        /// <summary>
        /// 获取或设置是否允许延迟显示内容。当页面内容较为复杂时，启用此选项有助于减少启动动画卡顿。默认值为False。
        /// </summary>
        public bool AllowShowDelay
        {
            get { return (bool)GetValue(AllowShowDelayProperty); }
            set { SetValue(AllowShowDelayProperty, value); }
        }
        public static readonly DependencyProperty AllowShowDelayProperty =
            DependencyProperty.Register("AllowShowDelay", typeof(bool), typeof(PUWindow));

        /// <summary>
        /// 获取或设置右侧标题栏按钮的显示状态。默认值为Visible。
        /// </summary>
        public Visibility NavButtonVisibility
        {
            get { return (Visibility)GetValue(NavButtonVisibilityProperty); }
            set { SetValue(NavButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty NavButtonVisibilityProperty =
            DependencyProperty.Register("NavButtonVisibility", typeof(Visibility), typeof(PUWindow), new PropertyMetadata(Visibility.Visible));


        /// <summary>
        /// 获取或设置是否在窗体打开时使用动画。默认值为True。
        /// </summary>
        public bool AnimateIn
        {
            get { return (bool)GetValue(AnimateInProperty); }
            set { SetValue(AnimateInProperty, value); }
        }
        public static readonly DependencyProperty AnimateInProperty =
            DependencyProperty.Register("AnimateIn", typeof(bool), typeof(PUWindow), new PropertyMetadata(true));

        /// <summary>
        /// 获取或设置是否在窗体关闭时使用动画，默认值为True。
        /// </summary>
        public bool AnimateOut
        {
            get { return (bool)GetValue(AnimateOutProperty); }
            set { SetValue(AnimateOutProperty, value); }
        }
        public static readonly DependencyProperty AnimateOutProperty =
            DependencyProperty.Register("AnimateOut", typeof(bool), typeof(PUWindow), new PropertyMetadata(true));

        /// <summary>
        /// 获取或设置窗体动画类型。默认值为Gradual（一个从上到下的渐变显示）。
        /// </summary>
        public AnimationStyles AnimationStyle
        {
            get { return (AnimationStyles)GetValue(AnimationStyleProperty); }
            set { SetValue(AnimationStyleProperty, value); }
        }
        public static readonly DependencyProperty AnimationStyleProperty =
            DependencyProperty.Register("AnimationStyle", typeof(AnimationStyles), typeof(PUWindow), new PropertyMetadata(AnimationStyles.Scale));

        /// <summary>
        /// 获取或设置窗体的圆角大小，默认值为0。
        /// </summary>
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty =
            DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(PUWindow));

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
            DependencyProperty.Register("Header", typeof(object), typeof(PUWindow));

        /// <summary>
        /// 左上角图标，默认值为null。
        /// </summary>
        public new object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public new static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), typeof(PUWindow));

        /// <summary>
        /// 控制栏背景色，默认值为White（白色）。
        /// </summary>
        public Brush NavbarBackground
        {
            get { return (Brush)GetValue(NavbarBackgroundProperty); }
            set { SetValue(NavbarBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavbarBackgroundProperty =
            DependencyProperty.Register("NavbarBackground", typeof(Brush), typeof(PUWindow));

        /// <summary>
        /// 控制栏高度，默认值为30。
        /// </summary>
        public double NavbarHeight
        {
            get { return (double)GetValue(NavbarHeightProperty); }
            set { SetValue(NavbarHeightProperty, value); }
        }
        public static readonly DependencyProperty NavbarHeightProperty =
            DependencyProperty.Register("NavbarHeight", typeof(double), typeof(PUWindow));

        /// <summary>
        /// 控制按钮高度，默认值为30。
        /// </summary>
        public double NavButtonHeight
        {
            get { return (double)GetValue(NavButtonHeightProperty); }
            set { SetValue(NavButtonHeightProperty, value); }
        }
        public static readonly DependencyProperty NavButtonHeightProperty =
            DependencyProperty.Register("NavButtonHeight", typeof(double), typeof(PUWindow));

        /// <summary>
        /// 控制按钮宽度，默认值为40。
        /// </summary>
        public double NavButtonWidth
        {
            get { return (double)GetValue(NavButtonWidthProperty); }
            set { SetValue(NavButtonWidthProperty, value); }
        }
        public static readonly DependencyProperty NavButtonWidthProperty =
            DependencyProperty.Register("NavButtonWidth", typeof(double), typeof(PUWindow));

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

        /// <summary>
        /// 获取或设置是否允许在调用Show或ShowDialog方法时自动打开父窗体的遮罩层，并在Close时将其关闭。
        /// <para>若没有父窗体或父窗体不是PUWindow类型，则不会触发任何效果。</para>
        /// </summary>
        public bool AllowAutoCoverMask
        {
            get { return (bool)GetValue(AllowAutoCoverMaskProperty); }
            set { SetValue(AllowAutoCoverMaskProperty, value); }
        }

        public static readonly DependencyProperty AllowAutoCoverMaskProperty =
            DependencyProperty.Register("AllowAutoCoverMask", typeof(bool), typeof(PUWindow), new PropertyMetadata(false));


        /// <summary>
        /// 获取或设置是否允许用户使用Alt + F4按键组合强制关闭窗体。该属性在窗体加载后的更改将无效。默认值为True（允许）。
        /// </summary>
        public bool AllowForcingClose
        {
            get { return (bool)GetValue(AllowForcingCloseProperty); }
            set { SetValue(AllowForcingCloseProperty, value); }
        }

        public static readonly DependencyProperty AllowForcingCloseProperty =
            DependencyProperty.Register("AllowForcingClose", typeof(bool), typeof(PUWindow), new PropertyMetadata(true));


        /// <summary>
        /// 获取或设置是否允许自动将当前的活动窗口设置为自己的Owner。默认值为True。
        /// </summary>
        public bool AllowAutoOwner
        {
            get { return (bool)GetValue(AllowAutoOwnerProperty); }
            set { SetValue(AllowAutoOwnerProperty, value); }
        }

        public static readonly DependencyProperty AllowAutoOwnerProperty =
            DependencyProperty.Register("AllowAutoOwner", typeof(bool), typeof(PUWindow), new PropertyMetadata(false, OnAllowAutoOwnerChanged));

        private static void OnAllowAutoOwnerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as PUWindow;
            if (window.AllowAutoOwner == false)
            {
                window.Owner = null;
            }
        }

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

        #region APIs
        /// <summary>
        /// 向控制栏的左侧添加一个新的按钮。
        /// <para>警告：当Window的前景色发生改变时，该按钮的前景色不会随之变化。</para>
        /// </summary>
        /// <param name="content">按钮的内容</param>
        /// <param name="clickHandler">点击按钮时应该触发的事件。</param>
        public void AppendNavButton(object content, RoutedEventHandler clickHandler, bool isIconFont = true, object tooltip = null)
        {
            var btn = new PUButton()
            {
                Content = content,
                ButtonStyle = ButtonStyles.Hollow,
                Foreground = Foreground,
                BorderBrush = new SolidColorBrush(Colors.Transparent),
                CoverBrush = new SolidColorBrush(((Color)ColorConverter.ConvertFromString("#99999999"))),
                HorizontalAlignment = HorizontalAlignment.Right,
            };
            if (isIconFont)
                btn.FontFamily = FindResource("IconFont") as FontFamily;
            if (tooltip != null)
                btn.ToolTip = tooltip;
            var visibility = new Binding() { Path = new PropertyPath("NavButtonVisibility"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Source = this, Mode = BindingMode.OneWay };
            BindingOperations.SetBinding(btn, VisibilityProperty, visibility);

            var width = new Binding() { Path = new PropertyPath("NavButtonWidth"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Source = this, Mode = BindingMode.OneWay };
            BindingOperations.SetBinding(btn, WidthProperty, width);
            var height = new Binding() { Path = new PropertyPath("NavButtonHeight"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Source = this, Mode = BindingMode.OneWay };
            BindingOperations.SetBinding(btn, HeightProperty, height);
            var fontsize = new Binding() { Path = new PropertyPath("FontSize"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Source = this, Mode = BindingMode.OneWay };
            BindingOperations.SetBinding(btn, FontSizeProperty, fontsize);
            btn.Click += clickHandler;
            if (IsLoaded)
                _stkNav.Children.Insert(0, btn);
            else
            {
                Loaded += delegate
                {
                    _stkNav.Children.Insert(0, btn);
                };
            }
        }
        #endregion

        #region Function
        private static bool IsModal(Window window)
        {
            return (bool)typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(window);
        }

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
    }

    internal class CloseWindowCommand : ICommand
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
    internal class MaxWindowCommand : ICommand
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
    internal class MinWindowCommand : ICommand
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
