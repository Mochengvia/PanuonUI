/*==============================================================
*作者：ZEOUN
*时间：2018/8/8 9:57:13
*说明： 
*日志：2018/8/8 9:57:13 创建。
*==============================================================*/
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Panuon.UI
{
    public class PUDatePicker : UserControl
    {
        static PUDatePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUDatePicker), new FrameworkPropertyMetadata(typeof(PUDatePicker)));
        }

        #region Property
        /// <summary>
        /// 鼠标悬浮时，日期按钮的颜色
        /// </summary>
        public Brush HoverBrush
        {
            get { return (Brush)GetValue(HoverBrushProperty); }
            set { SetValue(HoverBrushProperty, value); }
        }

        public static readonly DependencyProperty HoverBrushProperty =
            DependencyProperty.Register("HoverBrush", typeof(Brush), typeof(PUDatePicker));

        
        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(PUDatePicker));

        #endregion
    }
}
