using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Panuon.UI
{
    /// <summary>
    /// 此控件源码来源于网络，并在原来的基础上进行了修改。
    /// </summary>
    internal class ResizeThumb : Thumb
    {
        public ResizeThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.ResizeThumb_DragDelta);
        }

        #region Property


        public bool IsSquare
        {
            get { return (bool)GetValue(IsSquareProperty); }
            set { SetValue(IsSquareProperty, value); }
        }

        public static readonly DependencyProperty IsSquareProperty =
            DependencyProperty.Register("IsSquare", typeof(bool), typeof(ResizeThumb), new PropertyMetadata(false));


        #endregion

        #region Sys
        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var item = this.DataContext as FrameworkElement;

            if (item != null)
            {
                if (!IsSquare)
                {
                    double deltaVertical, deltaHorizontal;

                    switch (VerticalAlignment)
                    {
                        case VerticalAlignment.Bottom:
                            deltaVertical = Math.Min(-e.VerticalChange,
                                item.ActualHeight - item.MinHeight);
                            item.Height -= deltaVertical;
                            break;
                        case VerticalAlignment.Top:
                            deltaVertical = Math.Min(e.VerticalChange,
                                item.ActualHeight - item.MinHeight);
                            Canvas.SetTop(item, Canvas.GetTop(item) + deltaVertical);
                            item.Height -= deltaVertical;
                            break;
                        default:
                            break;
                    }
                    if (item.Height > item.MaxHeight)
                        item.Height = item.MaxHeight;

                    switch (HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            deltaHorizontal = Math.Min(e.HorizontalChange,
                                item.ActualWidth - item.MinWidth);
                            Canvas.SetLeft(item, Canvas.GetLeft(item) + deltaHorizontal);
                            item.Width -= deltaHorizontal;
                            break;
                        case HorizontalAlignment.Right:
                            deltaHorizontal = Math.Min(-e.HorizontalChange,
                                item.ActualWidth - item.MinWidth);
                            item.Width -= deltaHorizontal;
                            break;
                        default:
                            break;
                    }
                    if (item.Width > item.MaxWidth)
                        item.Width = item.MaxWidth;
                }
                else
                {
                    double deltaVertical, deltaHorizontal;

                    switch (VerticalAlignment)
                    {
                        case VerticalAlignment.Bottom:
                            deltaVertical = Math.Min(-e.VerticalChange,
                                item.ActualHeight - item.MinHeight);
                            item.Height -= deltaVertical;
                            if (item.Height > item.MaxHeight)
                                item.Height = item.MaxHeight;
                            item.Width = item.Height;
                            return;
                        case VerticalAlignment.Top:
                            deltaVertical = Math.Min(e.VerticalChange,
                                item.ActualHeight - item.MinHeight);
                            Canvas.SetTop(item, Canvas.GetTop(item) + deltaVertical);
                            item.Height -= deltaVertical;
                            if (item.Height > item.MaxHeight)
                                item.Height = item.MaxHeight;
                            item.Width = item.Height;
                            return;
                        default:
                            break;
                    }

                    switch (HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            deltaHorizontal = Math.Min(e.HorizontalChange,
                                item.ActualWidth - item.MinWidth);
                            Canvas.SetLeft(item, Canvas.GetLeft(item) + deltaHorizontal);
                            item.Width -= deltaHorizontal;
                            break;
                        case HorizontalAlignment.Right:
                            deltaHorizontal = Math.Min(-e.HorizontalChange,
                                item.ActualWidth - item.MinWidth);
                            item.Width -= deltaHorizontal;
                            break;
                        default:
                            break;
                    }
                    if (item.Width > item.MaxWidth)
                        item.Width = item.MaxWidth;
                    item.Height = item.Width;

                    e.Handled = true;
                }
            }
        }
        #endregion
    }
}
