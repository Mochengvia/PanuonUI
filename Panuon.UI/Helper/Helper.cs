using System.Windows;
using System.Windows.Controls;


namespace Panuon.UI
{
    public class Helper : DependencyObject
    {

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
    }
}
