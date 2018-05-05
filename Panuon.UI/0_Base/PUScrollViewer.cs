using System.Windows;
using System.Windows.Controls;

namespace Panuon.UI
{
    public class PUScrollViewer : ScrollViewer
    {
        static PUScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUScrollViewer), new FrameworkPropertyMetadata(typeof(PUScrollViewer)));
        }
    }
}
