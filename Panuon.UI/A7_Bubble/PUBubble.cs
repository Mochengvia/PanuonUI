using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PUBubble : UserControl
    {
        static PUBubble()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUBubble), new FrameworkPropertyMetadata(typeof(PUBubble)));
        }

        #region Property

        /// <summary>
        /// 获取或设置当鼠标悬浮在气泡上时，气泡的背景颜色。默认值为#555555。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }

        public static readonly DependencyProperty CoverBrushProperty =
            DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUBubble), new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));



        /// <summary>
        /// 获取或设置尖角的位置，默认值为Left（左侧）。
        /// </summary>
        public AnglePositions AnglePosition
        {
            get { return (AnglePositions)GetValue(AnglePositionProperty); }
            set { SetValue(AnglePositionProperty, value); }
        }

        public static readonly DependencyProperty AnglePositionProperty =
            DependencyProperty.Register("AnglePosition", typeof(AnglePositions), typeof(PUBubble), new PropertyMetadata(AnglePositions.Left));



        /// <summary>
        /// 获取或设置圆角大小。默认值为0。
        /// </summary>
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty BorderCornerRadiusProperty =
            DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(PUBubble));



        #endregion

    }

}
