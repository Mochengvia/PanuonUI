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
    public class ButtonViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public ButtonViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            ButtonStyle = ButtonStyles.General;
            ClickStyle = ClickStyles.Classic;
            RadiusInteger = 20;
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E3E3E"));
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DDDDDD"));
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#22FFFFFF"));
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

        public ButtonStyles ButtonStyle
        {
            get { return _buttonStyle; }
            set { _buttonStyle = value; NotifyOfPropertyChange(() => ButtonStyle); }
        }
        private ButtonStyles _buttonStyle;

        public ClickStyles ClickStyle
        {
            get { return _clickStyle; }
            set { _clickStyle = value; NotifyOfPropertyChange(() => ClickStyle); }
        }
        private ClickStyles _clickStyle;

        public int RadiusInteger
        {
            get { return _radiusInteger; }
            set { _radiusInteger = value; BorderCornerRadius = new CornerRadius(value); NotifyOfPropertyChange(() => RadiusInteger); }
        }
        private int _radiusInteger;

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

        public Brush CoverBrush
        {
            get { return _coverBrush; }
            set { _coverBrush = value; NotifyOfPropertyChange(() => CoverBrush); }
        }
        private Brush _coverBrush;
        #endregion

        #region Event
        /// <summary>
        /// 阻止滚轮事件传播给DataGrid。
        /// </summary>
        public void HandleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        public void StyleChanged(string content)
        {
            ButtonStyle = (ButtonStyles)Enum.Parse(typeof(ButtonStyles), content);
        }

        public void ClickStyleChanged(string content)
        {
            ClickStyle = (ClickStyles)Enum.Parse(typeof(ClickStyles), content);
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
        #endregion

        #region Function
        private async void Init()
        {
            await Task.Delay(100);
            DependencyPropertyList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "ButtonStyle" ,Type = "ButtonStyles枚举类型" ,Description = "获取或设置按钮的基本样式。【可选值：General、Hollow、Outline、Link】",DefaultValue = "General" },
                new DataSourceModel() { Name = "ClickStyle" ,Type = "ClickStyles枚举类型" ,Description = "获取或设置鼠标点击时按钮的效果。【可选值：Classic（无特殊效果）、Sink（点击时按钮下沉2个px）】",DefaultValue = "Classic" },
                new DataSourceModel() { Name = "BorderCornerRadius" ,Type = "CornerRadius" ,Description = "获取或设置按钮的圆角大小。】",DefaultValue = "0" },
                new DataSourceModel() { Name = "CoverBrush" ,Type = "Brush" ,Description = "获取或设置鼠标悬浮时遮罩层的背景颜色（在Outline和Link样式下为前景色），默认值为#26FFFFFF（在Hollow、Outline和Link样式下为灰黑色）。】",DefaultValue = "#26FFFFFF / #3E3E3E" },
            };
            AnnotationList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "General样式" ,Description = "标准按钮，悬浮/点击时背景色*叠加*一层CoverBrush。" },
                new DataSourceModel() { Name = "Hollow样式" ,Description = "带边框的无背景色按钮，悬浮时背景色变为CoverBrush，前景色变为白色；点击时背景色叠加0.1透明度的白色。" },
                new DataSourceModel() { Name = "Outline样式" ,Description = "带边框的无背景色按钮，悬浮时前景色和边框*叠加*一层CoverBrush；点击时背景色叠加0.1透明度的CoverBrush。" },
                new DataSourceModel() { Name = "Link样式" ,Description = "无边框无背景色的按钮，悬浮时前景色变为CoverBrush；点击时前景色叠加0.2透明度的白色。" },
            };
        }

        private void SetMaskCover(bool toOpen)
        {
            var parent = Parent as ShellWindowViewModel;
            if (toOpen)
                parent.ShowCoverMask();
            else
                parent.CloseCoverMask();
        }

        private void SetAwait(bool toOpen)
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
