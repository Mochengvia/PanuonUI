using Caliburn.Micro;
using Panuon.UI;
using System.Collections.Generic;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class TabControlsViewModel : Screen, IShell
    {
        public TabControlsViewModel()
        {
            var list = new List<PUTabItemModel>();
            list.Add(new PUTabItemModel()
            {
                Header = "Test1",
                Icon = "",
                CanDelete = false,
                Content = "1",
                Value = 1.1,
            });
            list.Add(new PUTabItemModel()
            {
                Header = "Test2",
                Icon= "",
                CanDelete = true,
                Content = "2",
                Value = 2.2,
            });

            list.Add(new PUTabItemModel()
            {
                Header = "Test3",
                Icon= "",
                CanDelete = false,
                Content = "3",
                Value = 3.3,
            });
            TabItemList = new BindableCollection<PUTabItemModel>(list);
        }

        public BindableCollection<PUTabItemModel> TabItemList
        {
            get { return _tabItemList; }
            set { _tabItemList = value; NotifyOfPropertyChange(() => TabItemList); }
        }
        private BindableCollection<PUTabItemModel> _tabItemList;


    }
}
