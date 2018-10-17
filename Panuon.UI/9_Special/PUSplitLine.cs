using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PUSplitLine : Control
    {
        static PUSplitLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUSplitLine), new FrameworkPropertyMetadata(typeof(PUSplitLine)));
        }

        #region Property


        public Brush LineBrush
        {
            get { return (Brush)GetValue(LineBrushProperty); }
            set { SetValue(LineBrushProperty, value); }
        }

        public static readonly DependencyProperty LineBrushProperty =
            DependencyProperty.Register("LineBrush", typeof(Brush), typeof(PUSplitLine));

    
        /// <summary>
        /// 分割线的宽度，默认值为1.0。
        /// </summary>
        public double Thickness
        {
            get { return (double)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register("Thickness", typeof(double), typeof(PUSplitLine));


        /// <summary>
        /// 分割线的停靠方向，默认为Bottom（底部）。
        /// </summary>
        public Alignments Alignment
        {
            get { return (Alignments)GetValue(AlignmentProperty); }
            set { SetValue(AlignmentProperty, value); }
        }

        public static readonly DependencyProperty AlignmentProperty =
            DependencyProperty.Register("Alignment", typeof(Alignments), typeof(PUSplitLine), new PropertyMetadata(Alignments.Bottom));

        public enum Alignments
        {
            Left,Top,Right,Bottom
        }


            
        #endregion
    }
}
