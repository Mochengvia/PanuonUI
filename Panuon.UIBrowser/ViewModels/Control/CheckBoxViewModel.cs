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
    public class CheckBoxViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public CheckBoxViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            CheckBoxStyle = CheckBoxStyles.General;
            RadiusInteger = 0;
            InnerHeight = 20;
            InnerWidth = 20;
            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E3E3E"));
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E3E3E"));
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

        public CheckBoxStyles CheckBoxStyle
        {
            get { return _checkBoxStyle; }
            set { _checkBoxStyle = value; NotifyOfPropertyChange(() => CheckBoxStyle); }
        }
        private CheckBoxStyles _checkBoxStyle;

        public int RadiusInteger
        {
            get { return _radiusInteger; }
            set { _radiusInteger = value; BorderCornerRadius = new CornerRadius(value); NotifyOfPropertyChange(() => RadiusInteger); }
        }
        private int _radiusInteger;

        public double InnerWidth
        {
            get { return _innerWidth; }
            set { _innerWidth = value; NotifyOfPropertyChange(() => InnerWidth); }
        }
        private double _innerWidth;

        public double InnerHeight
        {
            get { return _innerHeight; }
            set { _innerHeight = value; NotifyOfPropertyChange(() => InnerHeight); }
        }
        private double _innerHeight;

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
            CheckBoxStyle = (CheckBoxStyles)Enum.Parse(typeof(CheckBoxStyles), content);
            if(CheckBoxStyle == CheckBoxStyles.Switch)
            {
                if (InnerHeight == InnerWidth)
                    InnerWidth = InnerHeight * 1.5;
            }
        }

        public void CoverBrushChanged(string content)
        {
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void BorderBrushChanged(string content)
        {
            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        #endregion

        #region Function
        private async void Init()
        {
            await Task.Delay(100);
            DependencyPropertyList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "CheckBoxStyle" ,Type = "Brush" ,Description = "获取或设置选择框的基本样式。【可选项：General、Classic、Switch、Branch、Button】",DefaultValue = "General" },
                new DataSourceModel() { Name = "CoverBrush" ,Type = "Brush" ,Description = "获取或设置鼠标悬浮时子项的背景颜色。",DefaultValue = "#EEEEEE" },
                new DataSourceModel() { Name = "BorderCornerRadius" ,Type = "CornerRadius" ,Description = "获取或设置选择框的圆角大小。",DefaultValue = "0" },
                new DataSourceModel() { Name = "InnerWidth" ,Type = "Double" ,Description = "获取或设置选择框的宽度。",DefaultValue = "20 / 30 (Switch)" },
                new DataSourceModel() { Name = "InnerHeight" ,Type = "Double" ,Description = "获取或设置选择框的高度。",DefaultValue = "20" },
            };
            AnnotationList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "General样式" ,Description = "一个勾号为CoverBrush、内框没有背景色的选择框。" },
                new DataSourceModel() { Name = "Classic样式" ,Description = "一个勾号为白色，选中时内框背景色为CoverBrush的选择框。" },
                new DataSourceModel() { Name = "Switch样式" ,Description = "一个开关样式的，选中时内框背景色为CoverBrush的选择框。BorderCornerRadius属性不会对它造成影响。" },
                new DataSourceModel() { Name = "Branch样式" ,Description = "一个只有单边框的，初始背景色为BorderBrush，选中时背景色为CoverBrush的选择框（如要调整单边框的边长或位置，可以调整BorderThickness，默认值为5,0,0,0）。" },
                new DataSourceModel() { Name = "Button样式" ,Description = "一个按钮样式的，选中时背景色为CoverBrush，前景色变为白色的选择框。" },
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
