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
    public class ProgressBarViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public ProgressBarViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            ProgressBarStyle = ProgressBarStyles.Ring;
            RadiusInteger = 0;
            Height = 100;
            Width = 100;
            BorderThicknessInterger = 3;
            Percent = 0.2;
            ShowPercentIsChecked = true;
            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D3D3D3"));
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E3E3E"));
            Background = new SolidColorBrush(Colors.Transparent);
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AAAAAA"));
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

        public ObservableCollection<DataSourceModel> EventList
        {
            get { return _eventList; }
            set { _eventList = value; NotifyOfPropertyChange(() => EventList); }
        }
        private ObservableCollection<DataSourceModel> _eventList;

        public ProgressBarStyles ProgressBarStyle
        {
            get { return _ProgressBarStyle; }
            set { _ProgressBarStyle = value; NotifyOfPropertyChange(() => ProgressBarStyle); }
        }
        private ProgressBarStyles _ProgressBarStyle;

        public int RadiusInteger
        {
            get { return _radiusInteger; }
            set { _radiusInteger = value; BorderCornerRadius = new CornerRadius(value); NotifyOfPropertyChange(() => RadiusInteger); }
        }
        private int _radiusInteger;

        public double Width
        {
            get { return _width; }
            set { _width = value; NotifyOfPropertyChange(() => Width); }
        }
        private double _width;

        public double Height
        {
            get { return _Height; }
            set { _Height = value; NotifyOfPropertyChange(() => Height); }
        }
        private double _Height;

        public double Percent
        {
            get { return _percent; }
            set { _percent = value; NotifyOfPropertyChange(() => Percent); }
        }
        private double _percent;

        public int BorderThicknessInterger
        {
            get { return _borderThicknessInterger; }
            set { _borderThicknessInterger = value; BorderThickness = new Thickness(value); NotifyOfPropertyChange(() => BorderThicknessInterger); }
        }
        private int _borderThicknessInterger;

        public Thickness BorderThickness
        {
            get { return _borderThickness; }
            set { _borderThickness = value; NotifyOfPropertyChange(() => BorderThickness); }
        }
        private Thickness _borderThickness;


        public CornerRadius BorderCornerRadius
        {
            get { return _borderCornerRadius; }
            set { _borderCornerRadius = value; NotifyOfPropertyChange(() => BorderCornerRadius); }
        }
        private CornerRadius _borderCornerRadius;

        public Brush BorderBrush
        {
            get { return _borderBrush; }
            set { _borderBrush = value; NotifyOfPropertyChange(() => BorderBrush); }
        }
        private Brush _borderBrush;

        public Brush CoverBrush
        {
            get { return _coverBrush; }
            set { _coverBrush = value; NotifyOfPropertyChange(() => CoverBrush); }
        }
        private Brush _coverBrush;

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

        public bool ShowPercentIsChecked
        {
            get { return _showPercentIsChecked; }
            set { _showPercentIsChecked = value; NotifyOfPropertyChange(() => ShowPercentIsChecked); }
        }
        private bool _showPercentIsChecked;

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
            ProgressBarStyle = (ProgressBarStyles)Enum.Parse(typeof(ProgressBarStyles), content);
        }

        public void CoverBrushChanged(string content)
        {
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void BorderBrushChanged(string content)
        {
            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void BackgroundChanged(string content)
        {
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void ForegroundChanged(string content)
        {
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }
        #endregion

        #region Function
        private async void Init()
        {
            await Task.Delay(100);
            DependencyPropertyList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "Percent" ,Type = "Double" ,Description = "获取或设置当前进度条的百分比，从0~1的值。",DefaultValue = "0" },
                new DataSourceModel() { Name = "ProgressBarStyle" ,Type = "ProgressBarStyles枚举" ,Description = "获取或设置进度条的基本样式。【可选项：General、Ring】",DefaultValue = "General" },
                new DataSourceModel() { Name = "CoverBrush" ,Type = "Brush" ,Description = "获取或设置进度条的填充颜色。",DefaultValue = "#3E3E3E" },
                new DataSourceModel() { Name = "BorderCornerRadius" ,Type = "CornerRadius" ,Description = "获取或设置进度条的圆角大小。",DefaultValue = "0" },
                new DataSourceModel() { Name = "AnimationDuration" ,Type = "TimeSpan" ,Description = "获取或设置进度条的填充动画持续时间。",DefaultValue = "0.4秒" },
                new DataSourceModel() { Name = "IsPercentShow" ,Type = "Boolean" ,Description = "获取或设置是否显示百分比。",DefaultValue = "False" },
                new DataSourceModel() { Name = "ProgressDirection" ,Type = "ProgressDirections枚举" ,Description = "获取或设置进度条填充方向。【可选值：LeftToRight、RightToLeft、BottomToTop、TopToBottom】",DefaultValue = "LeftToRight" },
            };
            EventList = new ObservableCollection<Models.DataSourceModel>()
            {
                new DataSourceModel() { Name = "PercentChanged" ,Description = "当Percent属性发生变化时，触发此事件。事件参数：Percent属性值。" },
            };
            AnnotationList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "继承自Control" ,Description = "该控件继承自Control控件（而非ProgressBar），无法使用Maximuim、Minimium、Value等属性。" },
                new DataSourceModel() { Name = "支持纵向进度条" ,Description = "ProgressDirection支持从左到右、从右到左、从上到下、从下到上的填充方式。标准的横向进度条应是LeftToRight，纵向进度条应是BottomToTop。" },
                new DataSourceModel() { Name = "关于Ring样式" ,Description = "环形进度条的圆环初始色取决于BorderBrush，进度填充色取决于CoverBrush，环的粗细取决于BorderThickness。如果控件的长宽大小不等，会导致圆环异常扭曲。" },
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
