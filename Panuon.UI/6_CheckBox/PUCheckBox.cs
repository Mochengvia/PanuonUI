using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PUCheckBox : CheckBox
    {
        static PUCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUCheckBox), new FrameworkPropertyMetadata(typeof(PUCheckBox)));
        }

        #region Property
        /// <summary>
        /// 获取或设置选择框的基本样式。默认值为General。
        /// </summary>
        public CheckBoxStyles CheckBoxStyle
        {
            get { return (CheckBoxStyles)GetValue(CheckBoxStyleProperty); }
            set { SetValue(CheckBoxStyleProperty, value); }
        }
        public static readonly DependencyProperty CheckBoxStyleProperty = DependencyProperty.Register("CheckBoxStyle", typeof(CheckBoxStyles), typeof(PUCheckBox), new PropertyMetadata(CheckBoxStyles.General));

        /// <summary>
        /// 获取或设置选择框的圆角大小。默认值为0。
        /// </summary>
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(PUCheckBox), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// 获取或设置选择框的宽度。默认值为20（Switch样式下为30）。
        /// </summary>
        public double InnerWidth
        {
            get { return (double)GetValue(InnerWidthProperty); }
            set
            { SetValue(InnerWidthProperty, value); }
        }
        public static readonly DependencyProperty InnerWidthProperty = 
            DependencyProperty.Register("InnerWidth", typeof(double), typeof(PUCheckBox), new PropertyMetadata((double)20));

        /// <summary>
        /// 获取或设置选择框的高度。默认值为20。
        /// </summary>
        public double InnerHeight
        {
            get { return (double)GetValue(InnerHeightProperty); }
            set { SetValue(InnerHeightProperty, value); }
        }
        public static readonly DependencyProperty InnerHeightProperty = 
            DependencyProperty.Register("InnerHeight", typeof(double), typeof(PUCheckBox), new PropertyMetadata((double)20));

        /// <summary>
        /// Check时对号的背景颜色（或前景色），默认值为白色。
        /// <para>仅当按钮样式为General时生效。</para>
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }
        public static readonly DependencyProperty CoverBrushProperty = 
            DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUCheckBox));

        #endregion


    }

}
