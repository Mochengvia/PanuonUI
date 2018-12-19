using System.Windows;
using System.Windows.Controls;


namespace Panuon.UI
{
    public class Helper : DependencyObject
    {
        #region Column & Row
        public static string GetColumnDefinition(DependencyObject obj)
        {
            return (string)obj.GetValue(ColumnDefinitionProperty);
        }

        public static void SetColumnDefinition(DependencyObject obj, string value)
        {
            obj.SetValue(ColumnDefinitionProperty, value);
        }

        public static readonly DependencyProperty ColumnDefinitionProperty =
            DependencyProperty.RegisterAttached("ColumnDefinition", typeof(string), typeof(Helper), new PropertyMetadata(OnColumnDefinitionChanged));

        private static void OnColumnDefinitionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = (string)e.NewValue;
            var ele = d as FrameworkElement;
            var parent = ele.Parent as Grid;

            if (parent == null)
                return;

            Grid.SetColumn(ele, parent.ColumnDefinitions.Count);

            var length = 0.0;

            if (value.Contains("*"))
            {
                value = value.Replace("*", "");
                double.TryParse(value ,out length);
                if (length == 0)
                    length = 1;
                parent.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(length, GridUnitType.Star) });
            }
            else
            {
                double.TryParse(value, out length);
                parent.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(length, GridUnitType.Pixel) });
            }

        }


        public static string GetRowDefinition(DependencyObject obj)
        {
            return (string)obj.GetValue(RowDefinitionProperty);
        }

        public static void SetRowDefinition(DependencyObject obj, string value)
        {
            obj.SetValue(RowDefinitionProperty, value);
        }

        public static readonly DependencyProperty RowDefinitionProperty =
            DependencyProperty.RegisterAttached("RowDefinition", typeof(string), typeof(Helper), new PropertyMetadata(OnRowDefinitionChanged));

        private static void OnRowDefinitionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = (string)e.NewValue;
            var ele = d as FrameworkElement;
            var parent = ele.Parent as Grid;

            if (parent == null)
                return;

            Grid.SetRow(ele, parent.RowDefinitions.Count);

            var length = 0.0;

            if (value.Contains("*"))
            {
                value = value.Replace("*", "");
                double.TryParse(value, out length);
                if (length == 0)
                    length = 1;
                parent.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(length, GridUnitType.Star) });
            }
            else
            {
                double.TryParse(value, out length);
                parent.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(length, GridUnitType.Pixel) });
            }
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
