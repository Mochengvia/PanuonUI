using Caliburn.Micro;
using Panuon.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using static Panuon.UI.PUTreeView;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class TreeViewsViewModel : Screen, IShell
    {
        public TreeViewsViewModel()
        {
            TreeViewItems = new ObservableCollection<PUTreeViewItemModel>();
            LoadTreeView();
        }

        #region Binding
        public ObservableCollection<PUTreeViewItemModel> TreeViewItems
        {
            get { return _treeViewItems; }
            set { _treeViewItems = value; NotifyOfPropertyChange(() => TreeViewItems); }
        }
        private ObservableCollection<PUTreeViewItemModel> _treeViewItems;

        public object ChoosedHeader
        {
            get { return _choosedHeader; }
            set { _choosedHeader = value; NotifyOfPropertyChange(() => ChoosedHeader); }
        }
        private object _choosedHeader;

        public object ChoosedValue
        {
            get { return _choosedValue; }
            set { _choosedValue = value; NotifyOfPropertyChange(() => ChoosedValue); }
        }
        private object _choosedValue;
        public TreeViewStyles TreeViewStyle
        {
            get { return _treeViewStyle; }
            set { _treeViewStyle = value; NotifyOfPropertyChange(() => TreeViewStyle); }
        }
        private TreeViewStyles _treeViewStyle= TreeViewStyles.General;
        
        #endregion

        #region Event
        public void ChoosedItemChanged(RoutedPropertyChangedEventArgs<PUTreeViewItem> e)
        {
            var choosedItem = e.NewValue;
            if(choosedItem != null)
                ChoosedHeader = choosedItem.Header;
        }
        #endregion

        #region Function
        public void LoadTreeView()
        {
            TreeViewItems.Add(new PUTreeViewItemModel()
            {
                Header = "第一章",
                Value = "1",
                Items = new List<PUTreeViewItemModel>()
                 {
                      new PUTreeViewItemModel()
                      {
                          Header = "第一节",
                            Value = "1.1",
                      },
                      new PUTreeViewItemModel()
                      {
                          Header = "第二节",
                            Value = "1.2",
                      },
                 },
            });
            TreeViewItems.Add(new PUTreeViewItemModel()
            {
                Header = "第二章",
                Value = "2",
                Items = new List<PUTreeViewItemModel>()
                 {
                      new PUTreeViewItemModel()
                      {
                          Header = "第一节",
                            Value ="2.1",
                      },
                      new PUTreeViewItemModel()
                      {
                          Header = "第二节",
                            Value = "2.2",
                      },
                 },
            });
            TreeViewItems.Add(new PUTreeViewItemModel()
            {
                Header = "第三章",
                Value = "3",
                Items = new List<PUTreeViewItemModel>()
                 {
                      new PUTreeViewItemModel()
                      {
                          Header = "第一节",
                            Value = "3.1",
                      },
                      new PUTreeViewItemModel()
                      {
                          Header = "第二节",
                            Value = "3.2",
                      },
                 },
            });
            NotifyOfPropertyChange(() => TreeViewItems);
        }

        public void SelectionChanged(SelectionChangedEventArgs e)
        {
            var comboBoxItem = e.AddedItems[0] as PUComboBoxItem;
            switch (comboBoxItem.Value.ToString())
            {
                case "1":
                    TreeViewStyle = TreeViewStyles.General;
                    break;
                case "2":
                    TreeViewStyle = TreeViewStyles.Classic;
                    break;
            }
        }
        #endregion
    }
}
