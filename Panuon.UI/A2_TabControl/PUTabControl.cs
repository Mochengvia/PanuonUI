using Panuon.UI.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
                if (_tabPanel == null)
                {
                    var scrollViewer = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(this, 0), 0), 1), 0) as ScrollViewer;
                    _tabPanel = (scrollViewer.Content as VirtualizingStackPanel).Children[0] as TabPanel;
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

            var scrollViewer = (tabPanel.Parent as VirtualizingStackPanel).Parent as ScrollViewer;
           if(TabStripPlacement == Dock.Top || TabStripPlacement == Dock.Bottom)
            {
                if (e.Delta > 0)
                    scrollViewer.LineLeft();
                else
                    scrollViewer.LineRight();
            }
           else
            {
                if (e.Delta > 0)
                    scrollViewer.LineUp();
                else
                    scrollViewer.LineDown();
            }

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
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("该属性对此控件无效。请使用BindingItems属性替代。", true)]
        public new IEnumerable ItemsSource
        {
            get { return base.ItemsSource; }
            private set { base.ItemsSource = value; }
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("该属性对此控件无效。BindingItems属性中的Header属性即为要显示的内容。", true)]
        public new string DisplayMemberPath
        {
            get { return base.DisplayMemberPath; }
            private set { base.DisplayMemberPath = value; }
        }

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
        /// 获取或设置当子项设置为可删除时，用户点击删除按钮后应执行的操作。默认为删除项目并触发DeleteItem路由事件。
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
        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(PUTabControl));

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
        /// 获取被选中PUTabItem的Header或Value属性（这取决于SelectedValuePath），或反向选中子项目。
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
            {
                return;
            }
            if (e.NewValue == e.OldValue)
                return;

            var selectedItem = tabControl.SelectedItem as PUListBoxItem;
            foreach (var item in tabControl.Items)
            {
                var tabItem = item as PUTabItem;
                if ((tabControl.SelectedValuePath == SelectedValuePaths.Header ?
                    (tabItem.Content == null ? false : tabItem.Content.Equals(tabControl.SelectedValue)) :
                    (tabItem.Value == null ? false : tabItem.Value.Equals(tabControl.SelectedValue))))
                {
                    if (!tabItem.IsSelected)
                    {
                        tabItem.IsSelected = true;
                    }
                    return;
                }
            }

        }

        /// <summary>
        /// 若使用MVVM绑定，请使用此依赖属性。
        /// </summary>
        public ObservableCollection<PUTabItemModel> BindingItems
        {
            get { return (ObservableCollection<PUTabItemModel>)GetValue(BindingItemsProperty); }
            set { SetValue(BindingItemsProperty, value); }
        }

        public static readonly DependencyProperty BindingItemsProperty =
            DependencyProperty.Register("BindingItems", typeof(ObservableCollection<PUTabItemModel>), typeof(PUTabControl), new PropertyMetadata(OnBindingItemsChanged));

        private static void OnBindingItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tabControl = d as PUTabControl;
            if(tabControl.BindingItems != null)
            {
                tabControl.BindingItems.CollectionChanged -= tabControl.BindingItemChanged;
                tabControl.BindingItems.CollectionChanged += tabControl.BindingItemChanged;
            } 
            tabControl.GenerateBindindItems(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void BindingItemChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            GenerateBindindItems(e);
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

        /// <summary>
        /// 向左命令
        /// </summary>
        public ICommand UpCommand
        {
            get
            { return _upCommand; }
        }
        private ICommand _upCommand = new PUTabControlUpCommand();


        /// <summary>
        /// 向左命令
        /// </summary>
        public ICommand DownCommand
        {
            get
            { return _downCommand; }
        }
        private ICommand _downCommand = new PUTabControlDownCommand();

        #endregion

        #region APIs
        public void SelectItemByContent(object content)
        {
            var tabItem = GetItemByContent(content);
            if (tabItem != null)
                tabItem.IsSelected = true;
        }

        public void SelectItemByValue(object value)
        {
            var tabItem = GetItemByValue(value);
            if (tabItem != null)
                tabItem.IsSelected = true;
        }
        #endregion

        #region Function
        private void CheckSideButton()
        {
            if (tabPanel == null)
                return;
            if(TabStripPlacement == Dock.Top || TabStripPlacement == Dock.Bottom)
            {
                if (ActualWidth <= tabPanel.ActualWidth)
                {
                    SideButtonVisibility = Visibility.Visible;
                }
                else
                {
                    SideButtonVisibility = Visibility.Collapsed;
                }
            }
            else
            {
                if (ActualHeight <= tabPanel.ActualHeight)
                {
                    SideButtonVisibility = Visibility.Visible;
                }
                else
                {
                    SideButtonVisibility = Visibility.Collapsed;
                }
            }
        }

        private void GenerateBindindItems(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                   var _selectedValue = SelectedValue;
                    SelectedValue = null;
                    Items.Clear();
                    if (BindingItems == null)
                        break;
                    foreach (var item in BindingItems)
                    {
                        var tabItem = GenerateTabItem(item);
                        Items.Add(tabItem);
                    }
                    SelectedValue = _selectedValue;
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach(var item in e.NewItems)
                    {
                        var tabItem = GenerateTabItem(item as PUTabItemModel);
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
                        var tabItem = GenerateTabItem(item as PUTabItemModel);
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
            if (SelectedValue != null)
            {
                if (SelectedValuePath == SelectedValuePaths.Header)
                    SelectItemByContent(SelectedValue);
                else
                    SelectItemByValue(SelectedValue);
            }
        }

        private PUTabItem GenerateTabItem(PUTabItemModel model)
        {
            var tabItem = new PUTabItem()
            {
                Uid = model.Uid,
                Header = model.Header,
                Content = model.Content,
                Height = model.Height,
                Icon = model.Icon,
                Value = model.Value,
                CanDelete = model.CanDelete,
            };

            if (Items.Count == 0)
                tabItem.IsSelected = true;

            model.PropertyChanged += delegate
            {
                tabItem.Header = model.Header;
                tabItem.Content = model.Content;
                tabItem.Height = model.Height;
                tabItem.Icon = model.Icon;
                tabItem.Value = model.Value;
                tabItem.CanDelete = model.CanDelete;
            };

            return tabItem;
        }

        private PUTabItem GetItemByContent(object content)
        {
            foreach (var item in Items)
            {
                var tabItem = item as PUTabItem;
                if (tabItem == null)
                    throw new Exception("PUTabControl的子项必须是PUTabItem。");
                if (tabItem.Content.IsEqual(content))
                    return tabItem;
            }
            return null;
        }

        private PUTabItem GetItemByValue(object value)
        {
            foreach (var item in Items)
            {
                var tabItem = item as PUTabItem;
                if (tabItem == null)
                    throw new Exception("PUTabControl的子项必须是PUTabItem。");
                if (tabItem.Value.IsEqual(value))
                    return tabItem;
            }
            return null;
        }

        #endregion
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

    internal sealed class PUTabControlUpCommand : ICommand
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

            if (stkMain.VerticalOffset >= 20)
                stkMain.ScrollToVerticalOffset(stkMain.VerticalOffset - 20);
            else
                stkMain.ScrollToVerticalOffset(0);

        }
    }

    internal sealed class PUTabControlDownCommand : ICommand
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

            if (scrollViewer.VerticalOffset <= scrollViewer.ActualHeight - 20)
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + 20);
            else
                scrollViewer.ScrollToVerticalOffset(scrollViewer.ActualHeight);
        }
    }

}
