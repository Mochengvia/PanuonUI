/*==============================================================
*作者：ZEOUN
*时间：2018/10/25 12:44:39
*说明： 
*日志：2018/10/25 12:44:39 创建。
*==============================================================*/
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Panuon.UI
{
    public class PUListBox : ListBox
    {
        static PUListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUListBox), new FrameworkPropertyMetadata(typeof(PUListBox)));
        }

        #region Property
        /// <summary>
        /// 获取或设置当鼠标悬浮时ListBoxItem的背景色。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }

        public static readonly DependencyProperty CoverBrushProperty =
            DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUListBox));

        /// <summary>
        /// 获取或设置当ListBoxItem被选中时的背景色。
        /// </summary>
        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(PUListBox));

        /// <summary>
        /// 获取或设置当搜索ListBoxItem时，ListBoxItem被找到时应呈现的背景色。
        /// </summary>
        public Brush SearchBrush
        {
            get { return (Brush)GetValue(SearchBrushProperty); }
            set { SetValue(SearchBrushProperty, value); }
        }

        public static readonly DependencyProperty SearchBrushProperty =
            DependencyProperty.Register("SearchBrush", typeof(Brush), typeof(PUListBox));

        #endregion

        #region APIs
        public PUListBoxItem GetListBoxItemByValue(object value)
        {
            foreach (var item in Items)
            {
                var listBoxItem = item as PUListBoxItem;
                if (listBoxItem.Value == null ? false : listBoxItem.Value.Equals(value))
                    return listBoxItem;
            }
            return null;
        }

        public PUListBoxItem GetListBoxItemByContent(object content)
        {
            foreach (var item in Items)
            {
                var listBoxItem = item as PUListBoxItem;
                if (listBoxItem.Content == null ? false : listBoxItem.Content.Equals(content))
                    return listBoxItem;
            }
            return null;
        }

        /// <summary>
        /// 通过内容查询符合条件的第一个子项，若找到项目，则滚动到该项目并高亮。
        /// </summary>
        /// <param name="content">子项的内容。</param>
        /// <param name="allowFuzzySearch">是否允许模糊查询。</param>
        public void SearchItemByContent(string content, bool allowFuzzySearch)
        {
            foreach (var item in Items)
            {
                var listBoxItem = item as PUListBoxItem;
                if (listBoxItem.Content == null ? false : allowFuzzySearch ? listBoxItem.Content.ToString().Contains(content) : listBoxItem.Content.ToString() == content)
                {
                    ScrollIntoView(listBoxItem);
                    listBoxItem.OnSearched();
                    return;
                }
            }
        }

        public void SearchItemByValue(object value)
        {
            var item = GetListBoxItemByValue(value);
            if (item == null)
                return;
            ScrollIntoView(item);
            item.OnSearched();
        }

        #endregion

    }
}
