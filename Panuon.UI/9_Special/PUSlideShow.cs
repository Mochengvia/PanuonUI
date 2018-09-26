using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Panuon.UI
{
    public class PUSlideShow : ItemsControl
    {
        static PUSlideShow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUSlideShow), new FrameworkPropertyMetadata(typeof(PUSlideShow)));
        }

        /// <summary>
        /// 当前的页码（从0开始计数），默认值为0。
        /// </summary>
        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }
        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register("Index", typeof(int), typeof(PUSlideShow), new PropertyMetadata(0, IndexOnChanged));

        private static void IndexOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slideShow = d as PUSlideShow;
        }
    }
}
