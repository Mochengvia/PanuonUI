using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
        public bool CanDelete
        {
            get { return (bool)GetValue(CanDeleteProperty); }
            set { SetValue(CanDeleteProperty, value); }
        }

        public static readonly DependencyProperty CanDeleteProperty =
            DependencyProperty.Register("CanDelete", typeof(bool), typeof(PUTabItem), new PropertyMetadata(false));

        /// <summary>
        /// 获取或设置显示在选项卡前的图标，默认值为空。
        /// </summary>
        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), typeof(PUTabItem));

        /// <summary>
        /// 获取或设置该子项可以携带的值，不会对前端显示造成影响。
        /// </summary>
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(PUTabItem));
        #endregion

        public ICommand DeleteCommand
        {
            get
            { return _deleteCommand; }
        }
        private ICommand _deleteCommand = new PUTabItemDeleteCommand();


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
                if (tabControl.DeleteMode == DeleteModes.Delete)
                {
                    if (tabControl.BindingItems != null && !String.IsNullOrEmpty((tabItem.Uid)))
                    {
                        var model = tabControl.BindingItems.FirstOrDefault(x => x.Uid == tabItem.Uid);
                        if (model != null)
                            tabControl.BindingItems.Remove(model);
                        else
                            tabControl.Items.Remove(tabItem);
                    }
                    else
                    {
                        tabControl.Items.Remove(tabItem);
                    }
                }
                tabControl.OnDeleteItem(null, tabItem);
            }
        }
    }
}
