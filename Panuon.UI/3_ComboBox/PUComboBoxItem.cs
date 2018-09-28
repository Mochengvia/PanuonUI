/*==============================================================
*作者：ZEOUN
*时间：2018/9/27 16:00:11
*说明： 
*日志：2018/9/27 16:00:11 创建。
*==============================================================*/
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;

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
        /// 鼠标悬浮时的背景颜色，默认值为浅灰色(#DDDDDD)。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }
        public static readonly DependencyProperty CoverBrushProperty =
            DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUComboBoxItem), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEEEEE"))));

        /// <summary>
        /// 项目被选中时的背景颜色，默认值为浅灰色(#CCCCCC)。
        /// </summary>
        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(PUComboBoxItem), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DDDDDD"))));

        /// <summary>
        /// 是否显示删除按钮，默认为False（不显示）。
        /// </summary>
        public bool IsDeleteButtonShow
        {
            get { return (bool)GetValue(IsDeleteButtonShowProperty); }
            set { SetValue(IsDeleteButtonShowProperty, value); }
        }

        public static readonly DependencyProperty IsDeleteButtonShowProperty =
            DependencyProperty.Register("IsDeleteButtonShow", typeof(bool), typeof(PUComboBoxItem), new PropertyMetadata(false));


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
            if (combox.DeleteMode == PUComboBox.DeleteModes.DeleteItem)
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
        private object _value = 0;

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
        /// 鼠标悬浮时的背景颜色，若不设置，则使用默认值(#DDDDDD)。
        /// </summary>
        public Brush CoverBrush { get; set; }

        /// <summary>
        /// 项目被选中时的背景颜色，若不设置，则使用默认值(#CCCCCC)。
        /// </summary>
        public Brush SelectedBrush { get; set; }

    }

}
