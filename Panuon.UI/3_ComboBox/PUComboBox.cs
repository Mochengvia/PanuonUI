using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PUComboBox : ComboBox
    {

        static PUComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUComboBox), new FrameworkPropertyMetadata(typeof(PUComboBox)));
        }

        #region Property
        /// <summary>
        /// 圆角大小，默认值为0。
        /// </summary>
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(PUComboBox), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        ///  下拉框激活时阴影的颜色，默认值为#888888。
        /// </summary>
        public Color ShadowColor
        {
            get { return (Color)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }
        public static readonly DependencyProperty CoverBrushProperty = DependencyProperty.Register("ShadowColor", typeof(Color), typeof(PUComboBox), new PropertyMetadata((Color)ColorConverter.ConvertFromString("#888888")));

        #endregion


    }
}
