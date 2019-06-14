using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PUDateTimeSelector : Control
    {
        #region Constructor
        static PUDateTimeSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUDateTimeSelector), new FrameworkPropertyMetadata(typeof(PUDateTimeSelector)));
        }

        public PUDateTimeSelector()
        {
            Loaded += delegate
            {
                UpdateText();
            };
        }
        #endregion

        #region Property
        public DatePickerModes DatePickerMode
        {
            get { return (DatePickerModes)GetValue(DatePickerModeProperty); }
            set { SetValue(DatePickerModeProperty, value); }
        }

        public static readonly DependencyProperty DatePickerModeProperty =
            DependencyProperty.Register("DatePickerMode", typeof(DatePickerModes), typeof(PUDateTimeSelector), new PropertyMetadata(DatePickerModes.DateTime));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(PUDateTimeSelector));


        public Color ShadowColor
        {
            get { return (Color)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }

        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.Register("ShadowColor", typeof(Color), typeof(PUDateTimeSelector));

        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }

        public static readonly DependencyProperty CoverBrushProperty =
            DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUDateTimeSelector));


        public DateTime SelectedDateTime
        {
            get { return (DateTime)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateTimeProperty =
            DependencyProperty.Register("SelectedDateTime", typeof(DateTime), typeof(PUDateTimeSelector), new PropertyMetadata(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0), OnSelectedDateTimeChanged));

        private static void OnSelectedDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var picker = d as PUDateTimeSelector;
            picker.UpdateText();
        }

        public DateTime? MaxDate
        {
            get { return (DateTime?)GetValue(MaxDateProperty); }
            set { SetValue(MaxDateProperty, value); }
        }

        public static readonly DependencyProperty MaxDateProperty =
            DependencyProperty.Register("MaxDate", typeof(DateTime?), typeof(PUDateTimeSelector));


        public DateTime? MinDate
        {
            get { return (DateTime?)GetValue(MinDateProperty); }
            set { SetValue(MinDateProperty, value); }
        }

        public static readonly DependencyProperty MinDateProperty =
            DependencyProperty.Register("MinDate", typeof(DateTime?), typeof(PUDateTimeSelector));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(PUDateTimeSelector));

        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), typeof(PUDateTimeSelector));
        #endregion

        #region Function
        private void UpdateText()
        {
            switch (DatePickerMode)
            {
                case DatePickerModes.DateTime:
                    Text = SelectedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case DatePickerModes.DateOnly:
                    Text = SelectedDateTime.ToString("yyyy-MM-dd");
                    break;
                case DatePickerModes.TimeOnly:
                    Text = SelectedDateTime.ToString("HH:mm:ss");
                    break;
            }
        }
        #endregion
    }
}
