using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class SpecialViewModel : Screen, IShell
    {
        public bool IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; NotifyOfPropertyChange(() => IsRunning); }
        }
        private bool _isRunning = true;


        public void Switch()
        {
            IsRunning = !IsRunning;
        }
    }
}