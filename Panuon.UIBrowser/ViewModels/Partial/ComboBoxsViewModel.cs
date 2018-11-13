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
                CanDelete = true,
                Value = 123,
            });
            comboList.Add(new PUComboBoxItemModel()
            {
                Header = "它的Value为456",
                CanDelete = true,
                Value = 456,
            });
            ComboBoxItemsList = new BindableCollection<PUComboBoxItemModel>(comboList);
            SelectedValue = 456;
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
        private object _selectedValue;

        #endregion

        #region Event
        public void Delete(ItemCollection sender, RoutedPropertyChangedEventArgs<PUComboBoxItem> e)
        {
            var item = e.NewValue as PUComboBoxItem;
            if(PUMessageBox.ShowConfirm("确定要删除该选项吗？") == true)
            {
                sender.Remove(item);
            }
        }

        public void GetValue()
        {
            PUMessageBox.ShowDialog($"该选项的Value为“{SelectedValue?.ToString()}”。\n你可以通过修改PUComboBoxItem的Value属性来控制它。");
        }
        #endregion
    }
}
