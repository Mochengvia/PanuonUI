/*==============================================================
*作者：ZEOUN
*时间：2018/10/17 15:18:59
*说明： 
*日志：2018/10/17 15:18:59 创建。
*==============================================================*/
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Panuon.UI
{
    public class PUTabControl : TabControl
    {
        static PUTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUTabControl), new FrameworkPropertyMetadata(typeof(PUTabControl)));

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (e.Delta > 0)
                if (scrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible)
                    scrollViewer.LineUp();
                else if (scrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
                    scrollViewer.LineLeft();
                else
                    return;
            else
                 if (scrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible)
                scrollViewer.LineDown();
            else if (scrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
                scrollViewer.LineRight();
            else
                return;

            if (scrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible || scrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
                e.Handled = true;
        }


        #region RoutedEvent
        /// <summary>
        /// 用户点击删除按钮事件。
        /// </summary>
        public static readonly RoutedEvent DeleteItemEvent = EventManager.RegisterRoutedEvent("DeleteItem", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<PUTabItem>), typeof(PUComboBox));
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
        /// 获取或设置是否允许显示选项卡两侧的按钮。
        /// </summary>
        public Visibility SideButtonVisibility
        {
            get { return (Visibility)GetValue(SideButtonVisibilityProperty); }
            set { SetValue(SideButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty SideButtonVisibilityProperty =
            DependencyProperty.Register("SideButtonVisibility", typeof(Visibility), typeof(PUTabControl), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// 获取或设置选项卡的基础样式。默认值为General。
        /// </summary>
        public TabControlStyles TabControlStyle
        {
            get { return (TabControlStyles)GetValue(TabControlStyleProperty); }
            set { SetValue(TabControlStyleProperty, value); }
        }

        public static readonly DependencyProperty TabControlStyleProperty =
            DependencyProperty.Register("TabControlStyle", typeof(TabControlStyles), typeof(PUTabControl), new PropertyMetadata(TabControlStyles.Classic));

        /// <summary>
        /// 当子项目删除按钮可见时，用户点击删除按钮后的操作。默认为删除项目并触发DeleteItem路由事件。
        /// </summary>
        public DeleteModes DeleteMode
        {
            get { return (DeleteModes)GetValue(DeleteModeProperty); }
            set { SetValue(DeleteModeProperty, value); }
        }

        public static readonly DependencyProperty DeleteModeProperty =
            DependencyProperty.Register("DeleteMode", typeof(DeleteModes), typeof(PUTabControl), new PropertyMetadata(DeleteModes.DeleteAndRouted));


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
            DeleteAndRouted,
            /// <summary>
            /// 当用户点击删除按钮时，不直接删除项目（只触发DeleteItem路由事件）。
            /// </summary>
            EventOnly,
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
