using System;
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

        #region Property
        /// <summary>
        /// 行元素高度，默认值为40。
        /// </summary>
        public double InnerHeight
        {
            get { return (double)GetValue(InnerHeightProperty); }
            set { SetValue(InnerHeightProperty, value); }
        }
        public static readonly DependencyProperty InnerHeightProperty = DependencyProperty.Register("InnerHeight", typeof(double), typeof(PUTreeViewItem), new PropertyMetadata((double)40));


        /// <summary>
        /// 该元素是否被选择（即使上级子元素没有展开）。使用此属性而非IsSelected
        /// </summary>
        public bool IsChoosed
        {
            get { return (bool)GetValue(IsChoosedProperty); }
            set
            {SetValue(IsChoosedProperty, value);}
        }
        public static readonly DependencyProperty IsChoosedProperty = 
            DependencyProperty.Register("IsChoosed", typeof(bool), typeof(PUTreeViewItem), new PropertyMetadata(false,OnIsChoosedChanged));

        private static void OnIsChoosedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as PUTreeViewItem;
            if (item.IsChoosed == true)
            {
                if (item.ParentTreeView.ChoosedItem != null)
                    item.ParentTreeView.ChoosedItem.IsChoosed = false;
                item.ParentTreeView.ChoosedItem = item;
                item.ParentTreeView.ChoosedValue = (item.ParentTreeView.ChoosedValuePath == PUTreeView.ChoosedValuePaths.Header ? item.Header : item.Value);
            }
        }

        /// <summary>
        /// Icon
        /// </summary>
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = 
            DependencyProperty.Register("Icon", typeof(string), typeof(PUTreeViewItem), new PropertyMetadata(""));


        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(PUTreeViewItem));


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


    }
}
