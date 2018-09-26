using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
        public static readonly RoutedEvent ChoosedItemChangedEvent = EventManager.RegisterRoutedEvent("ChoosedItemChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<PUTreeView>), typeof(PUTreeView));
        public event RoutedPropertyChangedEventHandler<string> ChoosedItemChanged
        {
            add { AddHandler(ChoosedItemChangedEvent, value); }
            remove { RemoveHandler(ChoosedItemChangedEvent, value); }
        }
        internal void OnChoosedItemChanged()
        {
            RoutedPropertyChangedEventArgs<PUTreeView> arg = new RoutedPropertyChangedEventArgs<PUTreeView>(this, this, ChoosedItemChangedEvent);
            RaiseEvent(arg);
        }
        #endregion

        #region Property
        /// <summary>
        /// 请使用本属性获取被选中的元素，而不是SelectedItem。
        /// </summary>
        [Bindable(true, BindingDirection.TwoWay)]
        public PUTreeViewItem ChoosedItem
        {
            get { return (PUTreeViewItem)GetValue(ChoosedItemProperty); }
            set { SetValue(ChoosedItemProperty, value); }
        }
        public static readonly DependencyProperty ChoosedItemProperty = DependencyProperty.Register("ChoosedItem", typeof(PUTreeViewItem), typeof(PUTreeViewItem), new PropertyMetadata(null));

        /// <summary>
        /// 是否需要双击才展开列表，默认为False。
        /// </summary>
        public bool IsExpandDoubleClick
        {
            get { return (bool)GetValue(IsExpandDoubleClickProperty); }
            set { SetValue(IsExpandDoubleClickProperty, value); }
        }
        public static readonly DependencyProperty IsExpandDoubleClickProperty = DependencyProperty.Register("IsExpandDoubleClick", typeof(bool), typeof(PUTreeViewItem), new PropertyMetadata(false));
        #endregion

        #region APIs

        /// <summary>
        /// 通过Uid将控件设为选中状态，最多支持三层树嵌套。
        /// </summary>
        /// <param name="id">被选中控件的Uid名称。</param>
        /// <param name="activeSelectEvent">是否激活TreeView的Selected事件。</param>
        public void ChooseItemByUID(string uid, bool activeChangeEvent = true)
        {
            if (_item != null)
            {
                _item.IsChoosed = false;
                _item.IsSelected = false;
                _item = null;
            }
            GetChildItemByUID(Items, uid);
            if (_item == null)
                return;
            var parent = _item.Parent;
            while (parent != null && parent.GetType() == typeof(PUTreeViewItem))
            {
                (parent as PUTreeViewItem).IsExpanded = true;
                parent = (parent as PUTreeViewItem).Parent;
            }
            _item.IsSelected = true;
            _item.IsChoosed = true;
            ChoosedItem = _item;
            if (activeChangeEvent)
                OnChoosedItemChanged();
        }

        /// <summary>
        /// 通过Uid获取控件。
        /// </summary>
        public PUTreeViewItem GetItemByUID(string uid)
        {
            _item = null;
            GetChildItemByUID(Items, uid);
            return _item;
        }

        /// <summary>
        /// 通过标题选中控件。
        /// </summary>
        /// <param name="header">被选中控件的标题。</param>
        /// <param name="activeSelectEvent">是否激活TreeView的Selected事件。</param>
        public void ChooseItemByHeader(object header, bool activeChangeEvent = true)
        {
            if (_item != null)
            {
                _item.IsChoosed = false;
                _item.IsSelected = false;
                _item = null;
            }
            GetChildItemByHeader(Items, header);
            if (_item == null)
                return;
            var parent = _item.Parent;
            while (parent != null && parent.GetType() == typeof(PUTreeViewItem))
            {
                (parent as PUTreeViewItem).IsExpanded = true;
                parent = (parent as PUTreeViewItem).Parent;
            }
            _item.IsSelected = true;
            _item.IsChoosed = true;
            ChoosedItem = _item;
            if (activeChangeEvent)
                OnChoosedItemChanged();
        }
        #endregion

        #region Function
        private PUTreeViewItem _item = null;
        private void GetChildItemByUID(ItemCollection items, string uid)
        {
            foreach (var item in items)
            {
                var tvi = item as PUTreeViewItem;
                foreach (var itemx in tvi.Items)
                {
                    GetChildItemByUID(tvi.Items, uid);
                    var tvix = item as PUTreeViewItem;
                    foreach (var itemxx in tvix.Items)
                        GetChildItemByUID(tvix.Items, uid);
                }
                if ((tvi as PUTreeViewItem).Uid == uid)
                {
                    _item = (tvi as PUTreeViewItem);
                    return;
                }
            }
        }

        private void GetChildItemByHeader(ItemCollection items, object header)
        {
            foreach (var item in items)
            {
                var tvi = item as PUTreeViewItem;
                while (tvi.Items != null && tvi.Items.Count != 0)
                {
                    GetChildItemByHeader(tvi.Items, header);
                    return;
                }
                if ((tvi as PUTreeViewItem).Header == header)
                {
                    _item = (tvi as PUTreeViewItem);
                }

            }
        }

        #endregion


    }
}
