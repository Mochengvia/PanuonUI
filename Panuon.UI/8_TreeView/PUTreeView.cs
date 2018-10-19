using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
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
            RoutedPropertyChangedEventArgs<PUTreeViewItem> arg = new RoutedPropertyChangedEventArgs<PUTreeViewItem>(oldItem, newItem, ChoosedItemChangedEvent);   RaiseEvent(arg);
        }
        #endregion

        #region Property
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

        private static void OnChoosedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeView = d as PUTreeView;
            if (!treeView.IsLoaded || treeView.ChoosedValue == null)
                return;

            var tvi = treeView.GetTreeViewItem(treeView.ChoosedValue);

            if (tvi != null && !tvi.IsChoosed)
            {
                tvi.IsChoosed = true;
                var parent = tvi.Parent as PUTreeViewItem;
                if (parent != null)
                {
                    parent.IsExpanded = true;
                }
            }
            else if (tvi == null)
            {
                treeView.ChoosedItem.IsChoosed = false;
                treeView.ChoosedItem.IsSelected = false;
                treeView.ChoosedItem = null;
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
            var items = treeView.BindingItems;
            if (items == null)
                return;
            treeView.Items.Clear();

            foreach (var item in items)
            {
                var treeViewItem = new PUTreeViewItem()
                {
                    Header = item.Header,
                    Value = item.Value,
                };
                treeView.Items.Add(treeViewItem);
                if (item.Items != null && item.Items.Count != 0)
                    treeView.AppendItem(item, treeViewItem, 2);
            }
        }

        #endregion


        #region APIs
        /// <summary>
        /// 通过标题或Value值选中目标子项。参数应该是Header还是Value，取决于ChoosedValuePath的值（默认为Header）。
        /// </summary>
        /// <param name="headerOrValue"></param>
        /// <returns></returns>
        public PUTreeViewItem GetTreeViewItem(object headerOrValue)
        {
            PUTreeViewItem target = null;
            foreach (var item in Items)
            {
                var tvi = item as PUTreeViewItem;
                tvi.IsExpanded = false;

                var tvix = GetTreeViewItem(tvi, headerOrValue);
                if (tvix != null)
                {
                    tvi.IsExpanded = true;
                    target = tvix;
                }
            }
            return target;
        }


        #endregion


        #region Function
        private void AppendItem(PUTreeViewItemModel model, PUTreeViewItem parent, int deepth)
        {

            foreach (var item in model.Items)
            {
                var treeViewItem = new PUTreeViewItem()
                {
                    Header = item.Header,
                    Value = item.Value,
                    Padding = new Thickness(Padding.Left * 2, 0, 0, 0),
                };
                parent.Items.Add(treeViewItem);
                if (item.Items != null && item.Items.Count != 0)
                    AppendItem(item, treeViewItem, deepth + 1);
            }
        }
        private PUTreeViewItem GetTreeViewItem(PUTreeViewItem item, object value)
        {
            if (ChoosedValuePath == ChoosedValuePaths.Header ? item.Header.Equals(value) : (item.Value == null ? false : item.Value.Equals(value)))
                return item;
            if (item.HasItems)
            {
                foreach (var tvi in item.Items)
                {
                    (tvi as PUTreeViewItem).IsExpanded = false;
                    var tvix = GetTreeViewItem(tvi as PUTreeViewItem, value);
                    if (tvix != null)
                    {
                        (tvi as PUTreeViewItem).IsExpanded = true;
                        return tvix;
                    }
                }
            }
            return null;
        }
        #endregion
    }
}
