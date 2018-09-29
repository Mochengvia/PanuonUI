using Caliburn.Micro;
using Panuon.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class TreeViewsViewModel : Screen, IShell
    {
        public TreeViewsViewModel()
        {

        }

        #region Binding
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
        #endregion

        #region Event
        public void ChoosedItemChanged(RoutedPropertyChangedEventArgs<PUTreeViewItem> e)
        {
            var choosedItem = e.NewValue;
            if(choosedItem != null)
                ChoosedHeader = choosedItem.Header;
        }
        #endregion
    }
}
