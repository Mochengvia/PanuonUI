using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PUTreeViewItem : TreeViewItem
    {
        #region Identity
        private PUTreeView ParentTreeView
        {
            get
            {
                if (_parentTreeView == null)
                {
                    var parent = this.Parent;
                    while (parent != null && parent.GetType() != typeof(PUTreeView))
                    {
                        parent = (parent as PUTreeViewItem).Parent;
                    }
                    _parentTreeView = (parent as PUTreeView);
                }
                return _parentTreeView;
            }
        }
        private PUTreeView _parentTreeView;
        #endregion

        static PUTreeViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUTreeViewItem), new FrameworkPropertyMetadata(typeof(PUTreeViewItem)));
        }

        public override void OnApplyTemplate()
        {
            //初始选中时，将treeview的ChoosedItem设置为此
            if (IsChoosed)
            {
                var parent = this.Parent;
                while (parent != null && parent.GetType() != typeof(PUTreeView))
                {
                    parent = (parent as PUTreeViewItem).Parent;
                }
                var view = (parent as PUTreeView);
                view.ChoosedItem = this;
            }

            var stk = VisualTreeHelper.GetChild(this, 0) as StackPanel;
            stk.MouseLeftButtonDown += Stk_MouseLeftButtonDown;
        }

        #region Property
        /// <summary>
        /// 获取或设置该子项是否已被选中，含有子项目的行项目无法被选中。
        /// </summary>
        public bool IsChoosed
        {
            get { return (bool)GetValue(IsChoosedProperty); }
            set
            { SetValue(IsChoosedProperty, value); }
        }
        public static readonly DependencyProperty IsChoosedProperty =
            DependencyProperty.Register("IsChoosed", typeof(bool), typeof(PUTreeViewItem), new PropertyMetadata(false, OnIsChoosedChanged));

        private static void OnIsChoosedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as PUTreeViewItem;
            if (item.IsChoosed)
            {
                if (item.ParentTreeView.ChoosedItem != null)
                    item.ParentTreeView.ChoosedItem.IsChoosed = false;
                item.ParentTreeView.ChoosedItem = item;
            }
        }

        /// <summary>
        /// 获取或设置该子项可以携带的值，仅用于标记该子项的实际内容，不会对前端显示造成影响。
        /// </summary>
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(PUTreeViewItem));

        #endregion

        #region Sys
        private void Stk_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (!HasItems)
            {
                IsSelected = true;

                if (IsChoosed == false)
                {
                    IsChoosed = true;
                }
                else
                {
                    e.Handled = true;
                    return;
                }
            }
            else
            {
                IsSelected = false;
                if (!ParentTreeView.IsExpandDoubleClick)
                    IsExpanded = !IsExpanded;
            }
        }
        #endregion
    }

    /// <summary>
    /// 用于TreeView绑定的模型。
    /// </summary>

    public class PUTreeViewItemModel : INotifyPropertyChanged
    {
        protected internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #region Constructor
        public PUTreeViewItemModel()
        {
            Uid = Guid.NewGuid().ToString("N");
            Padding = new Thickness(10, 0, 0, 0);
        }
        #endregion

        #region Property
        /// <summary>
        /// 要显示的名称。可以作为SelectValuePath的值。
        /// </summary>
        public string Header
        {
            get { return _header; }
            set
            {
                _header = value; OnPropertyChanged("Header");
            }
        }
        private string _header = "";

        /// <summary>
        /// 该对象的值。可以作为SelectValuePath的值。必须是数字、字符串或布尔值，其他类型可能会导致选择内容出现错误。
        /// </summary>
        public object Value
        {
            get { return _value; }
            set
            {
                _value = value; OnPropertyChanged("Value");
            }
        }
        private object _value;


        /// <summary>
        /// 该项的子项目。
        /// </summary>
        public List<PUTreeViewItemModel> Items
        {
            get { return _items; }
            set
            {
                _items = value; OnPropertyChanged("Items");
            }
        }
        private List<PUTreeViewItemModel> _items;

        /// <summary>
        /// 缩进。默认值为10,0,0,0。
        /// </summary>
        public Thickness Padding
        {
            get { return _padding; }
            set
            {
                _padding = value; OnPropertyChanged("Padding");
            }
        }
        private Thickness _padding;
        #endregion

        #region Internal Property
        internal string Uid
        {
            get { return _uid; }
            set { _uid = value; }
        }
        private string _uid;


        #endregion

    }

}
