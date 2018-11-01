using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class PagingNavsViewModel : Screen, IShell
    {
        public int TotalPage
        {
            get { return _totalPage; }
            set { _totalPage = value; NotifyOfPropertyChange(() => TotalPage); }
        }
        private int _totalPage = 10;


        public int CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; NotifyOfPropertyChange(() => CurrentPage); }
        }
        private int _currentPage = 1;
    }
}
