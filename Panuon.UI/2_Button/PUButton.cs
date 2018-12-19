using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PUButton : Button
    {
        static PUButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUButton), new FrameworkPropertyMetadata(typeof(PUButton)));
        }

        #region Property

        /// <summary>
        /// 效果等同于Tag，无实际作用。
        /// </summary>
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(PUButton));

        /// <summary>
        /// 获取或设置按钮的基本样式。默认值为标准样式（General）。
        /// </summary>
        public ButtonStyles ButtonStyle
        {
            get { return (ButtonStyles)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }
        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(ButtonStyles), typeof(PUButton), new PropertyMetadata(ButtonStyles.General));

        /// <summary>
        /// 获取或设置鼠标点击时按钮的效果。默认为无特殊效果（Classic）。
        /// </summary>
        public ClickStyles ClickStyle
        {
            get { return (ClickStyles)GetValue(ClickStyleProperty); }
            set { SetValue(ClickStyleProperty, value); }
        }

        public static readonly DependencyProperty ClickStyleProperty =
            DependencyProperty.Register("ClickStyle", typeof(ClickStyles), typeof(PUButton), new PropertyMetadata(ClickStyles.Classic));

        /// <summary>
        /// 获取或设置按钮的圆角大小。默认值为0。
        /// </summary>
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty = 
            DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(PUButton));

        /// <summary>
        /// 获取或设置鼠标悬浮时遮罩层的背景颜色（在Outline和Link样式下为前景色），默认值为#26FFFFFF（在Hollow、Outline和Link样式下为灰黑色）。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }
        public static readonly DependencyProperty CoverBrushProperty = 
            DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUButton));

        #endregion
    }

}
