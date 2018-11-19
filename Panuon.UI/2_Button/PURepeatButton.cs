using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PURepeatButton : RepeatButton
    {
        static PURepeatButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PURepeatButton), new FrameworkPropertyMetadata(typeof(PURepeatButton)));
        }

        #region Property
        /// <summary>
        /// 获取或设置按钮的基本样式。默认值为标准样式（General）。
        /// </summary>
        public RepeatButtonStyles RepeatButtonStyle
        {
            get { return (RepeatButtonStyles)GetValue(RepeatButtonStyleProperty); }
            set { SetValue(RepeatButtonStyleProperty, value); }
        }
        public static readonly DependencyProperty RepeatButtonStyleProperty = DependencyProperty.Register("RepeatButtonStyle", typeof(RepeatButtonStyles), typeof(PURepeatButton), new PropertyMetadata(RepeatButtonStyles.General));

        /// <summary>
        /// 获取或设置鼠标点击时按钮的效果。默认为无效果（Classic）。
        /// </summary>
        public ClickStyles ClickStyle
        {
            get { return (ClickStyles)GetValue(ClickStyleProperty); }
            set { SetValue(ClickStyleProperty, value); }
        }

        public static readonly DependencyProperty ClickStyleProperty =
            DependencyProperty.Register("ClickStyle", typeof(ClickStyles), typeof(PURepeatButton), new PropertyMetadata(ClickStyles.Classic));

        /// <summary>
        /// 圆角大小，默认值为0。
        /// </summary>
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(PURepeatButton), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// 鼠标悬浮时遮罩层的背景颜色（在Outline和Link样式下为前景色），默认值为白色（在Outline和Link样式下为灰色）。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }
        public static readonly DependencyProperty CoverBrushProperty = DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PURepeatButton), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        #endregion

    }
}
