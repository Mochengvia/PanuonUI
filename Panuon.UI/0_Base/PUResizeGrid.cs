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
    }
}
