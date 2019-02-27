using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PUListBox : ListBox
    {
        static PUListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUListBox), new FrameworkPropertyMetadata(typeof(PUListBox)));
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (SelectedValuePath == SelectedValuePaths.Header)
                SelectedValue = SelectedItem == null ? "" : (SelectedItem as PUListBoxItem).Content;
            else
                SelectedValue = SelectedItem == null ? null : (SelectedItem as PUListBoxItem).Value;
            base.OnSelectionChanged(e);
        }

        #region Property
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("该属性对此控件无效。请使用BindingItems属性替代。",true)]
        public new IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            private set { SetValue(ItemsSourceProperty, value); }
        }

        public new static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(PUListBox));

        /// <summary>
        /// 该属性指定了当子项目被选中时，SelectedValue应呈现子项目的哪一个值。默认值为Header。
        /// </summary>
        public new SelectedValuePaths SelectedValuePath
        {
            get { return (SelectedValuePaths)GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }

        public new static readonly DependencyProperty SelectedValuePathProperty =
            DependencyProperty.Register("SelectedValuePath", typeof(SelectedValuePaths), typeof(PUListBox), new PropertyMetadata(SelectedValuePaths.Header));


        /// <summary>
        /// 获取被选中PUTabItem的Header（即ListBoxItem的Content属性）或Value属性（这取决于SelectedValuePath），或反向选中子项目。
        /// </summary>
        public new object SelectedValue
        {
            get { return (object)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        public new static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(object), typeof(PUListBox), new PropertyMetadata("", OnSelectedValueChanged));

        private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listBox = d as PUListBox;
            if (listBox.SelectedValue == null)
            {
                return;
            }
            if (e.NewValue == e.OldValue)
                return;

            var selectedItem = listBox.SelectedItem as PUListBoxItem;
            foreach (var item in listBox.Items)
            {
                var listBoxItem = item as PUListBoxItem;
                if ((listBox.SelectedValuePath == SelectedValuePaths.Header ?
                    (listBoxItem.Content == null ? false : listBoxItem.Content.Equals(listBox.SelectedValue)) :
                    (listBoxItem.Value == null ? false : listBoxItem.Value.Equals(listBox.SelectedValue))))
                {
                    if (!listBoxItem.IsSelected)
                    {
                        listBoxItem.IsSelected = true;
                        listBox.ScrollIntoView(listBoxItem);
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// 获取或设置当鼠标悬浮时ListBoxItem的背景色。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }

        public static readonly DependencyProperty CoverBrushProperty =
            DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUListBox));

        /// <summary>
        /// 获取或设置当ListBoxItem被选中时的背景色。
        /// </summary>
        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(PUListBox));

        /// <summary>
        /// 获取或设置当搜索ListBoxItem时，ListBoxItem被找到时应呈现的背景色。
        /// </summary>
        public Brush SearchBrush
        {
            get { return (Brush)GetValue(SearchBrushProperty); }
            set { SetValue(SearchBrushProperty, value); }
        }

        public static readonly DependencyProperty SearchBrushProperty =
            DependencyProperty.Register("SearchBrush", typeof(Brush), typeof(PUListBox));

        /// <summary>
        /// 若使用MVVM绑定，请使用此依赖属性。
        /// </summary>
        public ObservableCollection<PUListBoxItemModel> BindingItems
        {
            get { return (ObservableCollection<PUListBoxItemModel>)GetValue(BindingItemsProperty); }
            set { SetValue(BindingItemsProperty, value); }
        }

        public static readonly DependencyProperty BindingItemsProperty =
            DependencyProperty.Register("BindingItems", typeof(ObservableCollection<PUListBoxItemModel>), typeof(PUListBox), new PropertyMetadata(null, OnBindingItemsChanged));

        private static void OnBindingItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listBox = d as PUListBox;
            if (listBox.BindingItems != null)
            {
                listBox.BindingItems.CollectionChanged -= listBox.BindingItemChanged;
                listBox.BindingItems.CollectionChanged += listBox.BindingItemChanged;
            }
            listBox.GenerateBindindItems(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void BindingItemChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            GenerateBindindItems(e);
        }


        #endregion

        #region APIs
        /// <summary>
        /// 通过内容选中项目。
        /// <para>若内容不是值类型，则将逐一比较其中每一个可写属性的值是否相等。</para>
        /// </summary>
        /// <param name="content">要匹配的内容。</param>
        public void SelectItemByContent(object content)
        {
            var item = GetItemByContent(content);
            if (!item.IsSelected)
                item.IsSelected = true;
        }

        /// <summary>
        /// 通过Value选中项目。
        /// <para>若Value不是值类型，则将逐一比较其中每一个可写属性的值是否相等。</para>
        /// </summary>
        /// <param name="value">要匹配的Value</param>
        public void SelectItemByValue(object value)
        {
            var item = GetItemByValue(value);
            if (!item.IsSelected)
                item.IsSelected = true;
        }

        /// <summary>
        /// 通过内容查询符合条件的第一个子项，滚动到该项目并高亮（子项的内容须为string类型）。
        /// </summary>
        /// <param name="content">子项的内容。</param>
        /// <param name="allowFuzzySearch">是否允许模糊查询。</param>
        public void SearchItemByContent(string content, bool allowFuzzySearch = true)
        {
            foreach (var item in Items)
            {
                var listBoxItem = item as PUListBoxItem;
                if (listBoxItem.Content == null ? false : allowFuzzySearch ? listBoxItem.Content.ToString().Contains(content) : listBoxItem.Content.ToString() == content)
                {
                    ScrollIntoView(listBoxItem);
                    listBoxItem.OnSearched();
                    return;
                }
            }
        }

        /// <summary>
        /// 通过Value查询符合条件的第一个子项，滚动到该项目并高亮。
        /// </summary>
        public void SearchItemByValue(object value)
        {
            var item = GetItemByValue(value);
            if (item == null)
                return;
            ScrollIntoView(item);
            item.OnSearched();
        }
        #endregion

        #region Function
        /// <summary>
        /// 通过Value获取符合条件的第一个子项。
        /// </summary>
        private PUListBoxItem GetItemByValue(object value)
        {
            foreach (var item in Items)
            {
                var listBoxItem = item as PUListBoxItem;
                if (listBoxItem.Value == null ? false : listBoxItem.Value.Equals(value))
                    return listBoxItem;
            }
            return null;
        }

        /// <summary>
        /// 通过内容获取符合条件的第一个子项。
        /// </summary>
        private PUListBoxItem GetItemByContent(object content)
        {
            foreach (var item in Items)
            {
                var listBoxItem = item as PUListBoxItem;
                if (listBoxItem.Content == null ? false : listBoxItem.Content.Equals(content))
                    return listBoxItem;
            }
            return null;
        }

        private void GenerateBindindItems(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    var selectedValue = SelectedValue;
                    SelectedValue = null;
                    Items.Clear();
                    if (BindingItems == null)
                        break;
                    foreach (var item in BindingItems)
                    {
                        var tabItem = GenerateComboBoxItem(item);
                        Items.Add(tabItem);
                    }
                    SelectedValue = selectedValue;
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        var tabItem = GenerateComboBoxItem(item as PUListBoxItemModel);
                        Items.Insert(e.NewStartingIndex, tabItem);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        Items.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (var item in e.NewItems)
                    {
                        var tabItem = GenerateComboBoxItem(item as PUListBoxItemModel);
                        Items[e.OldStartingIndex] = tabItem;
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    {
                        var tabItem = Items[e.OldStartingIndex];
                        Items.RemoveAt(e.OldStartingIndex);
                        Items.Insert(e.NewStartingIndex, tabItem);
                    }
                    break;
            }
            if(SelectedValue != null)
            {
                if (SelectedValuePath == SelectedValuePaths.Header)
                    SelectItemByContent(SelectedValue);
                else
                    SelectItemByValue(SelectedValue);
            }
        }

        private PUListBoxItem GenerateComboBoxItem(PUListBoxItemModel model)
        {
            var comboBoxItem = new PUListBoxItem()
            {
                Uid = model.Uid,
                Content = model.Header,
                Value = model.Value,
            };

            if (Items.Count == 0)
                comboBoxItem.IsSelected = true;

            model.PropertyChanged += delegate
            {
                comboBoxItem.Content = model.Header;
                comboBoxItem.Value = model.Value;
            };

            return comboBoxItem;
        }

        #endregion
    }
}
