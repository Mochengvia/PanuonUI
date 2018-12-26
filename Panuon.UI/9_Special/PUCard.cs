using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PUCard : UserControl
    {
        #region Constructor
        static PUCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUCard), new FrameworkPropertyMetadata(typeof(PUCard)));

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            AddHandler(PUButton.ClickEvent, new RoutedEventHandler(OnDetailButtonClicked));
        }

        private void OnDetailButtonClicked(object sender, RoutedEventArgs e)
        {
            var btnDetail = (sender as PUButton);
            if (btnDetail == null || btnDetail.Tag == null || btnDetail.Tag.ToString() != "PART_Detail")
                return;
            OnDetail();
        }
        #endregion

        #region RoutedEvent
        /// <summary>
        /// 点击详情事件。
        /// </summary>
        public static readonly RoutedEvent DetailEvent = EventManager.RegisterRoutedEvent("Detail", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<PUCard>), typeof(PUCard));
        public event RoutedPropertyChangedEventHandler<PUCard> Detail
        {
            add { AddHandler(DetailEvent, value); }
            remove { RemoveHandler(DetailEvent, value); }
        }
        internal void OnDetail()
        {
            RoutedPropertyChangedEventArgs<PUCard> arg = new RoutedPropertyChangedEventArgs<PUCard>(this, this, DetailEvent);
            RaiseEvent(arg);
        }
        #endregion

        #region Property

        /// <summary>
        /// 获取或设置是否显示详情按钮。默认值为False。
        /// </summary>
        public bool IsDetailButtonShow
        {
            get { return (bool)GetValue(IsDetailButtonShowProperty); }
            set { SetValue(IsDetailButtonShowProperty, value); }
        }

        public static readonly DependencyProperty IsDetailButtonShowProperty =
            DependencyProperty.Register("IsDetailButtonShow", typeof(bool), typeof(PUCard));

        /// <summary>
        /// 获取或设置标题的内容。默认值为Null。
        /// </summary>
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(PUCard));

        /// <summary>
        /// 获取或设置标题前的图标按钮。默认值为Null。
        /// </summary>
        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), typeof(PUCard));


        /// <summary>
        /// 获取或设置阴影颜色。默认值为透明。
        /// </summary>
        public Color ShadowColor
        {
            get { return (Color)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }

        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.Register("ShadowColor", typeof(Color), typeof(PUCard));


        /// <summary>
        /// 获取或设置卡片的圆角大小。默认值为0。
        /// </summary>
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty BorderCornerRadiusProperty =
            DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(PUCard));

        /// <summary>
        /// 获取或设置卡片的标题高度（包含标题和详情按钮）。默认值为30。
        /// </summary>
        public double HeaderHeight
        {
            get { return (double)GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }

        public static readonly DependencyProperty HeaderHeightProperty =
            DependencyProperty.Register("HeaderHeight", typeof(double), typeof(PUCard));





        #endregion
    }
}
