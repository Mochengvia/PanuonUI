using System.Windows;
using System.Windows.Controls;

namespace Panuon.UI
{
    internal class PUResizeContainer : ContentControl
    {
        static PUResizeContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUResizeContainer), new FrameworkPropertyMetadata(typeof(PUResizeContainer)));
        }

        #region Property
        public bool IsSquare
        {
            get { return (bool)GetValue(IsSquareProperty); }
            set { SetValue(IsSquareProperty, value); }
        }

        public static readonly DependencyProperty IsSquareProperty =
            DependencyProperty.Register("IsSquare", typeof(bool), typeof(PUResizeContainer));

        public bool LimitInParent
        {
            get { return (bool)GetValue(LimitInParentProperty); }
            set { SetValue(LimitInParentProperty, value); }
        }

        public static readonly DependencyProperty LimitInParentProperty =
            DependencyProperty.Register("LimitInParent", typeof(bool), typeof(PUResizeContainer));

        public bool CanResize
        {
            get { return (bool)GetValue(CanResizeProperty); }
            set { SetValue(CanResizeProperty, value); }
        }

        public static readonly DependencyProperty CanResizeProperty =
            DependencyProperty.Register("CanResize", typeof(bool), typeof(PUResizeContainer), new PropertyMetadata(true));

        #endregion
    }
}
