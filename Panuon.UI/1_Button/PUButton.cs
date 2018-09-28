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
        /// 按钮样式，默认值为General
        /// </summary>
        public ButtonStyles ButtonStyle
        {
            get { return (ButtonStyles)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }
        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(ButtonStyles), typeof(PUButton), new PropertyMetadata(ButtonStyles.General));

        /// <summary>
        /// 圆角大小，默认值为0。
        /// </summary>
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(PUButton), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// 鼠标悬浮时遮罩层的背景颜色（在Outline和Link样式下为前景色），默认值为白色（在Outline和Link样式下为灰色）。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }
        public static readonly DependencyProperty CoverBrushProperty = DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUButton), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        #endregion

        public enum ButtonStyles
        {
            /// <summary>
            /// 一个常规按钮。
            /// </summary>
            General = 1,
            /// <summary>
            /// 一个带边框的空心按钮，当鼠标悬浮时才会显示背景色。
            /// <para>当鼠标移入时，该按钮的背景色将由Background变为指定的CoverBrush。</para>
            /// </summary>
            Hollow = 2,
            /// <summary>
            /// 一个带边框的空心按钮，当鼠标悬浮时才会显示前景色。
            /// <para>当鼠标移入时，该按钮的边框和前景色将由BorderBrush和Foreground变为指定的CoverBrush。</para>
            /// </summary>
            Outline = 3,
            /// <summary>
            /// 一个不带任何边框和背景色的文字按钮。
            /// <para>当鼠标移入时，该按钮的前景色将由Foreground变为指定的CoverBrush。</para>
            /// </summary>
            Link = 4,
        }
    }

}
