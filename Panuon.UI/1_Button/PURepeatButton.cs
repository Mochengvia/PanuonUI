/*==============================================================
*作者：ZEOUN
*时间：2018/10/17 15:28:09
*说明： 
*日志：2018/10/17 15:28:09 创建。
*==============================================================*/
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

        public enum RepeatButtonStyles
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

        public enum ClickStyles
        {
            /// <summary>
            /// 点击按钮时不触发下沉操作。
            /// </summary>
            Classic,
            /// <summary>
            /// 点击按钮时下沉。
            /// </summary>
            Sink,
        }
    }
}
