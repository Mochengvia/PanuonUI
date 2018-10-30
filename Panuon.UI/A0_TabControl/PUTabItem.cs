/*==============================================================
*作者：ZEOUN
*时间：2018/10/17 15:25:14
*说明： 
*日志：2018/10/17 15:25:14 创建。
*==============================================================*/
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;

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
        /// 获取或设置该子项可以携带的值，仅用于标记该子项的实际内容，不会对前端显示造成影响。
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
    }

    public class PUTabItemModel : INotifyPropertyChanged
    {
        protected internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

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
        /// 显示在标题前的图标，为空时不显示。
        /// </summary>
        public object Icon
        {
            get { return _icon; }
            set
            {
                _icon = value; OnPropertyChanged("Icon");
            }
        }
        private object _icon;

        /// <summary>
        /// 是否显示删除按钮。
        /// </summary>
        public bool CanDelete
        {
            get { return _canDelete; }
            set
            {
                _canDelete = value; OnPropertyChanged("CanDelete");
            }
        }
        private bool _canDelete = false;


        /// <summary>
        /// 该对象的值。可以作为SelectValuePath的值。必须是数字、字符串或布尔值，其他类型可能会导致选择内容出现错误。
        /// </summary>
        public object Content
        {
            get { return _content; }
            set
            {
                _content = value; OnPropertyChanged("Content");
            }
        }
        private object _content = 0;

        /// <summary>
        /// 高度，默认值为30。
        /// </summary>
        public double Height
        {
            get { return _height; }
            set
            {
                _height = value; OnPropertyChanged("Height");
            }
        }
        private double _height = 30;
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
