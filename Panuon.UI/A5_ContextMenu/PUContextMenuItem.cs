using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PUContextMenuItem : MenuItem
    {
        static PUContextMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUContextMenuItem), new FrameworkPropertyMetadata(typeof(PUContextMenuItem)));
        }

        #region Property



        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(PUContextMenuItem));

        #endregion
    }
}
