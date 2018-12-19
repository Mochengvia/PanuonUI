using System;
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
            base.OnApplyTemplate();

            if (IsChoosed)
            {
                ParentTreeView.ChoosedItem = this;
            }

            var stk = VisualTreeHelper.GetChild(this, 0) as StackPanel;
                stk.MouseLeftButtonDown += Stk_MouseLeftButtonDown;
        }

        #region Property
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("请使用IsChoosed属性。")]
        public new bool IsSelected
        {
            get { return base.IsSelected; }
            internal set { base.IsSelected = value; }
        }


        /// <summary>
        /// 获取或设置该子项是否已被选中，含有子项目的行项目无法被选中。
        /// </summary>
        public bool IsChoosed
        {
            get { return (bool)GetValue(IsChoosedProperty); }
            set { SetValue(IsChoosedProperty, value); }
        }
        public static readonly DependencyProperty IsChoosedProperty =
            DependencyProperty.Register("IsChoosed", typeof(bool), typeof(PUTreeViewItem), new PropertyMetadata(false, OnIsChoosedChanged));

        private static void OnIsChoosedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as PUTreeViewItem;
            if (item.IsChoosed)
            {
                if(item.ParentTreeView.ChoosedItem != null)
                    item.ParentTreeView.ChoosedItem.IsChoosed = false;
                item.ParentTreeView.ChoosedItem = item;
            }
        }

        /// <summary>
        /// 获取或设置该子项可以携带的值，不会对前端显示造成影响。
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
                if (ParentTreeView.ExpandMode == ExpandModes.Click)
                    IsExpanded = !IsExpanded;
            }
        }
        #endregion
    }

}
