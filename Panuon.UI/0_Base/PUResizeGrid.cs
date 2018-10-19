/*==============================================================
*作者：ZEOUN
*时间：2018/10/19 10:47:53
*说明： 
*日志：2018/10/19 10:47:53 创建。
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
    public class PUResizeGrid : UserControl
    {
        static PUResizeGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUResizeGrid), new FrameworkPropertyMetadata(typeof(PUResizeGrid)));
        }
    }
}
