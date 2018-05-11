using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Panuon.UI
{
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
            ContextMenu = null;

            Text = "";
            if (Password != null && Password != "")
                for (int i = 0; i < Password.Length; i++)
                    Text += PasswordChar;

            PreviewTextInput += PUPasswordBox_TextInput;
            PreviewKeyDown += PUPasswordBox_PreviewKeyDown;
        }

        private void PUPasswordBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            if (Password.Length >= MaxLength && SelectionLength == 0)
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

        private void PUPasswordBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //禁止任何与Control键有关的事(除了全选)
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.A))
            {
                e.Handled = true;
                return;
            }

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

        #region Event
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

        /// <summary>
        /// 密码属性
        /// </summary>
        [Bindable(true, BindingDirection.TwoWay)]
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set
            {
                var val = Password;
                SetValue(PasswordProperty, value);

                Text = "";
                if (Password != null && Password != "")
                    for (int i = 0; i < Password.Length; i++)
                        Text += PasswordChar;

                RoutedPropertyChangedEventArgs<string> arg = new RoutedPropertyChangedEventArgs<string>(val, Password, PasswordChangedEvent);
                RaiseEvent(arg);

            }
        }
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(PUPasswordBox), new PropertyMetadata(""));

        /// <summary>
        /// 密码掩饰字符，默认值为●。
        /// </summary>
        public char PasswordChar
        {
            get { return (char)GetValue(PasswordCharProperty); }
            set { SetValue(PasswordCharProperty, value); }
        }
        public static readonly DependencyProperty PasswordCharProperty = DependencyProperty.Register("PasswordChar", typeof(char), typeof(PUPasswordBox), new PropertyMetadata('●'));
        /// <summary>
        /// 密码框样式，默认值为General。
        /// </summary>
        public PasswordBoxStyles PasswordBoxStyle
        {
            get { return (PasswordBoxStyles)GetValue(PasswordBoxStyleProperty); }
            set { SetValue(PasswordBoxStyleProperty, value); }
        }
        public static readonly DependencyProperty PasswordBoxStyleProperty = DependencyProperty.Register("PasswordBoxStyle", typeof(PasswordBoxStyles), typeof(PUPasswordBox), new PropertyMetadata(PasswordBoxStyles.General));

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
            get { return (Color)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }
        public static readonly DependencyProperty CoverBrushProperty = DependencyProperty.Register("CoverBrush", typeof(Color), typeof(PUPasswordBox), new PropertyMetadata((Color)ColorConverter.ConvertFromString("#888888")));

        /// <summary>
        ///  水印内容，默认值为空。
        /// </summary>
        public string WaterMark
        {
            get { return (string)GetValue(WaterMarkProperty); }
            set { SetValue(WaterMarkProperty, value); }
        }
        public static readonly DependencyProperty WaterMarkProperty = DependencyProperty.Register("WaterMark", typeof(string), typeof(PUPasswordBox), new PropertyMetadata(""));

        /// <summary>
        /// 放置在密码框前的图标。
        /// <para>仅当密码框样式为IconGroup时有效。</para>
        /// </summary>
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(string), typeof(PUPasswordBox), new PropertyMetadata(""));

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

        #endregion

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
    }
}
