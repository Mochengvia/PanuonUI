using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

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
        /// 获取或设置主题颜色，默认值为#3E3E3E。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }

        public static readonly DependencyProperty CoverBrushProperty =
            DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUDatePicker), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E3E3E"))));

        /// <summary>
        /// 获取或设置可以选择的最大日期。该属性不能限制用户选择的时间，
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
        /// 获取或设置可以选择的最小日期。该属性不能限制用户选择的时间，
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

        /// <summary>
        /// 获取或设置当前选中的日期和时间。
        /// </summary>
        public DateTime SelectedDateTime
        {
            get { return (DateTime)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateTimeProperty =
            DependencyProperty.Register("SelectedDateTime", typeof(DateTime), typeof(PUDatePicker), new PropertyMetadata(DateTime.Now.Date, OnSelectedDateTimeChanged));

        private static void OnSelectedDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;

            var picker = d as PUDatePicker;
            if (!picker.IsLoaded)
                return;
            var oldDate = (DateTime)e.OldValue;
            var newDate = (DateTime)e.NewValue;
            if (picker.MaxDateTime != null)
            {
                var max = (DateTime)picker.MaxDateTime;
                if (newDate > max)
                {
                    picker.SelectedDateTime = max;
                    return;
                }
            }

            if (picker.MinDateTime != null)
            {
                var min = (DateTime)picker.MinDateTime;
                if (newDate < min)
                {
                    picker.SelectedDateTime = min;
                    return;
                }
            }

            if (oldDate.Year != newDate.Year || (oldDate.Year == newDate.Year && oldDate.Month != newDate.Month))
            {
                picker.ResetAndSelectYear(newDate.Year);
                picker.ResetAndSelectMonth(newDate.Month);
                picker.ResetDate(newDate.Year, newDate.Month);
                picker.CheckDateTimeLimit();
            }
            picker.SelectDate(newDate.Year, newDate.Month, newDate.Day);
            picker.SelectTime(newDate.Hour, newDate.Minute, newDate.Second);
        }


        /// <summary>
        /// 获取或设置日期选择器的模式。默认值为日期和时间（DateTime）。
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
            GrdYear.Visibility = Visibility.Hidden;
            GrdMonth.Visibility = Visibility.Hidden;

            switch (DatePickerMode)
            {
                case DatePickerModes.DateOnly:
                    ClearTimePanel();
                    LoadYearPanel();
                    LoadMonthPanel();
                    LoadDatePanel();
                    BtnBackToDate.Visibility = Visibility.Visible;
                    ResetDate(SelectedDateTime.Year, SelectedDateTime.Month);
                    SelectDate(SelectedDateTime.Year, SelectedDateTime.Month, SelectedDateTime.Day);
                    break;
                case DatePickerModes.TimeOnly:
                    GrdDate.Visibility = Visibility.Hidden;
                    GrdTime.Visibility = Visibility.Visible;
                    LoadTimePanel();
                    ClearDatePanel();
                    ClearYearPanel();
                    ClearMonthPanel();
                    BtnBackToDate.Visibility = Visibility.Hidden;
                    SelectTime(SelectedDateTime.Hour, SelectedDateTime.Minute, SelectedDateTime.Second);
                    break;
                case DatePickerModes.DateTime:
                    LoadYearPanel();
                    LoadMonthPanel();
                    LoadDatePanel();
                    LoadTimePanel();
                    BtnBackToDate.Visibility = Visibility.Visible;
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
                    RadioButtonStyle = RadioButtonStyles.Button,
                    Content = i.ToString("00"),
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(0),
                };
                Grid.SetRow(radio, (int)(i / 7));
                Grid.SetColumn(radio, i % 7);
                var fore = new Binding { Path = new PropertyPath("Foreground"), Source = this, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(radio, PURadioButton.ForegroundProperty, fore);
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
                    RadioButtonStyle = RadioButtonStyles.Button,
                    Content = i.ToString("00"),
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(0),
                    Tag = i,
                };
                Grid.SetRow(radio, i / 7);
                Grid.SetColumn(radio, i % 7);
                var fore = new Binding { Path = new PropertyPath("Foreground"), Source = this, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(radio, PURadioButton.ForegroundProperty, fore); var cover = new Binding { Path = new PropertyPath("CoverBrush"), Source = this, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(radio, PURadioButton.CoverBrushProperty, cover);
                radio.Click += HourRadioButton_Click;
                StkHour.Children.Add(radio);
            }
            for (int i = 0; i < 60; i++)
            {
                var radio = new PURadioButton
                {
                    RadioButtonStyle = RadioButtonStyles.Button,
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(0),
                    Content = i.ToString("00"),
                    Tag = i,
                };
                Grid.SetRow(radio, i / 7);
                Grid.SetColumn(radio, i % 7);
                var fore = new Binding { Path = new PropertyPath("Foreground"), Source = this, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(radio, PURadioButton.ForegroundProperty, fore); var cover = new Binding { Path = new PropertyPath("CoverBrush"), Source = this, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(radio, PURadioButton.CoverBrushProperty, cover);
                radio.Click += MinuteRadioButton_Click;
                StkMinute.Children.Add(radio);
            }
            for (int i = 0; i < 60; i++)
            {
                var radio = new PURadioButton
                {
                    RadioButtonStyle = RadioButtonStyles.Button,
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(0),
                    Content = i.ToString("00"),
                    Tag = i,
                };
                Grid.SetRow(radio, i / 7);
                Grid.SetColumn(radio, i % 7);
                var fore = new Binding { Path = new PropertyPath("Foreground"), Source = this, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(radio, PURadioButton.ForegroundProperty, fore); var cover = new Binding { Path = new PropertyPath("CoverBrush"), Source = this, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
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
        /// 清空并重新加载年份Panel。
        /// </summary>
        private void LoadYearPanel()
        {
            ClearYearPanel();
            for (int i = 0; i < 15; i++)
            {
                var radio = new PURadioButton
                {
                    RadioButtonStyle = RadioButtonStyles.Button,
                    Content = i.ToString("00"),
                    Height = 35,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(0),
                    Tag = i,
                };
                Grid.SetRow(radio, i / 3);
                Grid.SetColumn(radio, i % 3);
                var fore = new Binding { Path = new PropertyPath("Foreground"), Source = this, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(radio, PURadioButton.ForegroundProperty, fore); var cover = new Binding { Path = new PropertyPath("CoverBrush"), Source = this, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(radio, PURadioButton.CoverBrushProperty, cover);
                radio.Click += YearRadioButton_Click;
                GrdYearPanel.Children.Add(radio);
            }
        }

        /// <summary>
        /// 清空年份Panel。
        /// </summary>
        private void ClearYearPanel()
        {
            GrdYearPanel.Children.Clear();
        }

        /// <summary>
        /// 清空并重新加载月份Panel。
        /// </summary>
        private void LoadMonthPanel()
        {
            ClearMonthPanel();
            for (int i = 1; i <= 12; i++)
            {
                var radio = new PURadioButton
                {
                    RadioButtonStyle = RadioButtonStyles.Button,
                    Content = i + "月",
                    Height = 35,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(0),
                    Tag = i,
                };
                Grid.SetRow(radio, (i - 1) / 4);
                Grid.SetColumn(radio, (i - 1) % 4);
                var fore = new Binding { Path = new PropertyPath("Foreground"), Source = this, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(radio, PURadioButton.ForegroundProperty, fore); var cover = new Binding { Path = new PropertyPath("CoverBrush"), Source = this, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                BindingOperations.SetBinding(radio, PURadioButton.CoverBrushProperty, cover);
                radio.Click += MonthRadioButton_Click;
                GrdMonthPanel.Children.Add(radio);
            }
        }

        /// <summary>
        /// 清空月份Panel。
        /// </summary>
        private void ClearMonthPanel()
        {
            GrdMonthPanel.Children.Clear();
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
                var date = new DateTime(lastMonth.Year, lastMonth.Month, lastDay - firstDay + i + 1);
                var radio = GrdDatePanel.Children[i] as PURadioButton;
                radio.Opacity = 0.5;
                radio.Content = (lastDay - firstDay + i + 1).ToString();
                radio.Tag = date;

                if (MaxDateTime == null && MinDateTime == null)
                {
                    radio.IsEnabled = true;
                    radio.Opacity = 0.5;
                }
                else if (MaxDateTime != null && MinDateTime != null)
                {
                    if (date <= ((DateTime)MaxDateTime).Date && date >= ((DateTime)MinDateTime).Date)
                    {
                        radio.IsEnabled = true;
                        radio.Opacity = 0.5;
                    }
                    else
                    {
                        radio.IsEnabled = false;
                        radio.Opacity = 0.2;
                    }
                }
                else if (MaxDateTime != null && date <= ((DateTime)MaxDateTime).Date)
                {
                    radio.IsEnabled = true;
                    radio.Opacity = 0.5;
                }
                else if (MinDateTime != null && date >= ((DateTime)MinDateTime).Date)
                {
                    radio.IsEnabled = true;
                    radio.Opacity = 0.5;
                }
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
                {
                    radio.IsEnabled = true;
                    radio.Opacity = 1;
                }
                else if (MaxDateTime != null && MinDateTime != null)
                {
                    if (date <= ((DateTime)MaxDateTime).Date && date >= ((DateTime)MinDateTime).Date)
                    {
                        radio.IsEnabled = true;
                        radio.Opacity = 1;
                    }
                    else
                    {
                        radio.IsEnabled = false;
                        radio.Opacity = 0.2;
                    }
                }
                else if (MaxDateTime != null && date <= ((DateTime)MaxDateTime).Date)
                {
                    radio.IsEnabled = true;
                    radio.Opacity = 1;
                }
                else if (MinDateTime != null && date >= ((DateTime)MinDateTime).Date)
                {
                    radio.IsEnabled = true;
                    radio.Opacity = 1;
                }
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
                radio.Opacity = 0.5;
                radio.Content = i - firstDay - totalDay + 1;
                radio.Tag = date;

                if (MaxDateTime == null && MinDateTime == null)
                {
                    radio.IsEnabled = true;
                    radio.Opacity = 0.5;
                }
                else if (MaxDateTime != null && MinDateTime != null)
                {
                    if (date <= ((DateTime)MaxDateTime).Date && date >= ((DateTime)MinDateTime).Date)
                    {
                        radio.IsEnabled = true;
                        radio.Opacity = 0.5;
                    }
                    else
                    {
                        radio.IsEnabled = false;
                        radio.Opacity = 0.2;
                    }
                }
                else if (MaxDateTime != null && date <= ((DateTime)MaxDateTime).Date)
                {
                    radio.IsEnabled = true;
                    radio.Opacity = 0.5;
                }
                else if (MinDateTime != null && date >= ((DateTime)MinDateTime).Date)
                {
                    radio.IsEnabled = true;
                    radio.Opacity = 0.5;
                }
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
                ScrollHour.ScrollToVerticalOffset((hour - 2) * radio.Height);
            }
            {
                var radio = StkMinute.Children[minute] as PURadioButton;
                radio.IsChecked = true;
                ScrollMinute.ScrollToVerticalOffset((minute - 2) * radio.Height);
            }
            {
                var radio = StkSecond.Children[second] as PURadioButton;
                radio.IsChecked = true;
                ScrollSecond.ScrollToVerticalOffset((second - 2) * radio.Height);
            }
        }

        private void ResetAndSelectYear(int year)
        {
            if (GrdYearPanel.Children.Count != 15)
                return;

            BtnYearInterval.Content = SelectedDateTime.AddYears(-7).Year + "年 - " + SelectedDateTime.AddYears(7).Year + "年";

            for (int i = -7; i < 8; i++)
            {
                var radio = GrdYearPanel.Children[i + 7] as PURadioButton;
                radio.Content = (year + i) + "年";
                radio.Tag = year + i;
                if (i == 0)
                    radio.IsChecked = true;

                if (MaxDateTime == null && MinDateTime == null)
                {
                    radio.IsEnabled = true;
                    radio.Opacity = 1;
                }

                else if (MaxDateTime != null && MinDateTime != null)
                {
                    if ((year + i) <= ((DateTime)MaxDateTime).Year && (year + i) >= ((DateTime)MinDateTime).Year)
                    {
                        radio.IsEnabled = true;
                        radio.Opacity = 1;
                    }
                    else
                    {
                        radio.IsEnabled = false;
                        radio.Opacity = 0.2;
                    }
                }
                else if (MaxDateTime != null && (year + i) <= ((DateTime)MaxDateTime).Year)
                {
                    radio.IsEnabled = true;
                    radio.Opacity = 1;
                }
                else if (MinDateTime != null && (year + i) >= ((DateTime)MinDateTime).Year)
                {
                    radio.IsEnabled = true;
                    radio.Opacity = 1;
                }
                else
                {
                    radio.IsEnabled = false;
                    radio.Opacity = 0.2;
                }
            }
        }

        private void ResetAndSelectMonth(int month)
        {
            if (GrdMonthPanel.Children.Count != 12)
                return;

            BtnMonthInterval.Content = SelectedDateTime.Year + "年";

            for (int i = 1; i <= 12; i++)
            {
                var radio = GrdMonthPanel.Children[i - 1] as PURadioButton;
                if (i == month)
                    radio.IsChecked = true;

                if (MaxDateTime == null && MinDateTime == null)
                {
                    radio.IsEnabled = true;
                    radio.Opacity = 1;
                }
                else if (MaxDateTime != null && MinDateTime != null)
                {
                    if ((SelectedDateTime.Year != ((DateTime)MaxDateTime).Year || i <= ((DateTime)MaxDateTime).Month) && (SelectedDateTime.Year != ((DateTime)MinDateTime).Year || i >= ((DateTime)MinDateTime).Month))
                    {
                        radio.IsEnabled = true;
                        radio.Opacity = 1;
                    }
                    else
                    {
                        radio.IsEnabled = false;
                        radio.Opacity = 0.2;
                    }
                }
                else if (MaxDateTime != null && (SelectedDateTime.Year != ((DateTime)MaxDateTime).Year || i <= ((DateTime)MaxDateTime).Month))
                {
                    radio.IsEnabled = true;
                    radio.Opacity = 1;
                }
                else if (MinDateTime != null && (SelectedDateTime.Year != ((DateTime)MinDateTime).Year || i >= ((DateTime)MinDateTime).Month))
                {
                    radio.IsEnabled = true;
                    radio.Opacity = 1;
                }
                else
                {
                    radio.IsEnabled = false;
                    radio.Opacity = 0.2;
                }
            }
        }

        private void CheckDateTimeLimit()
        {
            ResetAndSelectYear(SelectedDateTime.Year);
            ResetAndSelectMonth(SelectedDateTime.Month);
            ResetDate(SelectedDateTime.Year, SelectedDateTime.Month);

            if (MaxDateTime != null)
            {
                var max = (DateTime)MaxDateTime;

                if (SelectedDateTime.Date > max.Date)
                {
                    SelectedDateTime = max;
                }

                if (SelectedDateTime.Year >= max.Year)
                {
                    BtnAddYear.Visibility = Visibility.Hidden;
                }
                else
                {
                    BtnAddYear.Visibility = Visibility.Visible;
                }

                if (SelectedDateTime.Year + 7 >= max.Year)
                {
                    BtnYearRight.Visibility = Visibility.Hidden;
                }
                else
                {
                    BtnYearRight.Visibility = Visibility.Visible;
                }

                if (SelectedDateTime.Year > max.Year || (SelectedDateTime.Year == max.Year && SelectedDateTime.Month >= max.Month))
                {
                    BtnAddMonth.Visibility = Visibility.Hidden;
                    BtnMonthRight.Visibility = Visibility.Hidden;
                }
                else
                {
                    BtnAddMonth.Visibility = Visibility.Visible;
                    BtnMonthRight.Visibility = Visibility.Visible;
                }
            }
            else
            {
                BtnAddYear.Visibility = Visibility.Visible;
                BtnAddMonth.Visibility = Visibility.Visible;
                BtnYearRight.Visibility = Visibility.Visible;
                BtnMonthRight.Visibility = Visibility.Visible;
            }

            if (MinDateTime != null)
            {
                var min = (DateTime)MinDateTime;

                if (SelectedDateTime.Date < min.Date)
                {
                    SelectedDateTime = min;
                }

                if (SelectedDateTime.Year <= min.Year)
                {
                    BtnDecYear.Visibility = Visibility.Hidden;
                }
                else
                {
                    BtnDecYear.Visibility = Visibility.Visible;
                }

                if (SelectedDateTime.Year - 7 <= min.Year)
                {
                    BtnYearLeft.Visibility = Visibility.Hidden;
                }
                else
                {
                    BtnYearLeft.Visibility = Visibility.Visible;
                }

                if (SelectedDateTime.Year < min.Year || (SelectedDateTime.Year == min.Year && SelectedDateTime.Month <= min.Month))
                {
                    BtnDecMonth.Visibility = Visibility.Hidden;
                    BtnMonthLeft.Visibility = Visibility.Hidden;
                }
                else
                {
                    BtnDecMonth.Visibility = Visibility.Visible;
                    BtnMonthLeft.Visibility = Visibility.Visible;
                }

            }
            else
            {
                BtnDecYear.Visibility = Visibility.Visible;
                BtnDecMonth.Visibility = Visibility.Visible;
                BtnYearLeft.Visibility = Visibility.Visible;
                BtnMonthLeft.Visibility = Visibility.Visible;
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

        private void YearRadioButton_Click(object sender, RoutedEventArgs e)
        {
            var radio = sender as PURadioButton;
            if (radio.Tag == null)
                return;
            var year = (int)radio.Tag;
            SelectedDateTime = new DateTime(year, SelectedDateTime.Month, SelectedDateTime.Day, SelectedDateTime.Hour, SelectedDateTime.Minute, SelectedDateTime.Second);
            GrdDate.Visibility = Visibility.Visible;
            GrdYear.Visibility = Visibility.Hidden;
        }

        private void MonthRadioButton_Click(object sender, RoutedEventArgs e)
        {
            var radio = sender as PURadioButton;
            if (radio.Tag == null)
                return;
            var month = (int)radio.Tag;
            SelectedDateTime = new DateTime(SelectedDateTime.Year, month, SelectedDateTime.Day, SelectedDateTime.Hour, SelectedDateTime.Minute, SelectedDateTime.Second);
            GrdDate.Visibility = Visibility.Visible;
            GrdMonth.Visibility = Visibility.Hidden;
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

        private void BtnYearLeft_Click(object sender, RoutedEventArgs e)
        {
            if (MinDateTime == null || SelectedDateTime.AddYears(-15) >= MinDateTime)
                SelectedDateTime = SelectedDateTime.AddYears(-15);
            else
                SelectedDateTime = (DateTime)MinDateTime;
        }

        private void BtnYearRight_Click(object sender, RoutedEventArgs e)
        {
            if (MaxDateTime == null || SelectedDateTime.AddYears(15) <= MaxDateTime)
                SelectedDateTime = SelectedDateTime.AddYears(15);
            else
                SelectedDateTime = (DateTime)MaxDateTime;
        }

        private void BtnMonthLeft_Click(object sender, RoutedEventArgs e)
        {
            if (MinDateTime == null || SelectedDateTime.AddYears(-1) >= MinDateTime)
                SelectedDateTime = SelectedDateTime.AddYears(-1);
            else
                SelectedDateTime = (DateTime)MinDateTime;
        }

        private void BtnMonthRight_Click(object sender, RoutedEventArgs e)
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

        

        private void BtnYear_Click(object sender, RoutedEventArgs e)
        {
            ResetAndSelectYear(SelectedDateTime.Year);
            GrdDate.Visibility = Visibility.Hidden;
            GrdYear.Visibility = Visibility.Visible;
        }

        private void BtnMonth_Click(object sender, RoutedEventArgs e)
        {
            ResetAndSelectMonth(SelectedDateTime.Month);
            GrdDate.Visibility = Visibility.Hidden;
            GrdMonth.Visibility = Visibility.Visible;
        }

        private void BtnMonthInterval_Click(object sender, RoutedEventArgs e)
        {
            ResetAndSelectYear(SelectedDateTime.Year);
            GrdMonth.Visibility = Visibility.Hidden;
            GrdYear.Visibility = Visibility.Visible;
        }
    }

}
