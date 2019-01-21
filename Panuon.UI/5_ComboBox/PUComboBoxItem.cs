using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Panuon.UI
{
    public class PUComboBoxItem : ComboBoxItem
    {

        public PUComboBoxItem()
        {

        }

        static PUComboBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUComboBoxItem), new FrameworkPropertyMetadata(typeof(PUComboBoxItem)));
        }

        #region Property
        /// <summary>
        /// 获取或设置是否显示删除按钮，默认值为False（不显示）。
        /// </summary>
        public bool CanDelete
        {
            get { return (bool)GetValue(CanDeleteProperty); }
            set { SetValue(CanDeleteProperty, value); }
        }

        public static readonly DependencyProperty CanDeleteProperty =
            DependencyProperty.Register("CanDelete", typeof(bool), typeof(PUComboBoxItem));


        /// <summary>
        /// 获取或设置用以标记该项目的值。默认值为Null。
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
            if (combox.DeleteMode == DeleteModes.Delete)
            {
                if (combox.BindingItems != null && !String.IsNullOrEmpty((comItem.Uid)))
                {
                    var model = combox.BindingItems.FirstOrDefault(x => x.Uid == comItem.Uid);
                    if (model != null)
                        combox.BindingItems.Remove(model);
                    else
                        combox.Items.Remove(comItem);
                }
                else
                {
                    combox.Items.Remove(comItem);
                }
            }
            combox.OnDeleteItem(null, comItem);
        }
    }
}
