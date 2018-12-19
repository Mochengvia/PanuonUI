using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Panuon.UI.Charts
{
    /// <summary>
    /// 适用于LineChart折线图的Points属性。
    /// </summary>
    public class PUChartPoint
    {
        /// <summary>
        /// 从0 ~ 1的值，表示该点在纵轴上的高度比例。
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// 当鼠标悬浮在该点上时应该显示的实际值。
        /// </summary>
        public string ValueTip { get; set; }
    }

}
