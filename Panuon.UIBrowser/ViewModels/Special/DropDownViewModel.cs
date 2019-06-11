using Caliburn.Micro;
using Microsoft.Win32;
using Panuon.UI;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Panuon.UIBrowser.ViewModels.Special
{
    public class DropDownViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public DropDownViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }
        #endregion

        #region Binding
        #endregion

        #region Event
        #endregion

    }
}
