/*==============================================================
*作者：ZEOUN
*时间：2018/10/25 12:44:49
*说明： 
*日志：2018/10/25 12:44:49 创建。
*==============================================================*/
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Panuon.UI.A2_ListBox
{
    public class PUListBoxItem : ListBoxItem
    {
        static PUListBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUListBoxItem), new FrameworkPropertyMetadata(typeof(PUListBoxItem)));
        }
    }
}
