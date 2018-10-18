/*==============================================================
*作者：ZEOUN
*时间：2018/10/17 15:25:14
*说明： 
*日志：2018/10/17 15:25:14 创建。
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
    public class PUTabItem : TabItem
    {
        static PUTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUTabItem), new FrameworkPropertyMetadata(typeof(PUTabItem)));
        }

        #region Property
        /// <summary>
        /// 获取或设置删除按钮的显示状态。
        /// </summary>
        public Visibility DeleteButtonVisibility
        {
            get { return (Visibility)GetValue(DeleteButtonVisibilityProperty); }
            set { SetValue(DeleteButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty DeleteButtonVisibilityProperty =
            DependencyProperty.Register("DeleteButtonVisibility", typeof(Visibility), typeof(PUTabItem), new PropertyMetadata(Visibility.Collapsed));

        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), typeof(PUTabItem));



        #endregion

        public ICommand DeleteCommand
        {
            get
            { return _deleteCommand; }
        }
        private ICommand _deleteCommand = new PUTabItemDeleteCommand();
    }

    internal sealed class PUTabItemDeleteCommand : ICommand
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
            var tabItem = (parameter as PUTabItem);
            var tabControl = tabItem.Parent as PUTabControl;
            if (tabControl.DeleteMode == PUTabControl.DeleteModes.DeleteAndRouted)
                tabControl.Items.Remove(tabItem);
            tabControl.OnDeleteItem(null, tabItem);
        }
    }
}
