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
    public class RadioButtonViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public RadioButtonViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            RadioButtonStyle = RadioButtonStyles.General;
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

        public RadioButtonStyles RadioButtonStyle
        {
            get { return _RadioButtonStyle; }
            set { _RadioButtonStyle = value; NotifyOfPropertyChange(() => RadioButtonStyle); }
        }
        private RadioButtonStyles _RadioButtonStyle;

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
            RadioButtonStyle = (RadioButtonStyles)Enum.Parse(typeof(RadioButtonStyles), content);
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
                new DataSourceModel() { Name = "RadioButtonStyle" ,Type = "Brush" ,Description = "获取或设置选择框的基本样式。【可选项：General、Classic、Switch、Branch、Button】",DefaultValue = "General" },
                new DataSourceModel() { Name = "CoverBrush" ,Type = "Brush" ,Description = "获取或设置鼠标悬浮时子项的背景颜色。",DefaultValue = "#EEEEEE" },
                new DataSourceModel() { Name = "BorderCornerRadius" ,Type = "CornerRadius" ,Description = "获取或设置选择框的圆角大小。",DefaultValue = "0" },
                new DataSourceModel() { Name = "InnerWidth" ,Type = "Double" ,Description = "获取或设置选择框的宽度。",DefaultValue = "20 / 30 (Switch)" },
                new DataSourceModel() { Name = "InnerHeight" ,Type = "Double" ,Description = "获取或设置选择框的高度。",DefaultValue = "20" },
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
