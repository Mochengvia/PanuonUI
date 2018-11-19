using System.Windows;
using System.Windows.Controls;


namespace Panuon.UI
{
    public class Helper : DependencyObject
    {
        #region Column & Row
        public static double GetColumnDefinition(DependencyObject obj)
        {
            return (double)obj.GetValue(ColumnDefinitionProperty);
        }

        public static void SetColumnDefinition(DependencyObject obj, double value)
        {
            obj.SetValue(ColumnDefinitionProperty, value);
        }

        public static readonly DependencyProperty ColumnDefinitionProperty =
            DependencyProperty.RegisterAttached("ColumnDefinition", typeof(double), typeof(Helper), new PropertyMetadata(OnColumnDefinitionChanged));

        private static void OnColumnDefinitionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = (double)e.NewValue;
            var ele = d as FrameworkElement;
            var parent = ele.Parent as Grid;

            if (parent == null)
                return;

            Grid.SetColumn(ele, parent.ColumnDefinitions.Count);
            parent.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(value, GridUnitType.Star) });
        }


        public static double GetRowDefinition(DependencyObject obj)
        {
            return (double)obj.GetValue(RowDefinitionProperty);
        }

        public static void SetRowDefinition(DependencyObject obj, double value)
        {
            obj.SetValue(RowDefinitionProperty, value);
        }

        public static readonly DependencyProperty RowDefinitionProperty =
            DependencyProperty.RegisterAttached("RowDefinition", typeof(double), typeof(Helper), new PropertyMetadata(OnRowDefinitionChanged));

        private static void OnRowDefinitionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = (double)e.NewValue;
            var ele = d as FrameworkElement;
            var parent = ele.Parent as Grid;

            if (parent == null)
                return;

            Grid.SetRow(ele, parent.RowDefinitions.Count);
            parent.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(value, GridUnitType.Star) });
        }
        #endregion

        #region Height & Width


        public static double GetHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(HeightProperty);
        }

        public static void SetHeight(DependencyObject obj, double value)
        {
            obj.SetValue(HeightProperty, value);
        }

        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.RegisterAttached("Height", typeof(double), typeof(Helper), new PropertyMetadata(OnHeightChanged));

        private static void OnHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = (double)e.NewValue;
            var ele = d as FrameworkElement;
            var parent = ele.Parent as FrameworkElement;

            if (parent == null)
                return;

            parent.Loaded += delegate
            {
                ele.Height = parent.ActualWidth * value;
            };
        }



        public static double GetWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(WidthProperty);
        }

        public static void SetWidth(DependencyObject obj, double value)
        {
            obj.SetValue(WidthProperty, value);
        }

        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.RegisterAttached("Width", typeof(double), typeof(Helper), new PropertyMetadata(OnWidthChanged));

        private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = (double)e.NewValue;
            var ele = d as FrameworkElement;
            var parent = ele.Parent as FrameworkElement;

            if (parent == null)
                return;

            parent.Loaded += delegate
            {
                ele.Width = parent.ActualWidth * value;
            };
        }
        #endregion
    }
}
