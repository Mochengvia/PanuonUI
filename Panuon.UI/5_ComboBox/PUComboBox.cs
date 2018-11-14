using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PUComboBox : ComboBox
    {

        static PUComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUComboBox), new FrameworkPropertyMetadata(typeof(PUComboBox)));
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {

            if (SelectedValuePath == SelectedValuePaths.Header)
                SelectedValue = SelectedItem == null ? "" : (SelectedItem as PUComboBoxItem).Content;
            else
                SelectedValue = SelectedItem == null ? null : (SelectedItem as PUComboBoxItem).Value;
            base.OnSelectionChanged(e);
            if (SearchMode == SearchModes.None)
                return;
            else if(SearchMode == SearchModes.TextChanged)
                AddHandler(PUTextBox.TextChangedEvent, new RoutedEventHandler(OnSearchTextChanged));
            else
                AddHandler(PUTextBox.PreviewKeyDownEvent, new RoutedEventHandler(OnSearchKeyDown));
        }

        private void OnSearchKeyDown(object sender, RoutedEventArgs e)
        {
            var eve = e as System.Windows.Input.KeyEventArgs;
            if (eve.Key != System.Windows.Input.Key.Enter)
                return;
            var tbSearch = e.OriginalSource as PUTextBox;
            if (tbSearch.Tag == null || tbSearch.Tag.ToString() != "Search")
                return;
            var text = tbSearch.Text;
            foreach (var item in Items)
            {
                var comboItem = item as ComboBoxItem;
                if (comboItem.Content.ToString().Contains(text))
                    comboItem.Visibility = Visibility.Visible;
                else
                    comboItem.Visibility = Visibility.Collapsed;
            }
            e.Handled = true;
        }

        private void OnSearchTextChanged(object sender, RoutedEventArgs e)
        {
            var tbSearch = e.OriginalSource as PUTextBox;
            if (tbSearch.Tag == null || tbSearch.Tag.ToString() != "Search")
                return;

            var text = tbSearch.Text;
            foreach(var item in Items)
            {
                var comboItem = item as ComboBoxItem;
                if (comboItem.Content.ToString().Contains(text))
                    comboItem.Visibility = Visibility.Visible;
                else
                    comboItem.Visibility = Visibility.Collapsed;
            }
        }

        #region RoutedEvent
        /// <summary>
        /// 用户点击删除按钮事件。
        /// </summary>
        public static readonly RoutedEvent DeleteItemEvent = EventManager.RegisterRoutedEvent("DeleteItem", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<PUComboBoxItem>), typeof(PUComboBox));
        public event RoutedPropertyChangedEventHandler<PUComboBoxItem> DeleteItem
        {
            add { AddHandler(DeleteItemEvent, value); }
            remove { RemoveHandler(DeleteItemEvent, value); }
        }
        internal void OnDeleteItem(PUComboBoxItem oldItem, PUComboBoxItem newItem)
        {
            RoutedPropertyChangedEventArgs<PUComboBoxItem> arg = new RoutedPropertyChangedEventArgs<PUComboBoxItem>(oldItem, newItem, DeleteItemEvent);
            RaiseEvent(arg);
        }

        #endregion

        #region Property
        /// <summary>
        /// 鼠标悬浮时的背景颜色，默认值为浅灰色(#EEEEEE)。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }
        public static readonly DependencyProperty CoverBrushProperty =
            DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUComboBox), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEEEEE"))));

        /// <summary>
        /// 项目被选中时的背景颜色，默认值为浅灰色(#DDDDDD)。
        /// </summary>
        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(PUComboBox), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DDDDDD"))));

        /// <summary>
        /// 圆角大小，默认值为0。
        /// </summary>
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty =
            DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(PUComboBox), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        ///  下拉框激活时阴影的颜色，默认值为#888888。
        /// </summary>
        public Color ShadowColor
        {
            get { return (Color)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }
        public static readonly DependencyProperty ShadowColorProperty = DependencyProperty.Register("ShadowColor", typeof(Color), typeof(PUComboBox), new PropertyMetadata((Color)ColorConverter.ConvertFromString("#888888")));

        /// <summary>
        /// 当子项目删除按钮可见时，用户点击删除按钮后的操作。默认为删除项目并触发DeleteItem路由事件。
        /// </summary>
        public DeleteModes DeleteMode
        {
            get { return (DeleteModes)GetValue(DeleteModeProperty); }
            set { SetValue(DeleteModeProperty, value); }
        }

        public static readonly DependencyProperty DeleteModeProperty =
            DependencyProperty.Register("DeleteMode", typeof(DeleteModes), typeof(PUComboBoxItem), new PropertyMetadata(DeleteModes.Delete));

        /// <summary>
        /// 若使用MVVM绑定，请使用此依赖属性。
        /// </summary>
        public IList<PUComboBoxItemModel> BindingItems
        {
            get { return (IList<PUComboBoxItemModel>)GetValue(BindingItemsProperty); }
            set { SetValue(BindingItemsProperty, value); }
        }

        public static readonly DependencyProperty BindingItemsProperty =
            DependencyProperty.Register("BindingItems", typeof(IList<PUComboBoxItemModel>), typeof(PUComboBox), new PropertyMetadata(null, OnBindingItemsChanged));

        private static void OnBindingItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var comboBox = d as PUComboBox;
            var items = comboBox.BindingItems;
            if (items == null)
                return;
            comboBox.Items.Clear();

            foreach (var item in items)
            {
                var comboBoxItem = new PUComboBoxItem()
                {
                    Content = item.Header,
                    Value = item.Value,
                    DeleteButtonVisibility = item.CanDelete ? Visibility.Visible : Visibility.Collapsed,
                };

                if (comboBox.Items.Count == 0)
                    comboBoxItem.IsSelected = true;
                comboBox.Items.Add(comboBoxItem);
            }
        }


        /// <summary>
        /// 该属性指定了当子项目被选中时，SelectedValue应呈现子项目的哪一个值。
        /// 可选项为Header或Value，默认值为Header。
        /// </summary>
        public new SelectedValuePaths SelectedValuePath
        {
            get { return (SelectedValuePaths)GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }

        public new static readonly DependencyProperty SelectedValuePathProperty =
            DependencyProperty.Register("SelectedValuePath", typeof(SelectedValuePaths), typeof(PUComboBox), new PropertyMetadata(SelectedValuePaths.Header));


        /// <summary>
        /// 获取被选中PUComboBoxItem的Header或Value属性（这取决于SelectedValuePath），
        /// 或根据设置的SelectedValue来选中子项目。
        /// </summary>
        public new object SelectedValue
        {
            get { return (object)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        public new static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(object), typeof(PUComboBox), new PropertyMetadata("", OnSelectedValueChanged));

        private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var comboBox = d as PUComboBox;
            if (comboBox.SelectedValue == null)
            {
                return;
            }
            if (e.NewValue == e.OldValue)
                return;

            var selectedItem = comboBox.SelectedItem as PUComboBoxItem;
            foreach (var item in comboBox.Items)
            {
                var comboBoxItem = item as PUComboBoxItem;
                if ((comboBox.SelectedValuePath == SelectedValuePaths.Header ?
                    (comboBoxItem.Content == null ? false : comboBoxItem.Content.ToString() == comboBox.SelectedValue.ToString()) :
                    (comboBoxItem.Value == null ? false : comboBoxItem.Value.Equals(comboBox.SelectedValue))))
                {
                    if (!comboBoxItem.IsSelected)
                        comboBoxItem.IsSelected = true;
                    return;
                }
            }
        }

        /// <summary>
        /// 获取或设置搜索模式。默认为不显示搜索。
        /// </summary>
        public SearchModes SearchMode
        {
            get { return (SearchModes)GetValue(SearchModeProperty); }
            set { SetValue(SearchModeProperty, value); }
        }

        public static readonly DependencyProperty SearchModeProperty =
            DependencyProperty.Register("SearchMode", typeof(SearchModes), typeof(PUComboBox), new PropertyMetadata(SearchModes.None,OnSearchModeChanged));

        private static void OnSearchModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var comboBox = d as PUComboBox;
            if (!comboBox.IsLoaded)
                return;

            comboBox.RemoveHandler(PUTextBox.TextChangedEvent, new RoutedEventHandler(comboBox.OnSearchTextChanged));
            comboBox.RemoveHandler(PUTextBox.KeyDownEvent, new RoutedEventHandler(comboBox.OnSearchKeyDown));

            if (comboBox.SearchMode == SearchModes.None)
                return;
            else if (comboBox.SearchMode == SearchModes.TextChanged)
                comboBox.AddHandler(PUTextBox.TextChangedEvent, new RoutedEventHandler(comboBox.OnSearchTextChanged));
            else
                comboBox.AddHandler(PUTextBox.KeyDownEvent, new RoutedEventHandler(comboBox.OnSearchKeyDown));
        }


        #endregion

        #region Enums
        public enum SelectedValuePaths
        {
            Header,
            Value
        }

        public enum DeleteModes
        {
            /// <summary>
            /// 当用户点击删除按钮时，删除项目并触发DeleteItem路由事件。
            /// </summary>
            Delete,
            /// <summary>
            /// 当用户点击删除按钮时，不直接删除项目（只触发DeleteItem路由事件）。
            /// </summary>
            EventOnly,
        }

        public enum SearchModes
        {
            /// <summary>
            /// 不显示搜索框。
            /// </summary>
            None,
            /// <summary>
            /// 在搜索框按下键盘时搜索。
            /// </summary>
            TextChanged,
            /// <summary>
            /// 当按下Enter键时发起搜索。
            /// </summary>
            Enter,
        }
        #endregion
    }
}
