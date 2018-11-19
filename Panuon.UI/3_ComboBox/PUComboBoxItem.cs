/*==============================================================
*作者：ZEOUN
*时间：2018/9/27 16:00:11
*说明： 
*日志：2018/9/27 16:00:11 创建。
*==============================================================*/
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PUComboBoxItem : ComboBoxItem
    {

        static PUComboBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUComboBoxItem), new FrameworkPropertyMetadata(typeof(PUComboBoxItem)));
        }

        #region Property

        /// <summary>
        /// 是否显示删除按钮，默认值为Collapsed（不显示）。
        /// </summary>
        public Visibility DeleteButtonVisibility
        {
            get { return (Visibility)GetValue(DeleteButtonVisibilityProperty); }
            set { SetValue(DeleteButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty DeleteButtonVisibilityProperty =
            DependencyProperty.Register("DeleteButtonVisibility", typeof(Visibility), typeof(PUComboBoxItem), new PropertyMetadata(Visibility.Collapsed));


        /// <summary>
        /// 用以标记该项目，或该项目可以额外携带的内容。
        /// </summary>
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(PUComboBoxItem));


        #endregion

        public ICommand DeleteCommand
        {
            get
            { return _deleteCommand; }
        }
        private ICommand _deleteCommand = new PUComboBoxDeleteCommand();
    }

    internal sealed class PUComboBoxDeleteCommand : ICommand
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
            var comItem = (parameter as PUComboBoxItem);
            var combox = comItem.Parent as PUComboBox;
            if (combox.DeleteMode == PUComboBox.DeleteModes.Delete)
                combox.Items.Remove(comItem);
            combox.OnDeleteItem(null, comItem);
        }
    }

    /// <summary>
    /// 用于ComboBox绑定的模型。
    /// </summary>

    public class PUComboBoxItemModel : INotifyPropertyChanged
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
    }

}
