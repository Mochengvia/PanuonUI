using Panuon.UI.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Panuon.UI
{
    /// <summary>
    /// DatePicker.xaml 的交互逻辑
    /// </summary>
    public partial class PUDatePicker : UserControl
    {

        public PUDatePicker()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Loaded += delegate
            {
                ReLoad();
            };
        }

        #region Property
        /// <summary>
        /// 获取或设置主题颜色。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }

        public static readonly DependencyProperty CoverBrushProperty =
            DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUDatePicker), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E3E3E"))));

        /// <summary>
        /// 获取或设置可以选择的最大日期时间。
        /// </summary>
        public DateTime? MaxDateTime
        {
            get { return (DateTime?)GetValue(MaxDateTimeProperty); }
            set { SetValue(MaxDateTimeProperty, value); }
        }

        public static readonly DependencyProperty MaxDateTimeProperty =
            DependencyProperty.Register("MaxDateTime", typeof(DateTime?), typeof(PUDatePicker), new PropertyMetadata(OnMaxDateTimeChanged));

        private static void OnMaxDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var picker = d as PUDatePicker;
            if (!picker.IsLoaded)
                return;

            picker.CheckDateTimeLimit();
        }

        /// <summary>
        /// 获取或设置可以选择的最小日期时间。
        /// </summary>
        public DateTime? MinDateTime
        {
            get { return (DateTime?)GetValue(MinDateTimeProperty); }
            set { SetValue(MinDateTimeProperty, value); }
        }

        public static readonly DependencyProperty MinDateTimeProperty =
            DependencyProperty.Register("MinDateTime", typeof(DateTime?), typeof(PUDatePicker), new PropertyMetadata(OnMinDateTimeChanged));

        private static void OnMinDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var picker = d as PUDatePicker;
            if (!picker.IsLoaded)
                return;

                picker.CheckDateTimeLimit();
        }

        public DateTime SelectedDateTime
        {
            get { return (DateTime)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateTimeProperty =
            DependencyProperty.Register("SelectedDateTime", typeof(DateTime), typeof(PUDatePicker), new PropertyMetadata(DateTime.Now.ToDateOnly(), OnSelectedDateTimeChanged));

        private static void OnSelectedDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;

            var picker = d as PUDatePicker;
            if (!picker.IsLoaded)
                return;
            var oldDate = (DateTime)e.OldValue;
            var newDate = (DateTime)e.NewValue;

            if (oldDate.Year != newDate.Year || (oldDate.Year == newDate.Year && oldDate.Month != newDate.Month))
            {
                picker.ResetDate(newDate.Year, newDate.Month);
                picker.CheckDateTimeLimit();
            }
            picker.SelectDate(newDate.Year, newDate.Month, newDate.Day);
            picker.SelectTime(newDate.Hour, newDate.Minute, newDate.Second);
        }


        /// <summary>
        /// 获取或设置日期选择器的模式。默认值为仅年月日（DateOnly）。
        /// </summary>
        public DatePickerModes DatePickerMode
        {
            get { return (DatePickerModes)GetValue(DatePickerModeProperty); }
            set { SetValue(DatePickerModeProperty, value); }
        }

        public static readonly DependencyProperty DatePickerModeProperty =
            DependencyProperty.Register("DatePickerMode", typeof(DatePickerModes), typeof(PUDatePicker), new PropertyMetadata(DatePickerModes.DateTime, OnDatePickerModeChanged));

        private static void OnDatePickerModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var picker = d as PUDatePicker;
            if (!picker.IsLoaded)
                return;
            picker.ReLoad();
        }

        #endregion

        #region Function
        private void ReLoad()
        {
            GrdDate.Visibility = Visibility.Visible;
            GrdTime.Visibility = Visibility.Hidden;
            switch (DatePickerMode)
            {
                case DatePickerModes.DateOnly:
                    ClearTimePanel();
                    LoadDatePanel();
                    ResetDate(SelectedDateTime.Year, SelectedDateTime.Month);
                    SelectDate(SelectedDateTime.Year, SelectedDateTime.Month, SelectedDateTime.Day);
                    break;
                case DatePickerModes.TimeOnly:
                    GrdDate.Visibility = Visibility.Hidden;
                    GrdTime.Visibility = Visibility.Visible;
                    LoadTimePanel();
                    ClearDatePanel();
                    SelectTime(SelectedDateTime.Hour, SelectedDateTime.Minute, SelectedDateTime.Second);
                    break;
                case DatePickerModes.DateTime:
                    LoadTimePanel();
                    LoadDatePanel();
                    ResetDate(SelectedDateTime.Year, SelectedDateTime.Month);
                    SelectDate(SelectedDateTime.Year, SelectedDateTime.Month, SelectedDateTime.Day);
                    SelectTime(SelectedDateTime.Hour, SelectedDateTime.Minute, SelectedDateTime.Second);
                    break;
            }
            CheckDateTimeLimit();
        }

        /// <summary>
        /// 清空并重新加载日期Panel。
        /// </summary>
        private void LoadDatePanel()
        {
            ClearDatePanel();
            for (int i = 0; i < 42; i++)
            {
                var radio = new PURadioButton
                {
                    RadioButtonStyle = PURadioButton.RadioButtonStyles.Button,
                    Content = i.ToString("00"),
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(0),
                };
                Grid.SetRow(radio, (int)(i / 7));
                Grid.SetColumn(radio, i % 7);
                var cover = new Binding { Path = new PropertyPath("CoverBrush"), Source = this, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(radio, PURadioButton.CoverBrushProperty, cover);
                radio.Click += DateRadioButton_Click;
                GrdDatePanel.Children.Add(radio);
            }
        }

        /// <summary>
        /// 清空日期Panel。
        /// </summary>
        private void ClearDatePanel()
        {
            GrdDatePanel.Children.Clear();
        }

        /// <summary>
        /// 清空并重新加载时间Panel。
        /// </summary>
        private void LoadTimePanel()
        {
            ClearTimePanel();
            for (int i = 0; i < 24; i++)
            {
                var radio = new PURadioButton
                {
                    RadioButtonStyle = PURadioButton.RadioButtonStyles.Button,
                    Content = i.ToString("00"),
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(0),
                    Tag = i,
                };
                Grid.SetRow(radio, i / 7);
                Grid.SetColumn(radio, i % 7);
                var cover = new Binding { Path = new PropertyPath("CoverBrush"), Source = this, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(radio, PURadioButton.CoverBrushProperty, cover);
                radio.Click += HourRadioButton_Click;
                StkHour.Children.Add(radio);
            }
            for (int i = 0; i < 60; i++)
            {
                var radio = new PURadioButton
                {
                    RadioButtonStyle = PURadioButton.RadioButtonStyles.Button,
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(0),
                    Content = i.ToString("00"),
                    Tag = i,
                };
                Grid.SetRow(radio, i / 7);
                Grid.SetColumn(radio, i % 7);
                var cover = new Binding { Path = new PropertyPath("CoverBrush"), Source = this, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(radio, PURadioButton.CoverBrushProperty, cover);
                radio.Click += MinuteRadioButton_Click;
                StkMinute.Children.Add(radio);
            }
            for (int i = 0; i < 60; i++)
            {
                var radio = new PURadioButton
                {
                    RadioButtonStyle = PURadioButton.RadioButtonStyles.Button,
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(0),
                    Content = i.ToString("00"),
                    Tag = i,
                };
                Grid.SetRow(radio, i / 7);
                Grid.SetColumn(radio, i % 7);
                var cover = new Binding { Path = new PropertyPath("CoverBrush"), Source = this, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(radio, PURadioButton.CoverBrushProperty, cover);
                radio.Click += SecondRadioButton_Click;
                StkSecond.Children.Add(radio);
            }
        }

        /// <summary>
        /// 清空时间Panel。
        /// </summary>
        private void ClearTimePanel()
        {
            StkHour.Children.Clear();
            StkMinute.Children.Clear();
            StkSecond.Children.Clear();
        }

        /// <summary>
        /// 重新设置RadioButton的日期。
        /// </summary>
        private void ResetDate(int year, int month)
        {
            if (GrdDatePanel.Children.Count != 42)
                return;

            BtnYear.Content = year + "年";
            BtnMonth.Content = month + "月";

            var currentMonth = new DateTime(year, month, 1);
            var lastMonth = currentMonth.AddMonths(-1);
            var nextMonth = currentMonth.AddMonths(1);

            //获取本月第一天是星期几
            var firstDay = (int)currentMonth.DayOfWeek;
            //获取上个月最后一天是几号。
            var lastDay = DateTime.DaysInMonth(lastMonth.Year, lastMonth.Month);
            //获取本月的天数。
            var totalDay = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);

            for (int i = 0; i < firstDay; i++)
            {
                var date = new DateTime(lastMonth.Year, lastMonth.Month, lastDay - firstDay + i);
                var radio = GrdDatePanel.Children[i] as PURadioButton;
                radio.Opacity = 0.7;
                radio.Content = (lastDay - firstDay + i).ToString();
                radio.Tag = date;

                if (MaxDateTime == null && MinDateTime == null)
                    radio.IsEnabled = true;
                else if (MaxDateTime != null && MinDateTime != null)
                {
                    if (date <= ((DateTime)MaxDateTime).ToDateOnly() && date >= ((DateTime)MinDateTime).ToDateOnly())
                        radio.IsEnabled = true;
                    else
                    {
                        radio.IsEnabled = false;
                        radio.Opacity = 0.2;
                    }
                }
                else if (MaxDateTime != null && date <= ((DateTime)MaxDateTime).ToDateOnly())
                    radio.IsEnabled = true;
                else if (MinDateTime != null && date >= ((DateTime)MinDateTime).ToDateOnly())
                    radio.IsEnabled = true;
                else
                {
                    radio.IsEnabled = false;
                    radio.Opacity = 0.2;
                }
            }

            for (int i = firstDay; i < firstDay + totalDay; i++)
            {
                var date = new DateTime(currentMonth.Year, currentMonth.Month, i - firstDay + 1);
                var radio = GrdDatePanel.Children[i] as PURadioButton;
                radio.Opacity = 1;
                radio.Content = i - firstDay + 1;
                radio.Tag = date;

                if (MaxDateTime == null && MinDateTime == null)
                    radio.IsEnabled = true;
                else if (MaxDateTime != null && MinDateTime != null)
                {
                    if (date <= ((DateTime)MaxDateTime).ToDateOnly() && date >= ((DateTime)MinDateTime).ToDateOnly())
                        radio.IsEnabled = true;
                    else
                    {
                        radio.IsEnabled = false;
                        radio.Opacity = 0.2;
                    }
                }
                else if (MaxDateTime != null && date <= ((DateTime)MaxDateTime).ToDateOnly())
                    radio.IsEnabled = true;
                else if (MinDateTime != null && date >= ((DateTime)MinDateTime).ToDateOnly())
                    radio.IsEnabled = true;
                else
                {
                    radio.IsEnabled = false;
                    radio.Opacity = 0.2;
                }
            }

            for (int i = firstDay + totalDay; i < 42; i++)
            {
                var date = new DateTime(nextMonth.Year, nextMonth.Month, i - firstDay - totalDay + 1);
                var radio = GrdDatePanel.Children[i] as PURadioButton;
                radio.Opacity = 0.7;
                radio.Content = i - firstDay - totalDay + 1;
                radio.Tag = date;

                if (MaxDateTime == null && MinDateTime == null)
                    radio.IsEnabled = true;
                else if (MaxDateTime != null && MinDateTime != null)
                {
                    if (date <= ((DateTime)MaxDateTime).ToDateOnly() && date >= ((DateTime)MinDateTime).ToDateOnly())
                        radio.IsEnabled = true;
                    else
                    {
                        radio.IsEnabled = false;
                        radio.Opacity = 0.2;
                    }
                }
                else if (MaxDateTime != null && date <= ((DateTime)MaxDateTime).ToDateOnly())
                    radio.IsEnabled = true;
                else if (MinDateTime != null && date >= ((DateTime)MinDateTime).ToDateOnly())
                    radio.IsEnabled = true;
                else
                {
                    radio.IsEnabled = false;
                    radio.Opacity = 0.2;
                }
            }
        }

        private void SelectDate(int year, int month, int day)
        {
            if (GrdDatePanel.Children.Count != 42)
                return;
            for (int i = 0; i < 42; i++)
            {
                var radio = GrdDatePanel.Children[i] as PURadioButton;
                if (radio.Content.ToString() != day.ToString())
                    continue;
                var date = (DateTime)radio.Tag;
                if (date.Year == year && date.Month == month)
                {
                    radio.IsChecked = true;
                    return;
                }
            }

        }

        /// <summary>
        ///  重新设置RadioButton的时间（如果必要），选中指定的时间。
        /// </summary>
        private void SelectTime(int hour, int minute, int second)
        {
            if (StkHour.Children.Count != 24 || StkMinute.Children.Count != 60 || StkSecond.Children.Count != 60)
                return;
            {
                var radio = StkHour.Children[hour] as PURadioButton;
                radio.IsChecked = true;
                ScrollHour.ScrollToVerticalOffset((hour - 2) * radio.ActualHeight);
            }
            {
                var radio = StkMinute.Children[minute] as PURadioButton;
                radio.IsChecked = true;
                ScrollMinute.ScrollToVerticalOffset((minute - 2) * radio.ActualHeight);
            }
            {
                var radio = StkSecond.Children[second] as PURadioButton;
                radio.IsChecked = true;
                ScrollSecond.ScrollToVerticalOffset((second - 2) * radio.ActualHeight);
            }
        }

        private void CheckDateTimeLimit()
        {
            if (MaxDateTime != null)
            {
                var max = (DateTime)MaxDateTime;

                if (SelectedDateTime > max)
                {
                    SelectedDateTime = max;
                    return;
                }
                else
                    ResetDate(SelectedDateTime.Year, SelectedDateTime.Month);

                if (SelectedDateTime.Year >= max.Year)
                    BtnAddYear.Visibility = Visibility.Hidden;
                else
                    BtnAddYear.Visibility = Visibility.Visible;

                if (SelectedDateTime.Year > max.Year || (SelectedDateTime.Year == max.Year && SelectedDateTime.Month >= max.Month))
                    BtnAddMonth.Visibility = Visibility.Hidden;
                else
                    BtnAddMonth.Visibility = Visibility.Visible;

                
            }
            else
            {
                BtnAddYear.Visibility = Visibility.Visible;
                BtnAddMonth.Visibility = Visibility.Visible;
            }

            if (MinDateTime != null)
            {
                var min = (DateTime)MinDateTime;

                if (SelectedDateTime > min)
                {
                    SelectedDateTime = min;
                    return;
                }
                else
                    ResetDate(SelectedDateTime.Year, SelectedDateTime.Month);

                if (SelectedDateTime.Year <= min.Year)
                    BtnDecYear.Visibility = Visibility.Hidden;
                else
                    BtnDecYear.Visibility = Visibility.Visible;

                if (SelectedDateTime.Year < min.Year || (SelectedDateTime.Year == min.Year && SelectedDateTime.Month <= min.Month))
                    BtnDecMonth.Visibility = Visibility.Hidden;
                else
                    BtnDecMonth.Visibility = Visibility.Visible;

            }
            else
            {
                BtnDecYear.Visibility = Visibility.Visible;
                BtnDecMonth.Visibility = Visibility.Visible;
            }
        }
        #endregion

        #region Event
        private void DateRadioButton_Click(object sender, RoutedEventArgs e)
        {
            var radio = sender as PURadioButton;
            if (radio.Tag == null)
                return;

            var date = (DateTime)radio.Tag;
            SelectedDateTime = new DateTime(date.Year, date.Month, date.Day, SelectedDateTime.Hour, SelectedDateTime.Minute, SelectedDateTime.Second);

            if (radio.IsChecked == true)
            {
                if (DatePickerMode == DatePickerModes.DateTime)
                {
                    GrdDate.Visibility = Visibility.Hidden;
                    GrdTime.Visibility = Visibility.Visible;
                }
                return;
            }
        }

        private void HourRadioButton_Click(object sender, RoutedEventArgs e)
        {
            var radio = sender as PURadioButton;
            if (radio.Tag == null)
                return;
            var hour = (int)radio.Tag;
            SelectedDateTime = new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, SelectedDateTime.Day, hour, SelectedDateTime.Minute, SelectedDateTime.Second);
        }

        private void MinuteRadioButton_Click(object sender, RoutedEventArgs e)
        {
            var radio = sender as PURadioButton;
            if (radio.Tag == null)
                return;
            var minute = (int)radio.Tag;
            SelectedDateTime = new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, SelectedDateTime.Day, SelectedDateTime.Hour, minute, SelectedDateTime.Second);
        }

        private void SecondRadioButton_Click(object sender, RoutedEventArgs e)
        {
            var radio = sender as PURadioButton;
            if (radio.Tag == null)
                return;
            var second = (int)radio.Tag;
            SelectedDateTime = new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, SelectedDateTime.Day, SelectedDateTime.Hour, SelectedDateTime.Minute, second);
        }

        private void BtnDecYear_Click(object sender, RoutedEventArgs e)
        {
            if (MinDateTime == null || SelectedDateTime.AddYears(-1) >= MinDateTime)
                SelectedDateTime = SelectedDateTime.AddYears(-1);
            else
                SelectedDateTime = (DateTime)MinDateTime;
        }

        private void BtnDecMonth_Click(object sender, RoutedEventArgs e)
        {
            if (MinDateTime == null || SelectedDateTime.AddMonths(-1) >= MinDateTime)
                SelectedDateTime = SelectedDateTime.AddMonths(-1);
            else
                SelectedDateTime = (DateTime)MinDateTime;
        }

        private void BtnAddMonth_Click(object sender, RoutedEventArgs e)
        {
            if (MaxDateTime == null || SelectedDateTime.AddMonths(1) <= MaxDateTime)
                SelectedDateTime = SelectedDateTime.AddMonths(1);
            else
                SelectedDateTime = (DateTime)MaxDateTime;
        }

        private void BtnAddYear_Click(object sender, RoutedEventArgs e)
        {
            if (MaxDateTime == null || SelectedDateTime.AddYears(1) <= MaxDateTime)
                SelectedDateTime = SelectedDateTime.AddYears(1);
            else
                SelectedDateTime = (DateTime)MaxDateTime;
        }

        private void BtnBackToDate_Click(object sender, RoutedEventArgs e)
        {
            GrdDate.Visibility = Visibility.Visible;
            GrdTime.Visibility = Visibility.Hidden;
        }

        #endregion

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

    }
}
