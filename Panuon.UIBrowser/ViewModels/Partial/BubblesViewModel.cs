using Caliburn.Micro;
using Panuon.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class BubblesViewModel : Screen, IShell
    {

        #region Binding
        public AnglePositions AnglePosition
        {
            get { return _anglePosition; }
            set { _anglePosition = value; NotifyOfPropertyChange(() => AnglePosition); }
        }
        private AnglePositions _anglePosition = AnglePositions.Left;
        #endregion

        #region Event
        public void SelectionChanged(SelectionChangedEventArgs e)
        {
            var comboBoxItem = e.AddedItems[0] as PUComboBoxItem;
            var value = Int32.Parse(comboBoxItem.Value.ToString());
            switch (value)
            {
                case 1:
                    AnglePosition = AnglePositions.Left;
                    break;
                case 2:
                    AnglePosition = AnglePositions.Right;
                    break;
                case 3:
                    AnglePosition = AnglePositions.BottomLeft;
                    break;
                case 4:
                    AnglePosition = AnglePositions.BottomCenter;
                    break;
                case 5:
                    AnglePosition = AnglePositions.BottomRight;
                    break;
            }
        }
        #endregion
    }
}
