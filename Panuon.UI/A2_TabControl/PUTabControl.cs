using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PUTabControl : TabControl
    {
        #region Identity
        private TabPanel tabPanel
        {
            get
            {
                if(_tabPanel == null)
                {
                    var scrollViewer = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(this, 0), 0), 1), 0) as ScrollViewer;
                    _tabPanel = (scrollViewer.Content as StackPanel).Children[0] as TabPanel;
                }
                return _tabPanel;
            }
        }
        private TabPanel _tabPanel;

        #endregion

        static PUTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUTabControl), new FrameworkPropertyMetadata(typeof(PUTabControl)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            tabPanel.SizeChanged += delegate
            {
                CheckSideButton();
            };
            tabPanel.MouseWheel += ScrollViewer_MouseWheel;
        }

        #region Sys
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (SelectedValuePath == SelectedValuePaths.Header)
                SelectedValue = SelectedItem == null ? "" : (SelectedItem as PUTabItem).Header.ToString();
            else
                SelectedValue = SelectedItem == null ? null : (SelectedItem as PUTabItem).Value;
            base.OnSelectionChanged(e);
        }

        private void ScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var tabPanel = sender as TabPanel;

            var scrollViewer = (tabPanel.Parent as StackPanel).Parent as ScrollViewer;
            if (e.Delta > 0)
                    scrollViewer.LineLeft();
            else
                scrollViewer.LineRight();
          
            if (scrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible || scrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
                e.Handled = true;
        }
        #endregion

        #region RoutedEvent
        /// <summary>
        /// 用户点击删除按钮事件。
        /// </summary>
        public static readonly RoutedEvent DeleteItemEvent = EventManager.RegisterRoutedEvent("DeleteItem", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<PUTabItem>), typeof(PUTabControl));
        public event RoutedPropertyChangedEventHandler<PUTabItem> DeleteItem
        {
            add { AddHandler(DeleteItemEvent, value); }
            remove { RemoveHandler(DeleteItemEvent, value); }
        }
        internal void OnDeleteItem(PUTabItem oldItem, PUTabItem newItem)
        {
            RoutedPropertyChangedEventArgs<PUTabItem> arg = new RoutedPropertyChangedEventArgs<PUTabItem>(oldItem, newItem, DeleteItemEvent);
            RaiseEvent(arg);
        }

        #endregion

        #region Property

        /// <summary>
        /// 获取或设置选项卡的基础样式。默认值为General。
        /// </summary>
        public TabControlStyles TabControlStyle
        {
            get { return (TabControlStyles)GetValue(TabControlStyleProperty); }
            set { SetValue(TabControlStyleProperty, value); }
        }

        public static readonly DependencyProperty TabControlStyleProperty =
            DependencyProperty.Register("TabControlStyle", typeof(TabControlStyles), typeof(PUTabControl), new PropertyMetadata(TabControlStyles.General));

        /// <summary>
        /// 当子项目删除按钮可见时，用户点击删除按钮后的操作。默认为删除项目并触发DeleteItem路由事件。
        /// </summary>
        public DeleteModes DeleteMode
        {
            get { return (DeleteModes)GetValue(DeleteModeProperty); }
            set { SetValue(DeleteModeProperty, value); }
        }

        public static readonly DependencyProperty DeleteModeProperty =
            DependencyProperty.Register("DeleteMode", typeof(DeleteModes), typeof(PUTabControl), new PropertyMetadata(DeleteModes.Delete));

        /// <summary>
        /// 获取或设置当某个子项被选中时的前景色。默认值为灰黑色(#3E3E3E)。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }

        public static readonly DependencyProperty CoverBrushProperty =
            DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUTabControl));

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
            DependencyProperty.Register("SelectedValuePath", typeof(SelectedValuePaths), typeof(PUTabControl), new PropertyMetadata(SelectedValuePaths.Header));


        /// <summary>
        /// 获取被选中PUTabItem的Header或Value属性（这取决于SelectedValuePath），
        /// 或根据设置的SelectedValue来选中子项目。
        /// </summary>
        public new object SelectedValue
        {
            get { return (object)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        public new static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(object), typeof(PUTabControl), new PropertyMetadata("", OnSelectedValueChanged));

        private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tabControl = d as PUTabControl;
            if (tabControl.SelectedValue == null)
                return;
            var selectedItem = tabControl.SelectedItem as PUTabItem;

            if (selectedItem == null)
            {
                foreach (var item in tabControl.Items)
                {
                    var tabItem = item as PUTabItem;
                    if ((tabControl.SelectedValuePath == SelectedValuePaths.Header ?
                        (tabItem.Content == null ? false : tabItem.Content.ToString() == tabControl.SelectedValue.ToString()) :
                        (tabItem.Value == null ? false : tabItem.Value.Equals(tabControl.SelectedValue))))
                    {
                        if (!tabItem.IsSelected)
                            tabItem.IsSelected = true;
                        return;
                    }
                }
            }

        }

        /// <summary>
        /// 若使用MVVM绑定，请使用此依赖属性。
        /// </summary>
        public IList<PUTabItemModel> BindingItems
        {
            get { return (IList<PUTabItemModel>)GetValue(BindingItemsProperty); }
            set { SetValue(BindingItemsProperty, value); }
        }

        public static readonly DependencyProperty BindingItemsProperty =
            DependencyProperty.Register("BindingItems", typeof(IList<PUTabItemModel>), typeof(PUTabControl), new PropertyMetadata(null, OnBindingItemsChanged));

        private static void OnBindingItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tabControl = d as PUTabControl;
            var items = tabControl.BindingItems;
            if (items == null)
                return;
            tabControl.Items.Clear();

            foreach (var item in items)
            {
                var comboBoxItem = new PUTabItem()
                {
                    Header = item.Header,
                    Content = item.Content,
                    Height = item.Height,
                    Icon = item.Icon,
                    Value = item.Value,
                    DeleteButtonVisibility = item.CanDelete ? Visibility.Visible : Visibility.Collapsed,
                };

                if (tabControl.Items.Count == 0)
                    comboBoxItem.IsSelected = true;
                tabControl.Items.Add(comboBoxItem);
            }
        }
        #endregion

        #region Internal Property
        /// <summary>
        /// 是否允许显示选项卡两侧的按钮。
        /// </summary>
        internal Visibility SideButtonVisibility
        {
            get { return (Visibility)GetValue(SideButtonVisibilityProperty); }
            set { SetValue(SideButtonVisibilityProperty, value); }
        }

        internal static readonly DependencyProperty SideButtonVisibilityProperty =
            DependencyProperty.Register("SideButtonVisibility", typeof(Visibility), typeof(PUTabControl), new PropertyMetadata(Visibility.Collapsed));

        #endregion

        #region Command
        /// <summary>
        /// 向左命令
        /// </summary>
        public ICommand LeftCommand
        {
            get
            { return _leftCommand; }
        }
        private ICommand _leftCommand = new PUTabControlLeftCommand();

        /// <summary>
        /// 向右命令
        /// </summary>
        public ICommand RightCommand
        {
            get
            { return _rightCommand; }
        }
        private ICommand _rightCommand = new PUTabControlRightCommand();

        #endregion

        #region Function
        private void CheckSideButton()
        {
            if (tabPanel == null)
                return;
            if (ActualWidth <= tabPanel.ActualWidth)
            {
                SideButtonVisibility = Visibility.Visible;
            }
            else
            {
                SideButtonVisibility = Visibility.Collapsed;
            }
        }
        #endregion

        public enum TabControlStyles
        {
            General,
            Classic,
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

        public enum SelectedValuePaths
        {
            Header,
            Value
        }
    }

    internal sealed class PUTabControlLeftCommand : ICommand
    {
        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var stkMain = (parameter as ScrollViewer);

            if (stkMain.HorizontalOffset >= 20)
                stkMain.ScrollToHorizontalOffset(stkMain.HorizontalOffset - 20);
            else
                stkMain.ScrollToHorizontalOffset(0);

        }
    }

    internal sealed class PUTabControlRightCommand : ICommand
    {
        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var scrollViewer = (parameter as ScrollViewer);

            if (scrollViewer.HorizontalOffset <= scrollViewer.ActualWidth - 20)
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + 20);
            else
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.ActualWidth);
        }
    }

}
