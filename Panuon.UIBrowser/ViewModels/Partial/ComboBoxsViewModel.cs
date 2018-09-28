using Caliburn.Micro;
using Panuon.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class ComboBoxsViewModel : Screen, IShell
    {
        public ComboBoxsViewModel()
        {

        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            var comboList = new List<PUComboBoxItemModel>();
            comboList.Add(new PUComboBoxItemModel()
            {
                Header = "它的Value为123",
                CanDelete = false,
                CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#22E089B8")),
                SelectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#55E089B8")),
                Value = 123,
            });
            comboList.Add(new PUComboBoxItemModel()
            {
                Header = "它的Value为456",
                CanDelete = false,
                CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#22E089B8")),
                SelectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#55E089B8")),
                Value = 456,
            });
            ComboBoxItemsList = new BindableCollection<PUComboBoxItemModel>(comboList);

        }

        #region Binding
        public string Password
        {
            get { return _password; }
            set { _password = value; NotifyOfPropertyChange(() => Password); }
        }
        private string _password = "12345";

        public BindableCollection<PUComboBoxItemModel> ComboBoxItemsList
        {
            get { return _comboBoxItemsList; }
            set { _comboBoxItemsList = value; NotifyOfPropertyChange(() => ComboBoxItemsList); }
        }
        private BindableCollection<PUComboBoxItemModel> _comboBoxItemsList;

        public object SelectedValue
        {
            get { return _selectedValue; }
            set { _selectedValue = value; NotifyOfPropertyChange(() => SelectedValue); }
        }
        private object _selectedValue ;

        #endregion

        #region Event
        public void Delete(ItemCollection sender, RoutedPropertyChangedEventArgs<PUComboBoxItem> e)
        {
            var item = e.NewValue as PUComboBoxItem;
            PUMessageBox.ShowDialog($"你点击了“{item.Content}”的删除按钮，但该项目不会立即删除。\n你可以在验证是否允许删除后再手动移除。在此弹窗关闭后，你点击的行项目将会被移除。");
            sender.Remove(item);
        }

        public void GetValue()
        {
            PUMessageBox.ShowDialog($"该选项的Value为“{SelectedValue.ToString()}”。\n你可以通过修改PUComboBoxItem的Value属性来控制它。");
        }
        #endregion
    }
}
