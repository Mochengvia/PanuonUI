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

    #region DropDown
    public enum DropDownPlacement
    {
        Bottom,
        RightBottom,
        LeftBottom
    }
    #endregion

    public enum ImageType
    {
        Rectangle,
        Square,
    }

    /// <summary>
    /// (PanuonUI) Animation styles for PUWindow.
    /// </summary>
    public enum AnimationStyles
    {
        /// <summary>
        /// 缩放。
        /// </summary>
        Scale = 0,
        /// <summary>
        /// 一个从上到下的渐变显示。
        /// </summary>
        Gradual = 1,
        /// <summary>
        /// 渐入渐出。
        /// </summary>
        Fade = 2
    }

    /// <summary>
    /// (PanuonUI) Button styles for PUButton.
    /// </summary>
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

    /// <summary>
    /// (PanuonUI) Button styles for PURepeatButton.
    /// </summary>
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

    /// <summary>
    /// (PanuonUI) Button click styles for PUButton.
    /// </summary>
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

    /// <summary>
    /// (PanuonUI) TextBox styles for PUTextBox.
    /// </summary>
    public enum TextBoxStyles
    {
        /// <summary>
        /// 一个标准的文本框。
        /// </summary>
        General = 1,
        /// <summary>
        /// 一个文本框前带图标的文本框。
        /// </summary>
        IconGroup = 2,
    }

    /// <summary>
    /// (PanuonUI) PasswordBox styles for PUPasswordBox.
    /// </summary>
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

    /// <summary>
    /// (PanuonUI) SelectedValuePath for PUTabControl , PUComboBox and PUTreeView
    /// </summary>
    public enum SelectedValuePaths
    {
        /// <summary>
        /// SelectedValue应呈现被选中子项的Header属性（在ComboBox中为Content属性）。
        /// </summary>
        Header,
        /// <summary>
        /// SelectedValue应呈现被选中子项的Value属性。
        /// </summary>
        Value
    }

    /// <summary>
    /// (PanuonUI) SelectedValuePath for PUTabControl , PUComboBox and PUTreeView
    /// </summary>
    public enum ChoosedValuePaths
    {
        /// <summary>
        /// ChoosedValue应呈现被选中子项的Header属性。
        /// </summary>
        Header,
        /// <summary>
        /// ChoosedValue应呈现被选中子项的Value属性。
        /// </summary>
        Value
    }

    /// <summary>
    /// (PanuonUI) DeleteModes for PUTabControl and PUComboBox
    /// </summary>
    public enum DeleteModes
    {
        /// <summary>
        /// 当用户点击删除按钮时，应立即删除项目并触发DeleteItem路由事件。
        /// </summary>
        Delete,
        /// <summary>
        /// 当用户点击删除按钮时，不删除项目，只触发DeleteItem路由事件。
        /// </summary>
        EventOnly,
    }

    /// <summary>
    /// (PanuonUI) SearchMode for PUComboBox
    /// </summary>
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

    /// <summary>
    /// (PanuonUI) CheckBoxStyles for PUCheckBox
    /// </summary>
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

    /// <summary>
    /// (PanuonUI) RadioButtonStyles for PURadioButton
    /// </summary>
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

    /// <summary>
    /// (PanuonUI) ProgressDirections for PUProgressBar
    /// </summary>
    public enum ProgressDirections
    {
        /// <summary>
        /// 从左到右填充进度。
        /// </summary>
        LeftToRight,
        /// <summary>
        /// 从右到左填充进度。
        /// </summary>
        RightToLeft,
        /// <summary>
        /// 从上到下填充进度。
        /// </summary>
        TopToBottom,
        /// <summary>
        /// 从下到上填充进度。
        /// </summary>
        BottomToTop,
    }

    /// <summary>
    /// (PanuonUI) ProgressBarStyles for PUProgressBar
    /// </summary>
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

    /// <summary>
    /// (PanuonUI) TreeViewStyles for PUTreeView
    /// </summary>
    public enum TreeViewStyles
    {
        General,
        Classic,
    }

    public enum ExpandModes
    {
        /// <summary>
        /// 单击TreeViewItem时展开（如果有子项）。
        /// </summary>
        Click,
        /// <summary>
        /// 双击TreeViewItem时展开（如果有子项）。
        /// </summary>
        DoubleClick,
    }

    /// <summary>
    /// (PanuonUI) TabControlStyles for PUTabControl
    /// </summary>
    public enum TabControlStyles
    {
        General,
        Classic,
    }

    /// <summary>
    /// (PanuonUI) DatePickerModes for PUDatePicker
    /// </summary>
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

    /// <summary>
    /// (PanuonUI) AnglePositions for PUBubble
    /// </summary>
    public enum AnglePositions
    {
        /// <summary>
        /// 尖角位于左侧。
        /// </summary>
        Left,
        /// <summary>
        /// 尖角位于左下角。
        /// </summary>
        BottomLeft,
        /// <summary>
        /// 尖角位于中间的底部。
        /// </summary>
        BottomCenter,
        /// <summary>
        /// 尖角位于右下角。
        /// </summary>
        BottomRight,
        /// <summary>
        /// 尖角位于右侧。
        /// </summary>
        Right,
    }

    /// <summary>
    /// (PanuonUI) Buttons for PUMessageBox
    /// </summary>
    public enum Buttons
    {
        /// <summary>
        /// 好
        /// </summary>
        Sure,
        /// <summary>
        /// 是
        /// </summary>
        Yes,
        /// <summary>
        /// 确定
        /// </summary>
        OK,
        /// <summary>
        /// 取消
        /// </summary>
        Cancel,
        /// <summary>
        /// 是/否
        /// </summary>
        YesOrNo,
        /// <summary>
        /// 是/取消
        /// </summary>
        YesOrCancel,
        /// <summary>
        /// 确定/取消
        /// </summary>
        OKOrCancel,
        /// <summary>
        /// 接受/取消
        /// </summary>
        AcceptOrCancel,
        /// <summary>
        /// 接受/拒绝
        /// </summary>
        AcceptOrRefused,
    }

    /// <summary>
    /// (PanuonUI) TextTypes for PUTextBox
    /// </summary>
    public enum TextTypes
    {
        /// <summary>
        /// 允许所有文本输入。
        /// </summary>
        Text,
        /// <summary>
        /// 只允许输入数字、以及其他操控键。
        /// </summary>
        Number,
        /// <summary>
        /// 只允许输入数字、小数点、以及其他操控键。
        /// </summary>
        Decimal,
    }
}
