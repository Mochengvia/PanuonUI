using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Panuon.UIBrowser.ViewModels
{
    [Export(typeof(IShell))]

    public class MainWindowViewModel : Screen, IShell
    {
        public MainWindowViewModel()
        {

        }

        public string AlertText
        {
            get { return _alertText; }
            set { _alertText = value; NotifyOfPropertyChange(() => AlertText); }
        }
        private string _alertText = "admin";

    }
}
