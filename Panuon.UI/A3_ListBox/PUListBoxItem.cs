using System.Windows;
using System.Windows.Controls;


namespace Panuon.UI
{
    public class PUListBoxItem : ListBoxItem
    {
        static PUListBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUListBoxItem), new FrameworkPropertyMetadata(typeof(PUListBoxItem)));
        }

        #region RoutedEvent
        /// <summary>
        /// 当子项被搜索到时，触发此事件。
        /// </summary>
        public static readonly RoutedEvent SearchedEvent = EventManager.RegisterRoutedEvent("Searched", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<PUListBoxItem>), typeof(PUListBoxItem));
        public event RoutedPropertyChangedEventHandler<PUListBoxItem> Searched
        {
            add { AddHandler(SearchedEvent, value); }
            remove { RemoveHandler(SearchedEvent, value); }
        }
        internal void OnSearched()
        {
            RoutedPropertyChangedEventArgs<PUListBoxItem> arg = new RoutedPropertyChangedEventArgs<PUListBoxItem>(null, this, SearchedEvent);
            RaiseEvent(arg);
        }

        #endregion

        #region Property
        /// <summary>
        /// 获取或设置该子项可以携带的值，仅用于标记该子项的实际内容，不会对前端显示造成影响。
        /// </summary>
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(PUListBoxItem));
        #endregion
    }
}
