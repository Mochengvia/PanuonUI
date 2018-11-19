using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Panuon.UI
{
    //ProgressBar专用内部圆角转换器
    internal class GeneralProgressBarConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //0是CornerRadius，1是Direction
            var cr = (CornerRadius)values[0];
            var dir = (ProgressDirections)values[1];
            if (dir == ProgressDirections.LeftToRight)
                return new CornerRadius(cr.TopLeft, 0, 0, cr.BottomLeft);
            else if (dir == ProgressDirections.RightToLeft)
                return new CornerRadius(0, cr.TopRight, cr.BottomRight, 0);
            else if (dir == ProgressDirections.TopToBottom)
                return new CornerRadius(cr.TopLeft, cr.TopRight, 0, 0);
            else
                return new CornerRadius(0, 0, cr.BottomRight, cr.BottomLeft);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { DependencyProperty.UnsetValue, DependencyProperty.UnsetValue };
        }
    }

    internal class RingProgressBarConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //0是CornerRadius，1是Direction
            var width = (double)values[0];
            var height = (double)values[1];
            var radius = (double)values[2];
            var percent = values.Length == 3 ? 1 : (double)values[3];


            var point1X = height / 2 * Math.Cos((2 * percent - 0.5) * Math.PI) + height / 2;
            var point1Y = height / 2 - height / 2 * Math.Sin((2 * percent  + 0.5) * Math.PI);
            var point2X = (height - radius)/ 2  * Math.Cos((2 * percent  - 0.5) * Math.PI) + height / 2;
            var point2Y = height / 2 - (height - radius) / 2  * Math.Sin((2 * percent + 0.5) * Math.PI);

            var path = "";

            if(percent == 0)
            {
                path = "";
            }
            else if (percent < 0.5)
            {
                path = "M " + width / 2 + "," + radius / 2 + " A " + (width - radius) / 2 + "," + (width - radius) / 2 + " 0 0 1 " + point2X + "," + point2Y + "";
            }
            else if(percent == 0.5)
            {
                path = "M " + width / 2 + "," + radius / 2 + " A " + (width - radius) / 2 + "," + (width - radius) / 2 + " 0 0 1 " + width / 2 + "," + (height - radius / 2);
            }
            else
            {
                path = "M " + width / 2 + "," + radius / 2 + " A " + (width - radius) / 2 + "," + (width - radius) / 2 + " 0 0 1 " + width / 2 + "," + (height - radius / 2) + 
                    " A " + (width - radius) / 2 + "," + (width - radius) / 2 + " 0 0 1 " + point2X + "," + point2Y + "";
            }
            return PathGeometry.Parse(path);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { DependencyProperty.UnsetValue, DependencyProperty.UnsetValue };
        }
    }

    internal class BubbleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var location = (AnglePositions)values[0];
            var radius = (CornerRadius)values[1];
            var width = (double)values[2];
            var height = (double)values[3];
            var path = "";
            switch (location)
            {
                case AnglePositions.Left:
                    if (radius == new CornerRadius(0))
                        path = "M0," + height / 2 + "L5," + (height / 2 + 4) +
                            "V" + height + "H" + width + "V 0 H 5 V" + (height / 2 - 4) + "Z";
                    else
                        path = "M0," + height / 2 + "L5," + (height / 2 + 4) +
                            "V" + (height - radius.BottomLeft) + "A" + radius.BottomLeft + "," + radius.BottomLeft + " 0 0 0 " + (5 + radius.BottomLeft) + "," + height +
                            "H" + (width - radius.BottomRight) + "A" + radius.BottomRight + "," + radius.BottomRight + " 0 0 0 " + width + "," + (height - radius.BottomRight) +
                            "V" + radius.TopRight + "A" + radius.TopRight + "," + radius.TopRight + " 0 0 0 " + (width - radius.TopRight) + "," + 0 +
                            "H" + (5 + radius.TopLeft) + "A" + radius.TopLeft + "," + radius.TopLeft + " 0 0 0 " + 5 + "," + radius.TopLeft +
                            "V" + (height / 2 - 4) + "Z";
                    break;
                case AnglePositions.BottomLeft:
                    if (radius == new CornerRadius(0))
                        path = "M0," + height + "L4," + (height - 5) +
                        "H " + width + "V 0 H 0 Z";
                    else
                        path = "M0," + height + "L4," + (height - 5) +
                            "H" + (width - radius.BottomRight) + "A" + radius.BottomRight +"," + radius.BottomRight + " 0 0 0 " + width + "," + (height - radius.BottomRight - 5) +
                            "V" + radius.TopRight + "A" + radius.TopRight + "," + radius.TopRight + " 0 0 0 " + (width - radius.TopRight) + "," + 0 +
                            "H" + radius.TopLeft + "A" + radius.TopLeft + "," + radius.TopLeft + " 0 0 0 " + 0 + "," + radius.TopLeft +
                            "Z";

                        break;
                case AnglePositions.BottomCenter:
                    if (radius == new CornerRadius(0))
                        path = "M" + width / 2 + "," + height + "L" + (width / 2 + 5) + "," + (height - 4) +
                            "H" + width + "V 0 H 0 V" + (height - 4) + "H" + (width / 2 - 5) + "Z";
                    else
                        path = "M" + width / 2 + "," + height + "L" + (width / 2 + 5) + "," + (height - 4) +
                            "H" + (width - radius.BottomRight) + "A" + radius.BottomRight + "," + radius.BottomRight + " 0 0 0 " + width + "," + (height - radius.BottomRight - 5) +
                            "V" + radius.TopRight + "A" + radius.TopRight + "," + radius.TopRight + " 0 0 0 " + (width - radius.TopRight) + "," + 0 +
                            "H" + radius.TopLeft + "A" + radius.TopLeft + "," + radius.TopLeft + " 0 0 0 " + 0 + "," + radius.TopLeft +
                            "V" + (height - radius.BottomRight - 5) + "A" + radius.BottomRight + "," + radius.BottomRight + " 0 0 0 " + radius.BottomRight + "," + (height - 5) +
                            "H" + (width / 2 - 5) + "Z";

                    break;
                case AnglePositions.BottomRight:
                    if (radius == new CornerRadius(0))
                        path = "M" + width + "," + height + "V 0 H 0 V " + (height - 4) + "H" + (width - 5) + "Z";
                    else
                        path = "M" + width + "," + height + "V" + radius.TopRight + "A" + radius.TopRight + ","+ radius.TopRight + " 0 0 0 " + (width - radius.TopRight) + "," + 0 +
                            "H" + radius.TopLeft + "A" + radius.TopLeft + "," + radius.TopLeft + " 0 0 0 " + 0 + "," + radius.TopLeft +
                            "V" + (height - radius.BottomRight - 5) + "A" + radius.BottomRight + "," + radius.BottomRight + " 0 0 0 " + radius.BottomRight + "," + (height - 5) + 
                            "H" + (width - 5) + "Z";
                    break;
                case AnglePositions.Right:
                    if (radius == new CornerRadius(0))
                        path = "M" + width + "," + height / 2 + "L" + (width - 5) + "," + (height / 2 - 4) + "V 0 H 0 V" +
                        height + "H " + (width - 5) + "V" + (height / 2 + 4) + "Z";
                    else
                        path = "M" + width + "," + height / 2 + "L" + (width - 5) + "," + (height / 2 - 4) + "V" + radius.TopRight + "A" + radius.TopRight + "," + radius.TopRight + " 0 0 0 " + (width - radius.TopRight - 5) + "," + 0 +
                            "H" + radius.TopLeft + "A" + radius.TopLeft + "," + radius.TopLeft + " 0 0 0 " + 0 + "," + radius.TopLeft +
                            "V" + (height - radius.BottomLeft) + "A" + radius.BottomLeft + "," + radius.BottomLeft + " 0 0 0 " + radius.BottomLeft + "," + height +
                            "H" + (width - radius.BottomRight - 5) + "A" + radius.BottomRight + "," + radius.BottomRight + " 0 0 0 " + (width - 5) + "," + (height - radius.BottomRight) +
                            "V" + (height / 2 + 4) + "Z";
                    break;
            }
            return Geometry.Parse(path);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { DependencyProperty.UnsetValue, DependencyProperty.UnsetValue };
        }
    }

    internal class WidthToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var actualWidth = (double)value;
            return new Thickness(actualWidth, 0, 0, 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    internal class MarginToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var thickness = (Thickness)value;
            return thickness.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }


    //Window Header和Title转换器
    internal class HeaderConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //0是Header，1是Title
            return (string)values[0] == null ? (string)values[1]: (string)values[0];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { DependencyProperty.UnsetValue, DependencyProperty.UnsetValue };
        }
    }
    //Icon字体大小转换器（ + 5）
    internal class IconFontSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value  + 5;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    //CheckBox General样式内部对号的缩放比例
    internal class ScaleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //0是InnerWidth，1是InnerHeight
            var minvalue = (double)values[0] < (double)values[1] ? (double)values[0] : (double)values[1];
            return minvalue / 22;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { DependencyProperty.UnsetValue, DependencyProperty.UnsetValue };
        }
    }

    internal class ToggleHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value - 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    //内部Toggle的圆角转换器
    internal class ToHalfConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    //内部Toggle的水平偏移量转换器
    internal class ToggleTranslateXConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //0是InnerWidth，1是InnerHeight
            return ((double)values[0] - (double)values[1] + 1) * -1;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { DependencyProperty.UnsetValue, DependencyProperty.UnsetValue };
        }
    }

    //输入框内部宽度转换器
    internal class TextBoxInnerWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //0是Width，1是Padding，，2是HorizontalContentAlignment
            if ((HorizontalAlignment)values[2] != HorizontalAlignment.Center)
                return (double)values[0] - ((Thickness)values[1]).Left - ((Thickness)values[1]).Right;
            else
                return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { DependencyProperty.UnsetValue, DependencyProperty.UnsetValue };
        }
    }

    //输入框内部高度转换器
    internal class TextBoxInnerHeightConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //0是Heihgt，1是Padding，2是VerticalContentAlignment
            if ((VerticalAlignment)values[2] != VerticalAlignment.Center)
                return (double)values[0] - ((Thickness)values[1]).Top - ((Thickness)values[1]).Bottom;
            else
                return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { DependencyProperty.UnsetValue, DependencyProperty.UnsetValue };
        }
    }
}
