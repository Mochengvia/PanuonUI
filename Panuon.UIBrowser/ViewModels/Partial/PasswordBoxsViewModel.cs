using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class PasswordBoxsViewModel : Screen, IShell
    {

        public string Password
        {
            get { return _password; }
            set { _password = value; NotifyOfPropertyChange(() => Password); }
        }
        private string _password = "12345";

        public string Password2
        {
            get { return _password2; }
            set { _password2 = value; NotifyOfPropertyChange(() => Password2); }
        }
        private string _password2 = "54321";

    }
}
