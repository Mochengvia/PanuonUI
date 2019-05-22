using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace Panuon.UI
{
     [ContentProperty(nameof(Password))]
    public class PUPasswordBox : TextBox
    {
        /*严重警告：
        *PasswordBox是一个密封类，因此控件无法直接从PasswordBox派生。
        *该控件继承自TextBox，因此无法保证在装有恶意软件的计算机上的密码安全（恶意软件可以通过内存读取密码）。
        *如果对密码安全有较高的需求，切勿使用此控件。*/

        static PUPasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUPasswordBox), new FrameworkPropertyMetadata(typeof(PUPasswordBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            AcceptsReturn = false;
            PUButton btnShowPwd;
            if(PasswordBoxStyle == PasswordBoxStyles.General)
                 btnShowPwd = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(this, 0), 1), 0), 2) as PUButton;
            else
                btnShowPwd = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(this, 0), 1), 0), 1), 2) as PUButton;


            btnShowPwd.PreviewMouseLeftButtonDown += BtnShowPwd_MouseLeftButtonDown;
            btnShowPwd.PreviewMouseLeftButtonUp += BtnShowPwd_MouseLeftButtonUp;

            ContextMenu = null;

            Text = "";
            if (Password != null && Password != "")
                for (int i = 0; i < Password.Length; i++)
                    Text += PasswordChar;

            PreviewTextInput += PUPasswordBox_TextInput;
            PreviewKeyDown += PUPasswordBox_PreviewKeyDown;
        }

        private void BtnShowPwd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Text = "";
            for (int i = 0; i < Password.Length; i++)
                Text += PasswordChar;
        }

        private void BtnShowPwd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Text = Password;
        }

        #region Sys
        private void PUPasswordBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            Password = Password ?? "";
            if ((MaxLength != 0 && Password.Length >= MaxLength) && SelectionLength == 0)
            {
                e.Handled = true;
                return;
            }

            if (e.Text == "\n" || e.Text == "\r" || e.Text == "\t")
                return;

            var currentCursor = SelectionStart;

            if (SelectionLength != 0)
            {
                var length = SelectionLength;
                Password = Password.Remove(currentCursor, length);
            }

            Password = Password.Insert(currentCursor, e.Text);
            Select(currentCursor + 1, 0);
            e.Handled = true;
        }

        private void PUPasswordBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //禁止任何与Control键有关的事(除了全选)
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.A)))
            {
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Enter)
                return;

            var currentCursor = SelectionStart;

            switch (e.Key)
            {
                case Key.Back:
                    if (Text.Length > 0 && currentCursor > 0 && SelectionLength == 0)
                    {

                        Password = Password.Remove(currentCursor - 1, 1);
                        if (currentCursor > 0)
                            Select(currentCursor - 1, 0);
                    }
                    else if (SelectionLength > 0)
                    {
                        var length = SelectionLength;

                        Password = Password.Remove(currentCursor, length);
                        Select(currentCursor, 0);
                    }
                    //批量选择情况下
                    break;
                case Key.Delete:
                    if (currentCursor < Text.Length)
                    {

                        Password = Password.Remove(currentCursor, 1);
                        Select(currentCursor, 0);
                    }
                    break;
                case Key.Space:
                    if (SelectionLength != 0)
                    {

                        Password = Password.Remove(currentCursor, SelectionLength);
                    }

                    Password = Password.Insert(currentCursor, " ");
                    Select(currentCursor + 1, 0);
                    break;
                default:
                    return;
            }
            e.Handled = true;
        }
       
        #endregion

        #region RoutedEvent
        /// <summary>
        /// 密码改变事件。
        /// </summary>
        public static readonly RoutedEvent PasswordChangedEvent = EventManager.RegisterRoutedEvent("PasswordChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<string>), typeof(PUPasswordBox));
        public event RoutedPropertyChangedEventHandler<string> PasswordChanged
        {
            add { AddHandler(PasswordChangedEvent, value); }
            remove { RemoveHandler(PasswordChangedEvent, value); }
        }
        #endregion

        #region Property
        [Obsolete("不能对密码框的Text属性赋值。")]
        public new string Text
        {
            get { return base.Text; }
            private set { base.Text = value; }
        }

        /// <summary>
        /// 密码属性
        /// </summary>
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly DependencyProperty PasswordProperty = 
            DependencyProperty.Register("Password", typeof(string), typeof(PUPasswordBox), new PropertyMetadata("", OnPasswordChanged));

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pb = d as PUPasswordBox;
            var val = (string)e.NewValue;
            pb.Text = "";
            if (pb.Password != null && pb.Password != "")
                for (int i = 0; i < pb.Password.Length; i++)
                    pb.Text += pb.PasswordChar;
            RoutedPropertyChangedEventArgs<string> arg = new RoutedPropertyChangedEventArgs<string>(val, pb.Password, PasswordChangedEvent);
            pb.RaiseEvent(arg);
        }

        /// <summary>
        /// 密码掩饰字符，默认值为●。
        /// </summary>
        public char PasswordChar
        {
            get { return (char)GetValue(PasswordCharProperty); }
            set { SetValue(PasswordCharProperty, value); }
        }
        public static readonly DependencyProperty PasswordCharProperty = 
            DependencyProperty.Register("PasswordChar", typeof(char), typeof(PUPasswordBox), new PropertyMetadata('●'));
        /// <summary>
        /// 密码框样式，默认值为General。
        /// </summary>
        public PasswordBoxStyles PasswordBoxStyle
        {
            get { return (PasswordBoxStyles)GetValue(PasswordBoxStyleProperty); }
            set { SetValue(PasswordBoxStyleProperty, value); }
        }
        public static readonly DependencyProperty PasswordBoxStyleProperty = 
            DependencyProperty.Register("PasswordBoxStyle", typeof(PasswordBoxStyles), typeof(PUPasswordBox), new PropertyMetadata(PasswordBoxStyles.General));

        /// <summary>
        /// 圆角大小，默认值为0。
        /// </summary>
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(PUPasswordBox), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        ///  密码框激活时阴影的颜色，默认值为#888888。
        /// </summary>
        public Color ShadowColor
        {
            get { return (Color)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }
        public static readonly DependencyProperty ShadowColorProperty = DependencyProperty.Register("ShadowColor", typeof(Color), typeof(PUPasswordBox), new PropertyMetadata((Color)ColorConverter.ConvertFromString("#888888")));

        /// <summary>
        ///  水印内容，默认值为空。
        /// </summary>
        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(PUPasswordBox));

        /// <summary>
        /// 放置在密码框前的图标。
        /// <para>仅当密码框样式为IconGroup时有效。</para>
        /// </summary>
        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(PUPasswordBox));

        /// <summary>
        /// 图标的宽度，默认值为30。
        /// <para>仅当密码框样式为IconGroup时有效。</para>
        /// </summary>
        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }
        public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register("IconWidth", typeof(double), typeof(PUPasswordBox), new PropertyMetadata((double)30));


        /// <summary>
        /// 获取或设置当鼠标悬浮时是否显示 显示密码 按钮。默认值为False。
        /// </summary>
        public bool IsShowPwdButtonShow
        {
            get { return (bool)GetValue(IsShowPwdButtonShowProperty); }
            set { SetValue(IsShowPwdButtonShowProperty, value); }
        }

        public static readonly DependencyProperty IsShowPwdButtonShowProperty =
            DependencyProperty.Register("IsShowPwdButtonShow", typeof(bool), typeof(PUPasswordBox));


        #endregion

    }
}
