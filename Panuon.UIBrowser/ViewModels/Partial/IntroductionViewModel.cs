using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class IntroductionViewModel : Screen,IShell
    {
        public string Website
        {
            get { return _website; }
            set { _website = value; NotifyOfPropertyChange(() => Website); }
        }
        private string _website = "https://blog.csdn.net/qq_36663276/article/details/80209684";

        public void OpenWebsite()
        {
            System.Diagnostics.Process.Start("explorer.exe", Website);
        }
    }
}
