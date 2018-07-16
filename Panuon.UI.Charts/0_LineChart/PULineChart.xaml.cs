using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Panuon.UI.Charts
{
    /// <summary>
    /// PULineChart.xaml 的交互逻辑
    /// </summary>
    public partial class PULineChart : UserControl
    {
        #region Identity
        /// <summary>
        /// X轴坐标栏高度
        /// </summary>
        private double _xAxisHeight = 30;
        /// <summary>
        /// Y轴坐标栏宽度
        /// </summary>
        private double _yAxisWidth = 80;
        #endregion

        public PULineChart()
        {
            InitializeComponent();
        }

        #region Property
        /// <summary>
        /// X轴（横）内容集合。修改此属性会触发控件重绘。
        /// </summary>
        public string[] XAxis
        {
            get { return (string[])GetValue(XAxisProperty); }
            set { SetValue(XAxisProperty, value); }
        }
        public static readonly DependencyProperty XAxisProperty = DependencyProperty.Register("XAxis", typeof(string[]), typeof(PULineChart), new PropertyMetadata(null, XAxisOnChanged));
        private static void XAxisOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var lineChart = d as PULineChart;
            lineChart.DrawChart();
        }

        /// <summary>
        /// Y轴（纵）内容集合。修改此属性会触发控件重绘。
        /// </summary>
        public string[] YAxis
        {
            get { return (string[])GetValue(YAxisProperty); }
            set { SetValue(YAxisProperty, value); }
        }
        public static readonly DependencyProperty YAxisProperty = DependencyProperty.Register("YAxis", typeof(string[]), typeof(PULineChart), new PropertyMetadata(null, AxisOnChanged));
        private static void AxisOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var lineChart = d as PULineChart;
            lineChart.DrawChart();
        }

        /// <summary>
        /// 值为0~1的集合，表示在Y坐标中的高度比例，从X轴的左侧开始向右排列。修改此属性会触发控件重绘。
        /// <para>例如，当Y轴的值域为0~10时，y=3的Value值为0.3，y=6的Value值为0.6。
        /// <para>当Y轴的值为非数字时，这个设定将可以解决你遇到的一些问题。再例如，当Y轴的值域为"等级零"到"等级五"时，y="等级三"的Value为0.6。</para>
        /// </summary>
        public double[] Values
        {
            get { return (double[])GetValue(ValuesProperty); }
            set
            {
                SetValue(ValuesProperty, value);
            }
        }
        public static readonly DependencyProperty ValuesProperty = DependencyProperty.Register("Values", typeof(double[]), typeof(PULineChart), new PropertyMetadata(null, AxisOnChanged));
        private static void ValuesOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var lineChart = d as PULineChart;
            lineChart.DrawChart();
        }

        /// <summary>
        /// 鼠标悬浮点上时显示的值。修改此属性会触发控件重绘。
        /// </summary>
        public string[] ValueTips
        {
            get { return (string[])GetValue(ValueTipsProperty); }
            set { SetValue(ValueTipsProperty, value); }
        }
        public static readonly DependencyProperty ValueTipsProperty = DependencyProperty.Register("ValueTips", typeof(string[]), typeof(PULineChart), new PropertyMetadata(null, AxisOnChanged));

        /// <summary>
        /// 坐标改变时是否使用动画，默认值为True（使用）。
        /// </summary>
        public bool UsingAnimation
        {
            get { return (bool)GetValue(UsingAnimationProperty); }
            set { SetValue(UsingAnimationProperty, value); }
        }
        public static readonly DependencyProperty UsingAnimationProperty = DependencyProperty.Register("UsingAnimation", typeof(bool), typeof(PULineChart), new PropertyMetadata(true));

        /// <summary>
        /// 网格线与坐标字体颜色，默认值为Gray。修改此属性时，只会部分重绘控件。
        /// </summary>
        public Brush AxisBrush
        {
            get { return (Brush)GetValue(AxisBrushProperty); }
            set { SetValue(AxisBrushProperty, value); }
        }
        public static readonly DependencyProperty AxisBrushProperty = DependencyProperty.Register("AxisBrush", typeof(Brush), typeof(PULineChart), new PropertyMetadata(new SolidColorBrush(Colors.Gray), OnBrushChanged));
        private static void OnBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var lineChart = d as PULineChart;
            lineChart.UpdateColors();
        }

        /// <summary>
        /// 绘制线与点的颜色。默认值为DimGray。修改此属性时，只会部分重绘控件。
        /// </summary>
        public Brush LineBrush
        {
            get { return (Brush)GetValue(LineBrushProperty); }
            set { SetValue(LineBrushProperty, value); }
        }
        public static readonly DependencyProperty LineBrushProperty = DependencyProperty.Register("LineBrush", typeof(Brush), typeof(PULineChart), new PropertyMetadata(new SolidColorBrush(Colors.DimGray), OnBrushChanged));


        /// <summary>
        /// 绘制线下方的区域颜色，默认值为灰色渐变。修改此属性时，只会部分重绘控件。
        /// <para>可以将其设置成LinearGradientBrush以获得渐变效果！ヾ(ﾟ∀ﾟゞ)</para>
        /// </summary>
        public Brush AreaBrush
        {
            get { return (Brush)GetValue(AreaBrushProperty); }
            set { SetValue(AreaBrushProperty, value); }
        }
        public static readonly DependencyProperty AreaBrushProperty = DependencyProperty.Register("AreaBrush", typeof(Brush), typeof(PULineChart), new PropertyMetadata(new LinearGradientBrush(new GradientStopCollection() { new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#AAAAAAAA"), Offset = 0 }, new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#22AAAAAA"), Offset = 1 } }, 90), OnBrushChanged));

        /// <summary>
        /// 折线粗细。修改此属性时，只会部分重绘控件。
        /// </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(PULineChart), new PropertyMetadata((double)4, OnBrushChanged));

        #endregion

        #region APIs
        /// <summary>
        /// 使用配置的颜色属性重新绘制折线图。
        /// </summary>
        public void DrawChart()
        {
            ClearChart();
            if (XAxis == null || YAxis == null || XAxis.Count() == 0 || YAxis.Count() == 0)
                return;

            //绘制坐标
            var xCellWidth = (Width - _yAxisWidth) / XAxis.Count();
            var yCellHeight = (Height - _xAxisHeight) / YAxis.Count();
            DrawAxis(xCellWidth, yCellHeight);

            //绘制网格线
            var canvasWidth = Width - _yAxisWidth;
            var canvasHeight = Height - _xAxisHeight;
            DrawGrid(xCellWidth, yCellHeight, canvasWidth, canvasHeight);

            //绘制点和线
            if (Values == null)
                return;
            DrawPointAndLineAndArea(xCellWidth, yCellHeight);
        }

        /// <summary>
        /// 清空所有内容。
        /// </summary>
        public void ClearChart()
        {
            pointCanvas.Children.Clear();
            gridPath.Data = null;
            polyline.Points.Clear();
            polygon.Points.Clear();
            XAxisCells.Children.Clear();
            YAxisCells.Children.Clear();

            UpdateColors();
        }
        #endregion

        #region Function
        internal void UpdateColors()
        {
            gridPath.Stroke = AxisBrush;
            polyline.Stroke = LineBrush;
            polyline.StrokeThickness = StrokeThickness;
            polygon.Fill = AreaBrush;
            foreach(var item in pointCanvas.Children)
            {
                var ell = item as Ellipse;
                if (ell == null)
                    continue;
                ell.Fill = LineBrush;
            }
            foreach (var item in XAxisCells.Children)
            {
                var lbl = item as Label;
                if (lbl == null)
                    continue;
                lbl.Foreground = AxisBrush;
            }
            foreach (var item in YAxisCells.Children)
            {
                var lbl = item as Label;
                if (lbl == null)
                    continue;
                lbl.Foreground = AxisBrush;
            }
        }
        /// <summary>
        /// 绘制网格。
        /// </summary>
        internal void DrawGrid(double xCellWidth, double yCellHeight, double canvasWidth, double canvasHeight)
        {
            var path = "";
            for (int i = 0; i < XAxis.Count(); i++)
            {
                path += $"M {xCellWidth * i},0 V {canvasHeight}";
            }
            for (int i = 0; i < YAxis.Count(); i++)
            {
                path += $"M 0,{canvasHeight - yCellHeight * i} H {canvasWidth}";
            }
            gridPath.Data = Geometry.Parse(path);
        }

        /// <summary>
        /// 绘制X、Y轴坐标值
        /// </summary>
        internal void DrawAxis(double xCellWidth, double yCellHeight)
        {
            //加载X轴坐标
            foreach (var xa in XAxis)
            {
                var label = new Label()
                {
                    Width = xCellWidth,
                    Height = _xAxisHeight,
                    VerticalContentAlignment = VerticalAlignment.Top,
                    Content = xa,
                    Foreground = AxisBrush,
                    FontSize = FontSize,
                };
                XAxisCells.Children.Add(label);
            }
            //加载Y轴坐标
            foreach (var ya in YAxis.Reverse())
            {
                var label = new Label()
                {
                    Width = _yAxisWidth,
                    Height = yCellHeight,
                    VerticalContentAlignment = VerticalAlignment.Bottom,
                    Content = ya,
                    Foreground = AxisBrush,
                    FontSize = FontSize,
                };
                YAxisCells.Children.Add(label);
            }

        }

        /// <summary>
        /// 绘制点、线和区域
        /// </summary>
        internal void DrawPointAndLineAndArea(double xCellWidth, double yCellHeight)
        {
            ScaleTransform scale;
            if (UsingAnimation)
                scale = new ScaleTransform() { ScaleY = 0 };
            else
                scale = new ScaleTransform() { ScaleY = 1 };

            polygon.RenderTransform = scale;
            polyline.RenderTransform = scale;
            polygon.Points.Add(new Point(0, Height));
            var xAxis = 0.0;
            for (int i = 0; i< Values.Count(); i ++)
            {
                var value = Values[i];

                var point = new Point(xAxis, (Height - _xAxisHeight - yCellHeight) * (1 - value) + yCellHeight);
                
                polyline.Points.Add(point);
                polygon.Points.Add(point);
                var ellipse = new Ellipse()
                {
                    Width = 10,
                    Height = 10,
                    Margin = new Thickness(-5, -5, 0, 0),
                    Fill = LineBrush,
                    ToolTip = ValueTips == null ? null : ValueTips.Count() > i ? ValueTips[i] : null,
                };
                if (UsingAnimation)
                {
                    Canvas.SetTop(ellipse, Height - _xAxisHeight);
                    ellipse.BeginAnimation(Canvas.TopProperty, GetDoubleAnimation((Height - _xAxisHeight - yCellHeight) * (1 - value) + yCellHeight, 1));
                }
                else
                {
                    Canvas.SetTop(ellipse, (Height - _xAxisHeight - yCellHeight) * (1 - value) + yCellHeight);
                }
                  
                Canvas.SetLeft(ellipse, xAxis);
                pointCanvas.Children.Add(ellipse);
                xAxis += xCellWidth;
                if (xAxis / xCellWidth >= XAxis.Count())
                    break;
            }
            polygon.Points.Add(new Point((xAxis - xCellWidth), Height));
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, GetDoubleAnimation(1,1));
        }

        private DoubleAnimation GetDoubleAnimation(double to, double duration = 0.2)
        {
            return new DoubleAnimation()
            {
                To = to,
                Duration = TimeSpan.FromSeconds(duration),
                EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseInOut },
            };
        }
        #endregion
    }
}
