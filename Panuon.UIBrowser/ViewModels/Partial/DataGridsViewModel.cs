using Caliburn.Micro;
using Microsoft.Win32;
using Panuon.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class DataGridsViewModel : Screen, IShell
    {
        public DataGridsViewModel()
        {
            var list = new List<ItemModel>()
            {
                new ItemModel(1,"SUM0000001","Sams"),
                new ItemModel(2,"SUM0000002","Jack"),
                new ItemModel(3,"SUM0000003","Michael"),
                new ItemModel(4,"SUM0000004","Rechard"),
                new ItemModel(5,"SUM0000005","Woods"),
            };
            ItemsList = new ObservableCollection<ItemModel>(list);
        }

        #region Binding
        public ObservableCollection<ItemModel> ItemsList
        {
            get { return _itemsList; }
            set
            { _itemsList = value; NotifyOfPropertyChange(() => ItemsList); }
        }
        private ObservableCollection<ItemModel> _itemsList;
        #endregion

        #region Event
        public void Delete(long id)
        {
            var item = ItemsList.FirstOrDefault(x => x.ID == id);
            if (item == null)
                return;
            ItemsList.Remove(item);
        }
        #endregion
    }
    public class ItemModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyOfPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ItemModel() { }

        public ItemModel(long id, string number, string name)
        {
            ID = id;
            Number = number;
            Name = name;
        }

        public long ID
        {
            get { return _id; }
            set { _id = value; NotifyOfPropertyChange("ID"); }
        }
        private long _id;

        public string Number
        {
            get { return _number; }
            set { _number = value; NotifyOfPropertyChange("Number"); }
        }
        private string _number;

        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyOfPropertyChange("Name"); }
        }
        private string _name;
    }

}
