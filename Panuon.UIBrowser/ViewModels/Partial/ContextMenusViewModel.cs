using Caliburn.Micro;
using Panuon.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class ContextMenusViewModel : Screen, IShell
    {
        #region Binding
        public string Text
        {
            get { return _text; }
            set { _text = value; NotifyOfPropertyChange(() => Text); }
        }
        private string _text = "右击打开菜单";
        #endregion


        #region Event
        public void Cut()
        {
            Clipboard.SetText(Text);
            Text = "";
        }

        public void Copy()
        {
            Clipboard.SetText(Text);

        }

        public void Paste()
        {
            Text = Clipboard.GetText();
        }
        #endregion
    }
}
