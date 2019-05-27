using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Panuon.UI
{
    /// <summary>
    /// Loading.xaml 的交互逻辑
    /// </summary>
    public partial class PULoading : UserControl
    {
        private Storyboard _storyboard;

        public PULoading()
        {
            InitializeComponent();
            Loaded += PULoading_Loaded;
        }

        private void PULoading_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsRunning)
                Draw();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #region Property
        public bool IsRunning
        {
            get { return (bool)GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, value); }
        }
        public static readonly DependencyProperty IsRunningProperty = DependencyProperty.Register("IsRunning", typeof(bool), typeof(PULoading), new PropertyMetadata(false, OnIsRunningChanged));

        private static void OnIsRunningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var load = d as PULoading;
            if (!load.IsLoaded)
                return;
            var run = (bool)e.NewValue;
            if (run)
                load.Draw();
            else
                load.Clear();
        }

        #endregion

        #region Internal APIs
        internal void Draw()
        {
            _storyboard = new Storyboard()
            {
                RepeatBehavior = RepeatBehavior.Forever,
            };

            double canvasHeight = 0;
            if (double.IsNaN(Height))
            {
                if (ActualHeight == 0)
                {
                    return;
                }
                else
                {
                    canvasHeight = ActualHeight;
                }
            }
            else
            {
                canvasHeight = this.Height;
            }
            var ellipseHeight = canvasHeight * 0.1;
            var rollHeight = canvasHeight * 0.75;
            var rollRadius = rollHeight / 2;

            for (int i = 0; i < 6; i++)
            {
                var ellipse = new Ellipse()
                {
                    Fill = Foreground,
                    Width = ellipseHeight,
                    Height = ellipseHeight,
                    Margin = new Thickness(-ellipseHeight / 2, -ellipseHeight / 2, 0, 0),
                    Opacity = 0,
                };
                Canvas.SetTop(ellipse, canvasHeight * 0.125);
                Canvas.SetLeft(ellipse, canvasHeight / 2);

                var path = $"M{canvasHeight / 2},{canvasHeight * 0.125} A {rollRadius},{rollRadius} 0 0 1 {canvasHeight / 2},{rollHeight + canvasHeight * 0.125} A {rollRadius},{rollRadius} 0 0 1 {canvasHeight / 2},{canvasHeight * 0.125} A {rollRadius},{rollRadius} 0 0 1 {canvasHeight / 2},{rollHeight + canvasHeight * 0.125} A {rollRadius},{rollRadius} 0 0 1 {canvasHeight / 2},{canvasHeight * 0.125}";

                var anima1 = GetDoubleAnimation(1, (i + i * 0.3) * 0.1, 0.01);
                Storyboard.SetTarget(anima1, ellipse);
                Storyboard.SetTargetProperty(anima1, new PropertyPath("Opacity"));
                _storyboard.Children.Add(anima1);
                var animaX = GetDoubleAnimationUsingPath(path, (i + i * 0.3) * 0.1, 1.5, PathAnimationSource.X);
                Storyboard.SetTarget(animaX, ellipse);
                Storyboard.SetTargetProperty(animaX, new PropertyPath("(Canvas.Left)"));
                _storyboard.Children.Add(animaX);
                var animaY = GetDoubleAnimationUsingPath(path, (i + i * 0.3) * 0.1, 1.5, PathAnimationSource.Y);
                Storyboard.SetTarget(animaY, ellipse);
                Storyboard.SetTargetProperty(animaY, new PropertyPath("(Canvas.Top)"));
                _storyboard.Children.Add(animaY);
                var anima2 = GetDoubleAnimation(0, 1.05 + 0.1 * i, 0.5);
                Storyboard.SetTarget(anima2, ellipse);
                Storyboard.SetTargetProperty(anima2, new PropertyPath("Opacity"));
                _storyboard.Children.Add(anima2);

                cvaMain.Children.Add(ellipse);
            }
            _storyboard.Begin();
        }

        internal void Clear()
        {
            _storyboard.Stop();
            cvaMain.Children.Clear();
        }
        #endregion


        #region Function
        private DoubleAnimation GetDoubleAnimation(double to, double beginTime, double duration, IEasingFunction easingFunction = null)
        {
            return new DoubleAnimation()
            {
                To = to,
                BeginTime = TimeSpan.FromSeconds(beginTime),
                Duration = TimeSpan.FromSeconds(duration),
                EasingFunction = easingFunction,
            };
        }

        private DoubleAnimationUsingPath GetDoubleAnimationUsingPath(string path, double beginTime, double duration, PathAnimationSource source)
        {
            return new DoubleAnimationUsingPath()
            {
                BeginTime = TimeSpan.FromSeconds(beginTime),
                PathGeometry = Geometry.Parse(path).GetFlattenedPathGeometry(),
                Duration = TimeSpan.FromSeconds(duration),
                Source = source,
            };
        }
        #endregion
    }
}
