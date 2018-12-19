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
    public class ContextMenuViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public ContextMenuViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            TextBoxStyle = TextBoxStyles.General;
            RadiusInteger = 3;
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#33AAAAAA"));
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

        public TextBoxStyles TextBoxStyle
        {
            get { return _textboxStyle; }
            set { _textboxStyle = value; NotifyOfPropertyChange(() => TextBoxStyle); }
        }
        private TextBoxStyles _textboxStyle;

        public int RadiusInteger
        {
            get { return _radiusInteger; }
            set { _radiusInteger = value; BorderCornerRadius = new CornerRadius(value); NotifyOfPropertyChange(() => RadiusInteger); }
        }
        private int _radiusInteger;

        public bool ClearButtonIsChecked
        {
            get { return _clearButtonIsChecked; }
            set { _clearButtonIsChecked = value; NotifyOfPropertyChange(() => ClearButtonIsChecked); }
        }
        private bool _clearButtonIsChecked;

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

        public Brush CoverBrush
        {
            get { return _coverBrush; }
            set { _coverBrush = value; NotifyOfPropertyChange(() => CoverBrush); }
        }
        private Brush _coverBrush;

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
        public void TextBoxStyleChanged(string content)
        {
            TextBoxStyle = (TextBoxStyles)Enum.Parse(typeof(TextBoxStyles), content);
        }

        public void BackgroundChanged(string content)
        {
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void ForegroundChanged(string content)
        {
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void CoverBrushChanged(string content)
        {
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
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
                new DataSourceModel() { Name = "TextBoxStyle" ,Type = "TextBoxStyles枚举" ,Description = "获取或设置文本框的基本样式。【可选值：General、IconGroup】",DefaultValue = "General" },
                new DataSourceModel() { Name = "BorderCornerRadius" ,Type = "CornerRadius" ,Description = "获取或设置文本框的圆角大小。",DefaultValue = "0" },
                new DataSourceModel() { Name = "ShadowColor" ,Type = "Color" ,Description = "获取或设置输入框获得焦点时阴影的颜色。",DefaultValue = "#66888888" },
                new DataSourceModel() { Name = "Watermark" ,Type = "String" ,Description = "获取或设置水印内容。",DefaultValue = "Null" },
                new DataSourceModel() { Name = "Icon" ,Type = "Object" ,Description = "获取或设置输入框获得焦点时阴影的颜色，仅在IconGroup样式下生效。",DefaultValue = "#66888888" },
                new DataSourceModel() { Name = "IconWidth" ,Type = "Color" ,Description = "获取或设置图标的宽度。",DefaultValue = "30" },
                new DataSourceModel() { Name = "IsClearButtonShow" ,Type = "Boolean" ,Description = "获取或设置当鼠标悬浮时是否显示清除按钮。",DefaultValue = "False" },
            };
            AnnotationList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "关于ShadowColor" ,Description = "当文本框获得焦点时，文本框会获得一个0.4透明度、5px大小的阴影。DropShadow效果的Color不支持8位HEX（仅支持6位HEX），因而透明度被限制在0.4。" },
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
