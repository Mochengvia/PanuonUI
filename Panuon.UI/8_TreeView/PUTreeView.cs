using System;
using System.ComponentModel;
using System.Linq;
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
            RoutedPropertyChangedEventArgs<PUTreeViewItem> arg = new RoutedPropertyChangedEventArgs<PUTreeViewItem>(oldItem, newItem, ChoosedItemChangedEvent);
            RaiseEvent(arg);
        }
        #endregion

        #region Property

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


        public ChoosedValuePaths ChoosedValuePath
        {
            get { return (ChoosedValuePaths)GetValue(ChoosedValuePathProperty); }
            set { SetValue(ChoosedValuePathProperty, value); }
        }

        public static readonly DependencyProperty ChoosedValuePathProperty =
            DependencyProperty.Register("ChoosedValuePath", typeof(ChoosedValuePaths), typeof(PUTreeView), new PropertyMetadata(ChoosedValuePaths.Header));


        public enum ChoosedValuePaths
        {
            Header,Value
        }

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
                tvi.IsChoosed = true;
            else if (tvi == null)
            {
                treeView.ChoosedItem.IsChoosed = false;
                treeView.ChoosedItem.IsSelected = false;
                treeView.ChoosedItem = null;
                treeView.ChoosedValue = null;
            }
        }
        #endregion


        #region Function
        private PUTreeViewItem GetTreeViewItem(object value)
        {
            foreach(var item in Items)
            {
                var tvi = item as PUTreeViewItem;
                var tvix = GetTreeViewItem(tvi, value);
                if (tvix != null)
                    return tvix;
            }
            return null;
        }

        private PUTreeViewItem GetTreeViewItem(PUTreeViewItem item,object value)
        {
            if (ChoosedValuePath == ChoosedValuePaths.Header ? item.Header.Equals(value) : (item.Value == null ? false : item.Value.Equals(value)))
                return item;
            if (item.HasItems)
            {
                foreach (var tvi in item.Items)
                {
                    var tvix = GetTreeViewItem(tvi as PUTreeViewItem, value);
                    if (tvix != null)
                        return tvix;
                }
            }
            return null;
        }
        #endregion


    }
}
