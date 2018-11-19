/*==============================================================
*作者：ZEOUN
*时间：2018/11/19 11:09:29
*说明： 
*日志：2018/11/19 11:09:29 创建。
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
    public enum ButtonStyles
    {
        /// <summary>
        /// 一个常规按钮。
        /// </summary>
        General = 1,
        /// <summary>
        /// 一个带边框的空心按钮，当鼠标悬浮时才会显示背景色。
        /// <para>当鼠标移入时，该按钮的背景色将由Background变为指定的CoverBrush。</para>
        /// </summary>
        Hollow = 2,
        /// <summary>
        /// 一个带边框的空心按钮，当鼠标悬浮时才会显示前景色。
        /// <para>当鼠标移入时，该按钮的边框和前景色将由BorderBrush和Foreground变为指定的CoverBrush。</para>
        /// </summary>
        Outline = 3,
        /// <summary>
        /// 一个不带任何边框和背景色的文字按钮。
        /// <para>当鼠标移入时，该按钮的前景色将由Foreground变为指定的CoverBrush。</para>
        /// </summary>
        Link = 4,
    }

    public enum RepeatButtonStyles
    {
        /// <summary>
        /// 一个常规按钮。
        /// </summary>
        General = 1,
        /// <summary>
        /// 一个带边框的空心按钮，当鼠标悬浮时才会显示背景色。
        /// <para>当鼠标移入时，该按钮的背景色将由Background变为指定的CoverBrush。</para>
        /// </summary>
        Hollow = 2,
        /// <summary>
        /// 一个带边框的空心按钮，当鼠标悬浮时才会显示前景色。
        /// <para>当鼠标移入时，该按钮的边框和前景色将由BorderBrush和Foreground变为指定的CoverBrush。</para>
        /// </summary>
        Outline = 3,
        /// <summary>
        /// 一个不带任何边框和背景色的文字按钮。
        /// <para>当鼠标移入时，该按钮的前景色将由Foreground变为指定的CoverBrush。</para>
        /// </summary>
        Link = 4,
    }

    public enum ClickStyles
    {
        /// <summary>
        /// 点击按钮时不触发下沉操作。
        /// </summary>
        Classic,
        /// <summary>
        /// 点击时按钮下沉2个px。
        /// </summary>
        Sink,
    }

    public enum TextBoxStyles
    {
        /// <summary>
        /// 一个标准的输入框。
        /// </summary>
        General = 1,
        /// <summary>
        /// 一个输入框前带图标的输入框。
        /// </summary>
        IconGroup = 2,
    }

    public enum PasswordBoxStyles
    {
        /// <summary>
        /// 一个标准的密码框。
        /// </summary>
        General = 1,
        /// <summary>
        /// 一个密码框前带图标的密码框。
        /// </summary>
        IconGroup = 2,
    }

    public enum SelectedValuePaths
    {
        Header,
        Value
    }

    public enum DeleteModes
    {
        /// <summary>
        /// 当用户点击删除按钮时，删除项目并触发DeleteItem路由事件。
        /// </summary>
        Delete,
        /// <summary>
        /// 当用户点击删除按钮时，不直接删除项目（只触发DeleteItem路由事件）。
        /// </summary>
        EventOnly,
    }

    public enum SearchModes
    {
        /// <summary>
        /// 不显示搜索框。
        /// </summary>
        None,
        /// <summary>
        /// 在搜索框按下键盘时搜索。
        /// </summary>
        TextChanged,
        /// <summary>
        /// 当按下Enter键时发起搜索。
        /// </summary>
        Enter,
    }

    public enum CheckBoxStyles
    {
        /// <summary>
        /// 一个标准的选择框。
        /// </summary>
        General = 1,
        /// <summary>
        /// 一个经典样式的选择框。
        /// </summary>
        Classic = 2,
        /// <summary>
        /// 一个开关样式的选择框。
        /// </summary>
        Switch = 3,
        /// <summary>
        /// 一个带有左边线的选择框。
        /// </summary>
        Branch = 4,
        /// <summary>
        /// 一个类似于按钮样式的选择框。
        /// </summary>
        Button = 5,
    }

    public enum RadioButtonStyles
    {
        /// <summary>
        /// 一个标准的RadioButton。
        /// </summary>
        General = 1,
        /// <summary>
        /// 一个经典样式的RadioButton。
        /// </summary>
        Classic = 2,
        /// <summary>
        /// 一个开关样式的RadioButton。
        /// </summary>
        Switch = 3,
        /// <summary>
        /// 一个带有左边线的RadioButton。
        /// </summary>
        Branch = 4,
        /// <summary>
        ///  一个类似于按钮样式的选择框。
        /// </summary>
        Button = 5,
    }

    public enum ProgressDirections
    {
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop,
    }

    public enum ProgressBarStyles
    {
        /// <summary>
        /// 一个标准的进度条。
        /// </summary>
        General,
        /// <summary>
        /// 一个环形的进度条。
        /// </summary>
        Ring
    }

    public enum TreeViewStyles
    {
        General,
        Classic,
    }

    public enum TabControlStyles
    {
        General,
        Classic,
    }

    public enum DatePickerModes
    {
        /// <summary>
        /// 年 月 日。
        /// </summary>
        DateOnly,
        /// <summary>
        /// 时 分 秒。
        /// </summary>
        TimeOnly,
        /// <summary>
        /// 年 月 日 时 分 秒。
        /// </summary>
        DateTime,
    }

    public enum AnglePositions
    {
        Left,
        BottomLeft,
        BottomCenter,
        BottomRight,
        Right,
    }

}
