using System.Windows;
using System.Windows.Controls;

namespace Panuon.UI
{
    public class PUSplitLine : Control
    {
        static PUSplitLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUSplitLine), new FrameworkPropertyMetadata(typeof(PUSplitLine)));
        }

        #region Property


      

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
