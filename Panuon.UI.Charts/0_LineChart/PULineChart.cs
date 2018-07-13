using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Panuon.UI.Charts
{
    public class PULineChart : UserControl
    {
        static PULineChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PULineChart), new FrameworkPropertyMetadata(typeof(PULineChart)));
        }
        #region Property
        /// <summary>
        /// X轴（横）内容集合。
        /// </summary>
        public string[] XAxis
        {
            get { return (string[])GetValue(XAxisProperty); }
            set { SetValue(XAxisProperty, value); }
        }
        public static readonly DependencyProperty XAxisProperty = DependencyProperty.Register("XAxis", typeof(string[]), typeof(PULineChart), new PropertyMetadata(true));

        /// <summary>
        /// Y轴（纵）内容集合。
        /// </summary>
        public string[] YAxis
        {
            get { return (string[])GetValue(YAxisProperty); }
            set { SetValue(YAxisProperty, value); }
        }
        public static readonly DependencyProperty YAxisProperty = DependencyProperty.Register("YAxis", typeof(string[]), typeof(PULineChart), new PropertyMetadata(true));

        /// <summary>
        /// 点集合，从左侧开始排列。若点的数量不等于X轴座的数量，可能会造成显示不全。
        /// </summary>
        public Point[] Points
        {
            get { return (Point[])GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }
        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register("Points", typeof(Point[]), typeof(PULineChart), new PropertyMetadata(true));

        /// <summary>
        /// 将线下方的颜色上色，默认值为透明。
        /// </summary>
        public SolidColorBrush CoverBrush
        {
            get { return (SolidColorBrush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }
        public static readonly DependencyProperty CoverBrushProperty = DependencyProperty.Register("CoverBrush", typeof(SolidColorBrush), typeof(PULineChart), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// 坐标改变时是否使用动画，默认值为True（使用）。
        /// </summary>
        public bool UsingAnimate
        {
            get { return (bool)GetValue(UsingAnimateProperty); }
            set { SetValue(UsingAnimateProperty, value); }
        }
        public static readonly DependencyProperty UsingAnimateProperty = DependencyProperty.Register("UsingAnimate", typeof(bool), typeof(PULineChart), new PropertyMetadata(true));
        #endregion 
    }


}
