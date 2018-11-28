using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        /// 子项目行元素高度，默认值为40。
        /// </summary>
        public double InnerHeight
        {
            get { return (double)GetValue(InnerHeightProperty); }
            set { SetValue(InnerHeightProperty, value); }
        }
        public static readonly DependencyProperty InnerHeightProperty =
            DependencyProperty.Register("InnerHeight", typeof(double), typeof(PUTreeView), new PropertyMetadata((double)40));

        /// <summary>
        /// 鼠标悬浮时遮罩层的背景颜色，默认值为#22666666。
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
        /// 请使用本属性获取被选中的元素，而不是SelectedItem。
        /// </summary>
        public PUTreeViewItem ChoosedItem
        {
            get { return (PUTreeViewItem)GetValue(ChoosedItemProperty); }
            set { SetValue(ChoosedItemProperty, value); }
        }
        public static readonly DependencyProperty ChoosedItemProperty =
            DependencyProperty.Register("ChoosedItem", typeof(PUTreeViewItem), typeof(PUTreeView), new PropertyMetadata(OnChoosedItemChanged));

        private static void OnChoosedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeView = d as PUTreeView;
            treeView.OnChoosedItemChanged(e.OldValue as PUTreeViewItem, e.NewValue as PUTreeViewItem);
        }

        /// <summary>
        /// 是否需要双击才展开列表，默认为False。
        /// </summary>
        public bool IsExpandDoubleClick
        {
            get { return (bool)GetValue(IsExpandDoubleClickProperty); }
            set { SetValue(IsExpandDoubleClickProperty, value); }
        }
        public static readonly DependencyProperty IsExpandDoubleClickProperty =
            DependencyProperty.Register("IsExpandDoubleClick", typeof(bool), typeof(PUTreeView), new PropertyMetadata(false));

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


        public enum ChoosedValuePaths
        {
            Header, Value
        }

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

            if (!treeView.IsLoaded || treeView.ChoosedValue == null)
                return;

            if (treeView.isInternalSetChoosedValue)
            {
                treeView.isInternalSetChoosedValue = false;
                return;
            }

            var tvi = treeView.ChoosedValuePath == ChoosedValuePaths.Header ? treeView.GetTreeViewItemByHeader(treeView.ChoosedValue) : treeView.GetTreeViewItemByValue(treeView.ChoosedValue);

            if (tvi != null)
            {
                if ( !tvi.IsChoosed && !tvi.HasItems)
                    tvi.IsChoosed = true;
               
            }
            else if (tvi == null)
            {
                if (treeView.ChoosedItem != null)
                {
                    treeView.ChoosedItem.IsChoosed = false;
                    treeView.ChoosedItem.IsSelected = false;
                    treeView.ChoosedItem = null;
                }
                treeView.ChoosedValue = null;
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
            if (treeView.ChoosedValue != null)
            {
                var tvi = treeView.ChoosedValuePath == ChoosedValuePaths.Header ? treeView.GetTreeViewItemByHeader(treeView.ChoosedValue) : treeView.GetTreeViewItemByValue(treeView.ChoosedValue);

                if (tvi != null)
                {
                    if (!tvi.IsChoosed && !tvi.HasItems)
                        tvi.IsChoosed = true;
                }
                else if (tvi == null)
                {
                    if (treeView.ChoosedItem != null)
                    {
                        treeView.ChoosedItem.IsChoosed = false;
                        treeView.ChoosedItem.IsSelected = false;
                        treeView.ChoosedItem = null;
                    }
                    treeView.ChoosedValue = null;
                }
            }
        }
        private void BindingItemChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            GenerateBindindItems(e);
        }

        #endregion

        #region APIs
        /// <summary>
        /// 通过标题获取子项。
        /// </summary>
        /// <param name="headerOrValue"></param>
        /// <returns></returns>
        public PUTreeViewItem GetTreeViewItemByHeader(object header)
        {
            PUTreeViewItem target = null;
            foreach (var item in Items)
            {
                var tvi = item as PUTreeViewItem;
                tvi.IsExpanded = false;

                var tvix = GetTreeViewItemByHeader(tvi, header);
                if (tvix != null)
                {
                    var parent = tvix.Parent as PUTreeViewItem;
                    if (parent != null)
                    {
                        parent.IsExpanded = true;
                    }
                    target = tvix;
                }
            }
            return target;
        }

        /// <summary>
        /// 通过Value获取子项。
        /// </summary>
        /// <param name="headerOrValue"></param>
        /// <returns></returns>
        public PUTreeViewItem GetTreeViewItemByValue(object value)
        {
            PUTreeViewItem target = null;
            foreach (var item in Items)
            {
                var tvi = item as PUTreeViewItem;
                tvi.IsExpanded = false;

                var tvix = GetTreeViewItemByValue(tvi, value);
                if (tvix != null)
                {
                    var parent = tvix.Parent as PUTreeViewItem;
                    if (parent != null)
                    {
                        parent.IsExpanded = true;
                    }
                    target = tvix;
                }
            }
            return target;
        }
        #endregion

        #region Function
        private PUTreeViewItem GetTreeViewItemByHeader(PUTreeViewItem item, object header)
        {
            if (item.Header != null && item.Header.Equals(header))
                return item;
            if (item.HasItems)
            {
                foreach (var tvi in item.Items)
                {
                    (tvi as PUTreeViewItem).IsExpanded = false;
                    var tvix = GetTreeViewItemByHeader(tvi as PUTreeViewItem, header);
                    if (tvix != null)
                    {
                        (tvi as PUTreeViewItem).IsExpanded = true;
                        return tvix;
                    }
                }
            }
            return null;
        }

        private PUTreeViewItem GetTreeViewItemByValue(PUTreeViewItem item, object value)
        {
            if (item.Value != null && item.Value.Equals(value))
                return item;
            if (item.HasItems)
            {
                foreach (var tvi in item.Items)
                {
                    (tvi as PUTreeViewItem).IsExpanded = false;
                    var tvix = GetTreeViewItemByValue(tvi as PUTreeViewItem, value);
                    if (tvix != null)
                    {
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
                    Items.Clear();
                    if (BindingItems == null)
                        break;
                    foreach (var item in BindingItems)
                    {
                        var tabItem = GenerateTreeViewItem(item);
                        Items.Add(tabItem);
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        var tabItem = GenerateTreeViewItem(item as PUTreeViewItemModel);
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

            foreach (var child in Generate(model.Items))
            {
                treeViewItem.Items.Add(child);
            }

            model.PropertyChanged += delegate
            {
                treeViewItem.Header = model.Header;
                treeViewItem.Value = model.Value;
                treeViewItem.Padding = model.Padding;
                treeViewItem.Items.Clear();
                foreach (var child in Generate(model.Items))
                {
                    treeViewItem.Items.Add(child);
                }
            };
            return treeViewItem;
        }

        private IList<PUTreeViewItem> Generate(IList<PUTreeViewItemModel> models)
        {
            if (models == null || models.Count == 0)
                return new List<PUTreeViewItem>();

            var itemList = new List<PUTreeViewItem>();
            foreach (var model in models)
            {
                var item = new UI.PUTreeViewItem()
                {
                    Uid = model.Uid,
                    Header = model.Header,
                    Value = model.Value,
                    Padding = model.Padding,
                };
                foreach (var child in Generate(model.Items))
                {
                    item.Items.Add(child);
                }
                itemList.Add(item);
            }

            return itemList;
        }
        #endregion


    }
}
