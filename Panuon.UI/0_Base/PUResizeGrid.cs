using System.Windows;
using System.Windows.Controls;


namespace Panuon.UI
{
    public class PUResizeGrid : UserControl
    {
        static PUResizeGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUResizeGrid), new FrameworkPropertyMetadata(typeof(PUResizeGrid)));
        }
        #region Property


        public bool IsSquare
        {
            get { return (bool)GetValue(IsSquareProperty); }
            set { SetValue(IsSquareProperty, value); }
        }

        public static readonly DependencyProperty IsSquareProperty =
            DependencyProperty.Register("IsSquare", typeof(bool), typeof(PUResizeGrid), new PropertyMetadata(false));


        #endregion
    }
}
