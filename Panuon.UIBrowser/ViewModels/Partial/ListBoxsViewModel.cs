using Caliburn.Micro;
using Panuon.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class ListBoxsViewModel : Screen, IShell
    {

        private PUListBox _listBox;

        public void Search(string contentWord)
        {
            _listBox.SearchItemByContent(contentWord, true);
        }

        // 作为Value实现精准查询
        //public void Search(object contentWord)
        //{
        //    _listBox.SearchItemByValue(contentWord);
        //}

        public void ListBoxLoaded(object sender)
        {
            _listBox = sender as PUListBox;
        }
    }
}
