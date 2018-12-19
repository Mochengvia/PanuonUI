using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Panuon.UI.Charts
{
    /// <summary>
    /// PULineChart.xaml 的交互逻辑
    /// </summary>
    public partial class PULineChart : UserControl
    {
        #region Identity
        private double _yWidth = 100;

        private double _xHeight = 30;
        #endregion

        public PULineChart()
        {
            InitializeComponent();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Loaded += delegate
            {
                Draw();
            };
        }

        #region Property

        /// <summary>
        /// X轴显示间距。
        /// </summary>
        public int XAxisGap
        {
            get { return (int)GetValue(XAxisGapProperty); }
            set { SetValue(XAxisGapProperty, value); }
        }

        public static readonly DependencyProperty XAxisGapProperty =
            DependencyProperty.Register("XAxisGap", typeof(int), typeof(PULineChart), new PropertyMetadata(0, OnXAxisGapChanged));

        private static void OnXAxisGapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as PULineChart;
            if (chart.IsLoaded)
            {
                chart.LoadXAxis(chart.ActualWidth);
            }
        }

        /// <summary>
        /// 获取或设置使用动画的方式。默认值为始终使用（Always）。
        /// </summary>
        public AnimationModes AnimationMode
        {
            get { return (AnimationModes)GetValue(AnimationModeProperty); }
            set { SetValue(AnimationModeProperty, value); }
        }

        public static readonly DependencyProperty AnimationModeProperty =
            DependencyProperty.Register("AnimationMode", typeof(AnimationModes), typeof(PULineChart), new PropertyMetadata(AnimationModes.Always));


        /// <summary>
        /// 获取或设置网格画刷。
        /// </summary>
        public Brush GridBrush
        {
            get { return (Brush)GetValue(GridBrushProperty); }
            set { SetValue(GridBrushProperty, value); }
        }

        public static readonly DependencyProperty GridBrushProperty =
            DependencyProperty.Register("GridBrush", typeof(Brush), typeof(PULineChart), new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));


        /// <summary>
        /// 获取或设置线条画刷。
        /// </summary>
        public Brush LineBrush
        {
            get { return (Brush)GetValue(LineBrushProperty); }
            set { SetValue(LineBrushProperty, value); }
        }

        public static readonly DependencyProperty LineBrushProperty =
            DependencyProperty.Register("LineBrush", typeof(Brush), typeof(PULineChart), new PropertyMetadata(new SolidColorBrush(Colors.DimGray)));

        /// <summary>
        /// 获取或设置线条粗细。
        /// </summary>
        public double LineThickness
        {
            get { return (double)GetValue(LineThicknessProperty); }
            set { SetValue(LineThicknessProperty, value); }
        }

        public static readonly DependencyProperty LineThicknessProperty =
            DependencyProperty.Register("LineThickness", typeof(double), typeof(PULineChart), new PropertyMetadata(1.0));

        /// <summary>
        /// 获取或设置线条下方的区域画刷。
        /// </summary>
        public Brush AreaBrush
        {
            get { return (Brush)GetValue(AreaBrushProperty); }
            set { SetValue(AreaBrushProperty, value); }
        }

        public static readonly DependencyProperty AreaBrushProperty =
            DependencyProperty.Register("AreaBrush", typeof(Brush), typeof(PULineChart));

        /// <summary>
        /// 获取或设置点大小。
        /// </summary>
        public double PointSize
        {
            get { return (double)GetValue(PointSizeProperty); }
            set { SetValue(PointSizeProperty, value); }
        }

        public static readonly DependencyProperty PointSizeProperty =
            DependencyProperty.Register("PointSize", typeof(double), typeof(PULineChart), new PropertyMetadata(8.0));

        /// <summary>
        /// 获取或设置X轴的值数组。
        /// </summary>
        public ObservableCollection<string> XAxis
        {
            get { return (ObservableCollection<string>)GetValue(XAxisProperty); }
            set { SetValue(XAxisProperty, value); }
        }

        public static readonly DependencyProperty XAxisProperty =
            DependencyProperty.Register("XAxis", typeof(ObservableCollection<string>), typeof(PULineChart), new PropertyMetadata(OnXAxisChanged));

        private static void OnXAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as PULineChart;

            if (chart.XAxis != null)
            {
                chart.XAxis.CollectionChanged -= chart.XAxisChanged;
                chart.XAxis.CollectionChanged += chart.XAxisChanged;
                chart.XAxisChanged(null, null);
            }
        }

        private void XAxisChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LoadXAxis(ActualWidth);
            DrawGrid(ActualWidth, ActualHeight);
        }

        /// <summary>
        /// 获取或设置Y轴的值数组。
        /// </summary>
        public ObservableCollection<string> YAxis
        {
            get { return (ObservableCollection<string>)GetValue(YAxisProperty); }
            set { SetValue(YAxisProperty, value); }
        }

        public static readonly DependencyProperty YAxisProperty =
            DependencyProperty.Register("YAxis", typeof(ObservableCollection<string>), typeof(PULineChart), new PropertyMetadata(OnYAxisChanged));


        private static void OnYAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as PULineChart;
            if (chart.YAxis != null)
            {
                chart.YAxis.CollectionChanged -= chart.YAxisChanged;
                chart.YAxis.CollectionChanged += chart.YAxisChanged;
                chart.YAxisChanged(null, null);
            }
        }

        private void YAxisChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LoadYAxis(ActualHeight);
            DrawGrid(ActualWidth, ActualHeight);
        }

        /// <summary>
        /// 获取或设置点集合。
        /// </summary>
        public ObservableCollection<PUChartPoint> Points
        {
            get { return (ObservableCollection<PUChartPoint>)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(ObservableCollection<PUChartPoint>), typeof(PULineChart), new PropertyMetadata(OnPointsChanged));

        private static void OnPointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as PULineChart;

            if (chart.Points != null)
            {
                chart.Points.CollectionChanged -= chart.PointsChanged;
                chart.Points.CollectionChanged += chart.PointsChanged;
                chart.PointsChanged(null, null);
            }
        }

        private void PointsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                InitLine(ActualWidth, ActualHeight, AnimationMode == AnimationModes.Always);
            }
        }

        #endregion

        #region Funtion
        /// <summary>
        /// 重新绘制X、Y轴。
        /// </summary>
        private void Draw()
        {
            var actualWidth = this.ActualWidth;
            var actualHeight = this.ActualHeight;
            if (Points == null)
                return;

            LoadXAxis(actualWidth);
            LoadYAxis(actualHeight);
            DrawGrid(actualWidth, actualHeight);
            InitLine(actualWidth, actualHeight, AnimationMode != AnimationModes.None);
        }

        private void LoadYAxis(double actualHeight)
        {
            if (YAxis == null)
                return;

            var yAxis = YAxis.Reverse().ToArray();

            var xHeight = (actualHeight - _xHeight) / (yAxis.Length - 0.5);

            if (canvasYAxis.Children.Count > yAxis.Length)
            {
                var count = canvasYAxis.Children.Count;
                for (int i = 0; i < count - yAxis.Length; i++)
                {
                    canvasYAxis.Children.RemoveAt(canvasYAxis.Children.Count - 1);
                }
            }
            for (int i = 0; i < canvasYAxis.Children.Count; i++)
            {
                var txt = canvasYAxis.Children[i] as TextBlock;
                txt.Text = yAxis[i];
                Canvas.SetTop(txt, xHeight * (i + 0.5) - GetTexHeight(txt) / 2);
            }
            for (int i = canvasYAxis.Children.Count; i < yAxis.Length; i++)
            {
                var txt = new TextBlock()
                {
                    Text = yAxis[i],
                };
                var fore = new Binding() { Source = this, Path = new PropertyPath("Foreground"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(txt, TextBlock.ForegroundProperty, fore);
                Canvas.SetRight(txt, 10);
                Canvas.SetTop(txt, xHeight * (i + 0.5) - GetTexHeight(txt) / 2);
                canvasYAxis.Children.Add(txt);
            }
        }

        private void LoadXAxis(double actualWidth)
        {
            if (XAxis == null)
                return;

            var yWidth = (actualWidth - _yWidth) / (XAxis.Count - 0.5);

            if (canvasXAxis.Children.Count > XAxis.Count)
            {
                var count = canvasXAxis.Children.Count;
                for (int i = 0; i < count - XAxis.Count; i++)
                {
                    canvasXAxis.Children.RemoveAt(canvasXAxis.Children.Count - 1);
                }
            }
            for (int i = 0; i < canvasXAxis.Children.Count; i++)
            {
                var txt = canvasXAxis.Children[i] as TextBlock;
                txt.Text = XAxisGap == 0 ? XAxis[i] : (i % (XAxisGap + 1) != 0 ? "" : XAxis[i]);
                Canvas.SetLeft(txt, yWidth * i + _yWidth - (GetTextWidth(txt) / 2));
            }
            for (int i = canvasXAxis.Children.Count; i < XAxis.Count; i++)
            {
                var txt = new TextBlock()
                {
                    Text = XAxis[i],
                };
                var fore = new Binding() { Source = this, Path = new PropertyPath("Foreground"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(txt, TextBlock.ForegroundProperty, fore);
                Canvas.SetTop(txt, 5);
                Canvas.SetLeft(txt, yWidth * i + _yWidth - (GetTextWidth(txt) / 2));
                canvasXAxis.Children.Add(txt);
            }
        }

        private double GetTextWidth(TextBlock textBlock)
        {
            var formattedText = new FormattedText(
                    textBlock.Text,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch),
                    textBlock.FontSize,
                    Brushes.Black,
                    new NumberSubstitution(),
                     TextFormattingMode.Display);

            return formattedText.Width;
        }

        private double GetTexHeight(TextBlock textBlock)
        {
            var formattedText = new FormattedText(
                    textBlock.Text,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch),
                    textBlock.FontSize,
                    Brushes.Black,
                    new NumberSubstitution(),
                     TextFormattingMode.Display);

            return formattedText.Height;
        }


        private void DrawGrid(double actualWidth, double actualHeight)
        {
            if (YAxis == null || XAxis == null)
                return;

            var cvaHeight = actualHeight - _xHeight;
            var cvaWidth = actualWidth - _yWidth;

            var xHeight = cvaHeight / (YAxis.Count - 0.5);
            var yWidth = cvaWidth / (XAxis.Count - 0.5);

            var path = "";
            for (int i = 0; i < XAxis.Count; i++)
            {
                path += "M " + i * yWidth + ",0 V" + cvaHeight;
            }

            for (int i = 1; i <= YAxis.Count; i++)
            {
                path += "M 0," + (i - 0.5) * xHeight + " H" + cvaWidth;
            }

            pathGrid.Data = Geometry.Parse(path);
        }

        private void InitLine(double actualWidth, double actualHeight, bool usingAnima)
        {
            if (XAxis == null || YAxis == null || Points == null)
                return;

            ScaleTransform scale;
            if (usingAnima)
                scale = new ScaleTransform() { ScaleY = 0 };
            else
                scale = new ScaleTransform() { ScaleY = 1 };

            polygon.RenderTransform = scale;
            polyline.RenderTransform = scale;


            var cvaHeight = actualHeight - _xHeight;
            var cvaWidth = actualWidth - _yWidth;

            var xHeight = cvaHeight / (YAxis.Count - 0.5);
            var yWidth = cvaWidth / (XAxis.Count - 0.5);

            var realHeight = cvaHeight - xHeight * 0.5;

            polyline.Points.Clear();
            polygon.Points.Clear();
            canvasPoints.Children.Clear();

            var count = XAxis.Count > Points.Count ? Points.Count : XAxis.Count;
            for (int i = 0; i < count; i++)
            {
                var point = new Point(yWidth * i, (1 - Points[i].Value) * realHeight + 0.5 * xHeight);
                polyline.Points.Add(point);
                polygon.Points.Add(point);
                var ell = new Ellipse();
                var toolTip = new PUChartToolTip() { Header = XAxis[i], Value = Points[i].ValueTip };

                var cover = new Binding() { Source = this, Path = new PropertyPath("LineBrush"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(toolTip, PUChartToolTip.CoverBrushProperty, cover);
                ell.ToolTip = toolTip;

                var back = new Binding() { Source = this, Path = new PropertyPath("LineBrush"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(ell, Ellipse.FillProperty, back);
                var size = new Binding() { Source = this, Path = new PropertyPath("PointSize"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(ell, Ellipse.WidthProperty, size);
                BindingOperations.SetBinding(ell, Ellipse.HeightProperty, size);
                Canvas.SetLeft(ell, yWidth * i - PointSize / 2);
                if (usingAnima)
                {
                    Canvas.SetTop(ell, cvaHeight - PointSize / 2);
                    ell.BeginAnimation(Canvas.TopProperty, GetDoubleAnimation((1 - Points[i].Value) * realHeight + 0.5 * xHeight - PointSize / 2, 1));
                }
                else
                {
                    Canvas.SetTop(ell, (1 - Points[i].Value) * realHeight + 0.5 * xHeight - PointSize / 2);
                }

                canvasPoints.Children.Add(ell);
            }

            polygon.Points.Add(new Point(yWidth * (Points.Count - 1), cvaHeight));
            polygon.Points.Add(new Point(0, cvaHeight));
            if (usingAnima)
            {
                scale.BeginAnimation(ScaleTransform.ScaleYProperty, GetDoubleAnimation(1, 1));
            }
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

        #region Sys
        private void chart_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ActualWidth <= 0)
                throw new Exception("折线图控件的实际宽度必须大于0。");

            canvasYAxis.Height = this.ActualHeight;
            canvasYAxis.Width = _yWidth;

            canvasXAxis.Width = this.ActualWidth;
            canvasXAxis.Height = _xHeight;

            canvasContainer.Width = this.ActualWidth - _yWidth;
            canvasContainer.Height = this.ActualHeight - _xHeight;
            Canvas.SetLeft(canvasContainer, _yWidth);

            canvasPoints.Width = this.ActualWidth - _yWidth;
            canvasPoints.Height = this.ActualHeight - _xHeight;

            polygon.Width = this.ActualWidth - _yWidth;
            polygon.Height = this.ActualHeight - _xHeight;

            polyline.Width = this.ActualWidth - _yWidth;
            polyline.Height = this.ActualHeight - _xHeight;

            if (IsLoaded)
                Draw();
        }

        #endregion


    }






}
