using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Panuon.UI
{
    /// <summary>
    /// PUPagingNav.xaml 的交互逻辑
    /// </summary>
    public partial class PUPagingNav : UserControl
    {
        public PUPagingNav()
        {
            InitializeComponent();
            Foreground = new SolidColorBrush(Colors.White);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Loaded += delegate
            {
                Load();
                Select();
            };
        }

        #region RoutedEvent
        /// <summary>
        /// 页码发生改变事件。
        /// </summary>
        public static readonly RoutedEvent CurrentPageChangedEvent = EventManager.RegisterRoutedEvent("CurrentPageChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(PUPagingNav));
        public event RoutedPropertyChangedEventHandler<int> CurrentPageChanged
        {
            add { AddHandler(CurrentPageChangedEvent, value); }
            remove { RemoveHandler(CurrentPageChangedEvent, value); }
        }
        internal void OnCurrentPageChanged(int oldItem, int newItem)
        {
            RoutedPropertyChangedEventArgs<int> arg = new RoutedPropertyChangedEventArgs<int>(oldItem, newItem, CurrentPageChangedEvent);
            RaiseEvent(arg);
        }
        #endregion

        #region Property
        /// <summary>
        /// 获取或设置当前的总页数。默认值为1。
        /// </summary>
        public int TotalPage
        {
            get { return (int)GetValue(TotalPageProperty); }
            set { SetValue(TotalPageProperty, value); }
        }

        public static readonly DependencyProperty TotalPageProperty =
            DependencyProperty.Register("TotalPage", typeof(int), typeof(PUPagingNav), new PropertyMetadata(1, OnTotalPageChanged));

        private static void OnTotalPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            var nav = d as PUPagingNav;
            if (!nav.IsLoaded)
                return;
            nav.Load();
            nav.Select();
        }

        /// <summary>
        /// 获取或设置当前的页码。默认值为1。
        /// </summary>
        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(int), typeof(PUPagingNav), new PropertyMetadata(1, OnCurrentPageChanged));

        private static void OnCurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;

            var nav = d as PUPagingNav;
            if (!nav.IsLoaded)
                return;
            nav.OnCurrentPageChanged((int)e.OldValue, (int)e.NewValue);
            nav.Load();
            nav.Select();
        }

        /// <summary>
        /// 获取或设置按钮的圆角大小，默认值为3。
        /// </summary>
        public CornerRadius ButtonCornerRadius
        {
            get { return (CornerRadius)GetValue(ButtonCornerRadiusProperty); }
            set { SetValue(ButtonCornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty ButtonCornerRadiusProperty =
            DependencyProperty.Register("ButtonCornerRadius", typeof(CornerRadius), typeof(PUPagingNav), new PropertyMetadata(new CornerRadius(3)));



        /// <summary>
        /// 获取或设置两侧的按钮是否显示。默认值为True。
        /// </summary>
        public bool IsSideButtonShow
        {
            get { return (bool)GetValue(IsSideButtonShowProperty); }
            set { SetValue(IsSideButtonShowProperty, value); }
        }

        public static readonly DependencyProperty IsSideButtonShowProperty =
            DependencyProperty.Register("IsSideButtonShow", typeof(bool), typeof(PUPagingNav), new PropertyMetadata(true, OnIsSideButtonShowChanged));

        private static void OnIsSideButtonShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var nav = d as PUPagingNav;
            if (nav.IsSideButtonShow)
            {
                nav.BtnLeft.Visibility = Visibility.Visible;
                nav.BtnRight.Visibility = Visibility.Visible;
            }
            else
            {
                nav.BtnLeft.Visibility = Visibility.Collapsed;
                nav.BtnRight.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 获取或设置按钮的背景颜色。默认值为#AA3E3E3E。
        /// </summary>
        public Brush ButtonBrush
        {
            get { return (Brush)GetValue(ButtonBrushProperty); }
            set { SetValue(ButtonBrushProperty, value); }
        }

        public static readonly DependencyProperty ButtonBrushProperty =
            DependencyProperty.Register("ButtonBrush", typeof(Brush), typeof(PUPagingNav), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AA3E3E3E"))));

        /// <summary>
        /// 获取或设置按钮被选中或点击时的颜色。默认值为#3E3E3E。
        /// </summary>
        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(PUPagingNav), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E3E3E"))));
        #endregion

        #region Function
        private void Load()
        {
            if(TotalPage <= 0)
            {
                TotalPage = 0;
                return;
            }
            if(CurrentPage <= 0)
            {
                CurrentPage = 1;
                return;
            }
            if (CurrentPage > TotalPage)
            {
                CurrentPage = TotalPage;
                return;
            }

            StkMain.Children.Clear();
            if (TotalPage <= 7)
            {
                for (var i = 1; i <= TotalPage; i++)
                {
                    StkMain.Children.Add(GetRadioButton(i));
                }
            }
            else
            {
                StkMain.Children.Add(GetRadioButton(1));
                StkMain.Children.Add(GetRadioButton(2));
                //第1或2页或3，直接追加到5
                if (CurrentPage == 1 || CurrentPage == 2 || CurrentPage == 3 || CurrentPage == 4)
                {
                    StkMain.Children.Add(GetRadioButton(3));
                    StkMain.Children.Add(GetRadioButton(4));
                    StkMain.Children.Add(GetRadioButton(5));
                }

                //...
                StkMain.Children.Add(GetTextBlock());

                //距离终点小于4，直接追加直到末尾
                if (CurrentPage >= TotalPage - 3)
                {
                    StkMain.Children.Add(GetTextBlock());
                    for (var i = TotalPage - 4; i <= TotalPage; i++)
                    {
                        StkMain.Children.Add(GetRadioButton(i));
                    }
                    return;
                }
                if (CurrentPage != 1 && CurrentPage != 2 && CurrentPage != 3 && CurrentPage != 4)
                {
                    //追加三条
                    for (var i = CurrentPage - 1; i <= (CurrentPage + 1); i++)
                    {
                        StkMain.Children.Add(GetRadioButton(i));
                    }
                }
                StkMain.Children.Add(GetTextBlock());
                for (var i = TotalPage - 1; i <= TotalPage; i++)
                {
                    StkMain.Children.Add(GetRadioButton(i));
                }
            }
        }

        private void Select()
        {
            foreach (var item in StkMain.Children)
            {
                var radio = item as PURadioButton;
                if (radio == null)
                    continue;
                if (radio.Content.ToString() == CurrentPage.ToString())
                {
                    radio.IsChecked = true;
                    break;
                }
            }
        }

        private PURadioButton GetRadioButton(int content)
        {
            var radio = new PURadioButton()
            {
                RadioButtonStyle = RadioButtonStyles.Button,
                Content = content,
                HorizontalAlignment = HorizontalAlignment.Left,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Padding = new Thickness(5,0,5,0),
                Margin = new Thickness(6, 0, 0, 0),
            };
            var back = new Binding() { Source = this, Path = new PropertyPath("ButtonBrush"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            BindingOperations.SetBinding(radio, PURadioButton.BackgroundProperty, back);
            var cover = new Binding() { Source = this, Path = new PropertyPath("SelectedBrush"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            BindingOperations.SetBinding(radio, PURadioButton.CoverBrushProperty, cover);
            var fore = new Binding() { Source = this, Path = new PropertyPath("Foreground"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            BindingOperations.SetBinding(radio, PURadioButton.ForegroundProperty, fore);
            var height = new Binding() { Source = this, Path = new PropertyPath("ActualHeight"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            BindingOperations.SetBinding(radio, PURadioButton.HeightProperty, height);
            var radius = new Binding() { Source = this, Path = new PropertyPath("ButtonCornerRadius"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            BindingOperations.SetBinding(radio, PURadioButton.BorderCornerRadiusProperty, radius);

            radio.Click += delegate
            {
                CurrentPage = content;
            };
            return radio;
        }

        private TextBlock GetTextBlock()
        {
           var txt = new TextBlock()
            {
                Text = "...",
                Margin = new Thickness(5, 0, 5, 0),
                VerticalAlignment = VerticalAlignment.Center,
            };

            var fore = new Binding() { Source = this, Path = new PropertyPath("ButtonBrush"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            BindingOperations.SetBinding(txt, TextBlock.ForegroundProperty, fore);
            return txt;
        }
        #endregion

        #region Sys
        private void BtnLeft_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage--;
        }

        private void BtnRight_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < TotalPage)
                CurrentPage++;
        }
        #endregion
    }
}
