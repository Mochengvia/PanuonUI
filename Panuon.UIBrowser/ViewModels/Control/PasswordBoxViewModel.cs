using Caliburn.Micro;
using Panuon.UI;
using Panuon.UIBrowser.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Panuon.UIBrowser.ViewModels.Control
{
    public class PasswordBoxViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public PasswordBoxViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            PasswordBoxStyle = PasswordBoxStyles.General;
            RadiusInteger = 3;
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E3E3E"));
            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D3D3D3"));
            ShadowColor = (Color)ColorConverter.ConvertFromString("#22888888");
            Init();
        }
        #endregion

        #region Bindings
        public ObservableCollection<DataSourceModel> DependencyPropertyList
        {
            get { return _dependencyPropertyList; }
            set { _dependencyPropertyList = value; NotifyOfPropertyChange(() => DependencyPropertyList); }
        }
        private ObservableCollection<DataSourceModel> _dependencyPropertyList;

        public ObservableCollection<DataSourceModel> AnnotationList
        {
            get { return _annotationList; }
            set { _annotationList = value; NotifyOfPropertyChange(() => AnnotationList); }
        }
        private ObservableCollection<DataSourceModel> _annotationList;

        public PasswordBoxStyles PasswordBoxStyle
        {
            get { return _PasswordBoxStyle; }
            set { _PasswordBoxStyle = value; NotifyOfPropertyChange(() => PasswordBoxStyle); }
        }
        private PasswordBoxStyles _PasswordBoxStyle;

        public int RadiusInteger
        {
            get { return _radiusInteger; }
            set { _radiusInteger = value; BorderCornerRadius = new CornerRadius(value); NotifyOfPropertyChange(() => RadiusInteger); }
        }
        private int _radiusInteger;

        public bool ShowPwdButtonIsChecked
        {
            get { return _showPwdButtonIsChecked; }
            set { _showPwdButtonIsChecked = value; NotifyOfPropertyChange(() => ShowPwdButtonIsChecked); }
        }
        private bool _showPwdButtonIsChecked;

        public CornerRadius BorderCornerRadius
        {
            get { return _borderCornerRadius; }
            set { _borderCornerRadius = value; NotifyOfPropertyChange(() => BorderCornerRadius); }
        }
        private CornerRadius _borderCornerRadius;

        public Brush Background
        {
            get { return _background; }
            set { _background = value; NotifyOfPropertyChange(() => Background); }
        }
        private Brush _background;

        public Brush Foreground
        {
            get { return _foreground; }
            set { _foreground = value; NotifyOfPropertyChange(() => Foreground); }
        }
        private Brush _foreground;

        public Brush BorderBrush
        {
            get { return _borderBrush; }
            set { _borderBrush = value; NotifyOfPropertyChange(() => BorderBrush); }
        }
        private Brush _borderBrush;

        public Color ShadowColor
        {
            get { return _shadowColor; }
            set { _shadowColor = value; NotifyOfPropertyChange(() => ShadowColor); }
        }
        private Color _shadowColor;

        #endregion

        #region Event
        public void PasswordBoxStyleChanged(string content)
        {
            PasswordBoxStyle = (PasswordBoxStyles)Enum.Parse(typeof(PasswordBoxStyles), content);
        }

        public void BackgroundChanged(string content)
        {
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void ForegroundChanged(string content)
        {
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void ShadowColorChanged(string content)
        {
            ShadowColor = (Color)ColorConverter.ConvertFromString(content);
        }

        public void BorderBrushChanged(string content)
        {
            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));

        }
        /// <summary>
        /// 阻止滚轮事件传播给DataGrid。
        /// </summary>
        public void HandleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        #endregion

        #region Function
        public async void Init()
        {
            await Task.Delay(100);
            DependencyPropertyList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "PasswordBoxStyle" ,Type = "PasswordBoxStyles枚举" ,Description = "获取或设置密码框的基本样式。【可选值：General、IconGroup】",DefaultValue = "General" },
                new DataSourceModel() { Name = "BorderCornerRadius" ,Type = "CornerRadius" ,Description = "获取或设置密码框的圆角大小。",DefaultValue = "0" },
                new DataSourceModel() { Name = "ShadowColor" ,Type = "Color" ,Description = "获取或设置密码框获得焦点时阴影的颜色。",DefaultValue = "#66888888" },
                new DataSourceModel() { Name = "Watermark" ,Type = "String" ,Description = "获取或设置水印内容。",DefaultValue = "Null" },
                new DataSourceModel() { Name = "Icon" ,Type = "Object" ,Description = "获取或设置密码框获得焦点时阴影的颜色，仅在IconGroup样式下生效。",DefaultValue = "#66888888" },
                new DataSourceModel() { Name = "IconWidth" ,Type = "Color" ,Description = "获取或设置图标的宽度。",DefaultValue = "30" },
                new DataSourceModel() { Name = "IsShowPwdButtonShow" ,Type = "Boolean" ,Description = "获取或设置当鼠标悬浮时是否显示 显示密码 按钮。",DefaultValue = "False" },
            };
            AnnotationList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "关于ShadowColor" ,Description = "当密码框获得焦点时，密码框会获得一个0.4透明度、5px大小的阴影。DropShadow效果的Color不支持8位HEX（仅支持6位HEX），因而透明度被限制在0.4。" },
                new DataSourceModel() { Name = "该控件继承自TextBox" ,Description = "原生的PasswordBox提供了安全加密算法，使得其他程序无法通过读取计算机内存来获得用户输入的密码。由于PasswordBox是一个密封类，PUPasswordBox事实上是一个继承自TextBox的控件。该控件不能像原生控件那样提供内存安全保护，因而不建议在高风险环境中使用。" },
                new DataSourceModel() { Name = "已隐藏Text属性的Set方法" ,Description = "控件提供了Password和PasswordChar属性，并对外隐藏了Text属性的Set方法。" },
            };
        }

        public void SetMaskCover(bool toOpen)
        {
            var parent = Parent as ShellWindowViewModel;
            if (toOpen)
                parent.ShowCoverMask();
            else
                parent.CloseCoverMask();
        }

        public void SetAwait(bool toOpen)
        {
            var parent = Parent as ShellWindowViewModel;
            if (toOpen)
                parent.ShowAwait();
            else
                parent.CloseAwait();
        }
        #endregion
    }
}
