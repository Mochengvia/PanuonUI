using Panuon.UI.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PUTreeView : TreeView
    {
        static PUTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUTreeView), new FrameworkPropertyMetadata(typeof(PUTreeView)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #region Event
        /// <summary>
        /// 选择了新项目事件。
        /// </summary>
        public static readonly RoutedEvent ChoosedItemChangedEvent = EventManager.RegisterRoutedEvent("ChoosedItemChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<PUTreeViewItem>), typeof(PUTreeView));
        public event RoutedPropertyChangedEventHandler<PUTreeViewItem> ChoosedItemChanged
        {
            add { AddHandler(ChoosedItemChangedEvent, value); }
            remove { RemoveHandler(ChoosedItemChangedEvent, value); }
        }
        internal void OnChoosedItemChanged(PUTreeViewItem oldItem, PUTreeViewItem newItem)
        {
            RoutedPropertyChangedEventArgs<PUTreeViewItem> arg = new RoutedPropertyChangedEventArgs<PUTreeViewItem>(oldItem, newItem, ChoosedItemChangedEvent); RaiseEvent(arg);
        }
        #endregion

        #region Property
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("该属性对此控件无效。请使用BindingItems属性替代。", true)]
        public new IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            private set { SetValue(ItemsSourceProperty, value); }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("该属性对此控件无效。请该用ChoosedValuePath属性替代。", true)]
        public new string SelectedValuePath
        {
            get { return base.SelectedValuePath; }
            private set { base.SelectedValuePath = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ObsoleteAttribute("该属性对此控件无效。请该用ChoosedValue属性替代。", true)]
        public new object SelectedValue
        {
            get { return base.SelectedValue; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("该属性对此控件无效。BindingItems属性中的Header属性即为要显示的内容。", true)]
        public new string DisplayMemberPath
        {
            get { return base.DisplayMemberPath; }
            private set { base.DisplayMemberPath = value; }
        }

        /// <summary>
        /// 获取或设置树视图的基本样式，默认值为General。
        /// </summary>
        public TreeViewStyles TreeViewStyle
        {
            get { return (TreeViewStyles)GetValue(TreeViewStyleProperty); }
            set { SetValue(TreeViewStyleProperty, value); }
        }

        public static readonly DependencyProperty TreeViewStyleProperty =
            DependencyProperty.Register("TreeViewStyle", typeof(TreeViewStyles), typeof(PUTreeView), new PropertyMetadata(TreeViewStyles.General));


        /// <summary>
        /// 获取或设置子项目的单行元素高度，默认值为40。
        /// </summary>
        public double InnerHeight
        {
            get { return (double)GetValue(InnerHeightProperty); }
            set { SetValue(InnerHeightProperty, value); }
        }
        public static readonly DependencyProperty InnerHeightProperty =
            DependencyProperty.Register("InnerHeight", typeof(double), typeof(PUTreeView), new PropertyMetadata((double)40));

        /// <summary>
        /// 获取或设置鼠标悬浮时遮罩层的背景颜色，默认值为#22666666。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }
        public static readonly DependencyProperty CoverBrushProperty =
            DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUTreeView));

        /// <summary>
        /// 鼠标悬浮时遮罩层的背景颜色，默认值为#44666666。
        /// </summary>
        public Brush ChoosedBrush
        {
            get { return (Brush)GetValue(ChoosedBrushProperty); }
            set { SetValue(ChoosedBrushProperty, value); }
        }

        public static readonly DependencyProperty ChoosedBrushProperty =
            DependencyProperty.Register("ChoosedBrush", typeof(Brush), typeof(PUTreeView));

        /// <summary>
        /// 获取被选中的元素。
        /// </summary>
        public PUTreeViewItem ChoosedItem
        {
            get { return (PUTreeViewItem)GetValue(ChoosedItemProperty); }
            internal set { SetValue(ChoosedItemProperty, value); }
        }
        public static readonly DependencyProperty ChoosedItemProperty =
            DependencyProperty.Register("ChoosedItem", typeof(PUTreeViewItem), typeof(PUTreeView), new PropertyMetadata(OnChoosedItemChanged));

        private static void OnChoosedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeView = d as PUTreeView;
            if (treeView.ChoosedItem == null)
            {
                treeView.ChoosedValue = null;
            }
            else
            {
                var value = treeView.ChoosedValuePath == ChoosedValuePaths.Header ? treeView.ChoosedItem.Header : treeView.ChoosedItem.Value;
                if (treeView.ChoosedValue != value)
                {
                    treeView.isInternalSetChoosedValue = true;
                    treeView.ChoosedValue = value;
                }
            }
            treeView.OnChoosedItemChanged(e.OldValue as PUTreeViewItem, e.NewValue as PUTreeViewItem);
        }

        /// <summary>
        /// 获取或设置是否需要展开父项的方式，默认为Click。
        /// </summary>
        public ExpandModes ExpandMode
        {
            get { return (ExpandModes)GetValue(ExpandModeProperty); }
            set { SetValue(ExpandModeProperty, value); }
        }

        public static readonly DependencyProperty ExpandModeProperty =
            DependencyProperty.Register("ExpandMode", typeof(ExpandModes), typeof(PUTreeView), new PropertyMetadata(ExpandModes.Click));

        /// <summary>
        /// 该属性指定了当子项目被选中时，ChoosedValue应呈现子项目的哪一个值。
        /// 可选项为Header或Value，默认值为Header。
        /// </summary>
        public ChoosedValuePaths ChoosedValuePath
        {
            get { return (ChoosedValuePaths)GetValue(ChoosedValuePathProperty); }
            set { SetValue(ChoosedValuePathProperty, value); }
        }

        public static readonly DependencyProperty ChoosedValuePathProperty =
            DependencyProperty.Register("ChoosedValuePath", typeof(ChoosedValuePaths), typeof(PUTreeView), new PropertyMetadata(ChoosedValuePaths.Header));

        /// <summary>
        /// 获取被选中PUTreeViewItem的Header或Value属性（这取决于SelectedValuePath），
        /// 或根据设置的ChoosedValue来选中子项目。
        /// </summary>
        public object ChoosedValue
        {
            get { return (object)GetValue(ChoosedValueProperty); }
            set { SetValue(ChoosedValueProperty, value); }
        }

        public static readonly DependencyProperty ChoosedValueProperty =
            DependencyProperty.Register("ChoosedValue", typeof(object), typeof(PUTreeView), new PropertyMetadata(OnChoosedValueChanged));

        internal bool isInternalSetChoosedValue = false;
        private static void OnChoosedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeView = d as PUTreeView;
            if (treeView.ChoosedValue == null)
                return;

            if (treeView.isInternalSetChoosedValue)
            {
                treeView.isInternalSetChoosedValue = false;
                return;
            }

            var tvi = treeView.ChoosedValuePath == ChoosedValuePaths.Header ? treeView.GetItemByHeader(treeView.ChoosedValue, true, false) : treeView.GetItemByValue(treeView.ChoosedValue, true, false);

            if (tvi != null)
            {
                if (!tvi.IsChoosed)
                    tvi.IsChoosed = true;
            }
            else
            {
                if (treeView.ChoosedItem != null)
                {
                    treeView.ChoosedItem.IsChoosed = false;
                    treeView.ChoosedItem = null;
                }
            }
        }

        /// <summary>
        /// 用于TreeView的绑定。
        /// </summary>
        public ObservableCollection<PUTreeViewItemModel> BindingItems
        {
            get { return (ObservableCollection<PUTreeViewItemModel>)GetValue(BindingItemsProperty); }
            set { SetValue(BindingItemsProperty, value); }
        }

        public static readonly DependencyProperty BindingItemsProperty =
            DependencyProperty.Register("BindingItems", typeof(ObservableCollection<PUTreeViewItemModel>), typeof(PUTreeView), new PropertyMetadata(OnBindingItemsChanged));

        private static void OnBindingItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeView = d as PUTreeView;
            if (treeView.BindingItems != null)
            {
                treeView.BindingItems.CollectionChanged -= treeView.BindingItemChanged;
                treeView.BindingItems.CollectionChanged += treeView.BindingItemChanged;
            }
            treeView.GenerateBindindItems(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void BindingItemChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            GenerateBindindItems(e);
        }
        #endregion

        #region APIs
        /// <summary>
        /// 通过标题选中子项。
        /// 若标题不是值类型，则将逐一比较每一个可写属性的值是否相等。
        /// </summary>
        /// <param name="header">要匹配的标题。</param>
        public void ChooseItemByHeader(object header)
        {
            var tvi = GetItemByHeader(header, true, false);
            if (tvi != null)
            {
                if (!tvi.IsChoosed && !tvi.HasItems)
                    tvi.IsChoosed = true;
            }
            else if (tvi == null)
            {
                if (ChoosedItem != null)
                {
                    ChoosedItem.IsChoosed = false;
                    ChoosedItem = null;
                }
                ChoosedValue = null;
            }
        }

        /// <summary>
        /// 通过Value获取子项。
        /// 若Value不是值类型，则将逐一比较每一个可写属性的值是否相等。
        /// </summary>
        /// <param name="value">要匹配的值。</param>
        public void ChooseItemByValue(object value)
        {
            var tvi = GetItemByValue(value, true, false);
            if (tvi != null)
            {
                if (!tvi.IsChoosed && !tvi.HasItems)
                    tvi.IsChoosed = true;
            }
            else if (tvi == null)
            {
                if (ChoosedItem != null)
                {
                    ChoosedItem.IsChoosed = false;
                    ChoosedItem = null;
                }
                ChoosedValue = null;
            }
        }

        #endregion

        #region Function
        /// <summary>
        /// 通过标题获取Item。
        /// </summary>
        /// <param name="header">要匹配的标题。</param>
        /// <param name="autoExpand">在检索过程中是否自动折叠不是目标项的项目，并将目标项的父PUTreeViewItem展开。</param>
        /// <param name="includeParent">返回结果中是否包含含有子项的项目。</param>
        /// <returns></returns>
        private PUTreeViewItem GetItemByHeader(object header, bool autoExpand = false, bool includeParent = true)
        {
            foreach (var item in Items)
            {
                var tvi = item as PUTreeViewItem;
                if (autoExpand)
                    tvi.IsExpanded = false;

                var tvix = GetTreeViewItemByHeader(tvi, header, autoExpand, includeParent);
                if (tvix != null)
                {
                    if (autoExpand)
                    {
                        var parent = tvix.Parent as PUTreeViewItem;
                        if (parent != null)
                            parent.IsExpanded = true;
                    }
                    return tvix;
                }
            }
            return null;
        }

        /// <summary>
        /// 通过Value获取Item。
        /// </summary>
        /// <param name="value">要匹配的Value。</param>
        /// <param name="autoExpand">在检索过程中是否自动折叠不是目标项的项目，并将目标项的父PUTreeViewItem展开。</param>
        /// <param name="includeParent">返回结果中是否包含含有子项的项目。</param>
        /// <returns></returns>
        private PUTreeViewItem GetItemByValue(object value, bool autoExpand = false, bool includeParent = true)
        {
            foreach (var item in Items)
            {
                var tvi = item as PUTreeViewItem;
                if (autoExpand)
                    tvi.IsExpanded = false;

                var tvix = GetTreeViewItemByValue(tvi, value, autoExpand, includeParent);
                if (tvix != null)
                {
                    if (autoExpand)
                    {
                        var parent = tvix.Parent as PUTreeViewItem;
                        if (parent != null)
                            parent.IsExpanded = true;
                    }
                    return tvix;
                }
            }
            return null;
        }

        private PUTreeViewItem GetTreeViewItemByHeader(PUTreeViewItem item, object header, bool autoExpand, bool includeParent)
        {
            if ((includeParent || !item.HasItems) && item.Header != null && item.Header.IsEqual(header))
                return item;
            if (item.HasItems)
            {
                foreach (var tvi in item.Items)
                {
                    if (autoExpand)
                        (tvi as PUTreeViewItem).IsExpanded = false;
                    var tvix = GetTreeViewItemByHeader(tvi as PUTreeViewItem, header, autoExpand, includeParent);
                    if (tvix != null)
                    {
                        if (autoExpand)
                            (tvi as PUTreeViewItem).IsExpanded = true;
                        return tvix;
                    }
                }
            }
            return null;
        }

        private PUTreeViewItem GetTreeViewItemByValue(PUTreeViewItem item, object value, bool autoExpand, bool includeParent)
        {
            if ((includeParent || !item.HasItems) && item.Value != null && item.Value.IsEqual(value))
                return item;
            if (item.HasItems)
            {
                foreach (var tvi in item.Items)
                {
                    if (autoExpand)
                        (tvi as PUTreeViewItem).IsExpanded = false;
                    var tvix = GetTreeViewItemByValue(tvi as PUTreeViewItem, value, autoExpand, includeParent);
                    if (tvix != null)
                    {
                        if (autoExpand)
                            (tvi as PUTreeViewItem).IsExpanded = true;
                        return tvix;
                    }
                }
            }
            return null;
        }

        private void GenerateBindindItems(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    var _choosedValue = ChoosedValue;
                    ChoosedValue = null;
                    Items.Clear();
                    if (BindingItems == null)
                        break;
                    foreach (var item in BindingItems)
                    {
                        var treeViewItem = GenerateTreeViewItem(item);
                        Items.Add(treeViewItem);
                    }
                    ChoosedValue = _choosedValue;
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        var treeViewItem = GenerateTreeViewItem(item as PUTreeViewItemModel);
                        Items.Insert(e.NewStartingIndex, treeViewItem);
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
                        var tabItem = GenerateTreeViewItem(item as PUTreeViewItemModel);
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
            if (ChoosedValue != null)
            {
                if (ChoosedValuePath == ChoosedValuePaths.Header)
                    ChooseItemByHeader(ChoosedValue);
                else
                    ChooseItemByValue(ChoosedValue);
            }
        }

        private PUTreeViewItem GenerateTreeViewItem(PUTreeViewItemModel model)
        {
            var treeViewItem = new PUTreeViewItem()
            {
                Uid = model.Uid,
                Header = model.Header,
                Value = model.Value,
                Padding = model.Padding,
            };
            if (model.ToolTip != null)
                treeViewItem.ToolTip = model.ToolTip;

            foreach (var child in Generate(model.Items))
            {
                treeViewItem.Items.Add(child);
            }

            model.PropertyChanged += delegate
            {
                treeViewItem.Header = model.Header;
                treeViewItem.Value = model.Value;
                treeViewItem.Padding = model.Padding;
                if (model.ToolTip != null)
                    treeViewItem.ToolTip = model.ToolTip;

                treeViewItem.Items.Clear();
                foreach (var child in Generate(model.Items))
                {
                    treeViewItem.Items.Add(child);
                }
            };
            return treeViewItem;
        }

        private IEnumerable<PUTreeViewItem> Generate(IList<PUTreeViewItemModel> models)
        {
            if (models == null || models.Count == 0)
                yield break;

            foreach (var model in models)
            {
                var item = new UI.PUTreeViewItem()
                {
                    Uid = model.Uid,
                    Header = model.Header,
                    Value = model.Value,
                    Padding = model.Padding,
                };
                if (model.ToolTip != null)
                    item.ToolTip = model.ToolTip;

                foreach (var child in Generate(model.Items))
                {
                    item.Items.Add(child);
                }
                yield return item;
            }

        }
        #endregion


    }
}
