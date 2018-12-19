/*==============================================================
*作者：ZEOUN
*时间：2018/11/15 13:12:35
*说明： 样板。
*日志：2018/11/15 13:12:35 创建。
*==============================================================*/
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

namespace Panuon.UIBrowser.ViewModels.Special
{
    public class BubbleViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public BubbleViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            RadiusInteger = 3;
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555"));
            AnglePosition = AnglePositions.Left;
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

        public int RadiusInteger
        {
            get { return _radiusInteger; }
            set { _radiusInteger = value; BorderCornerRadius = new CornerRadius(value); NotifyOfPropertyChange(() => RadiusInteger); }
        }
        private int _radiusInteger;

        public AnglePositions AnglePosition
        {
            get { return _anglePosition; }
            set { _anglePosition = value; NotifyOfPropertyChange(() => AnglePosition); }
        }
        private AnglePositions _anglePosition;

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

        #endregion

        #region Event

        public void AnglePositionChanged(string content)
        {
            AnglePosition = (AnglePositions)Enum.Parse(typeof(AnglePositions), content);
        }


        public void BackgroundChanged(string content)
        {
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void CoverBrushChanged(string content)
        {
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
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
                new DataSourceModel() { Name = "AnglePosition" ,Type = "AnglePositions枚举" ,Description = "获取或设置气泡的尖角位置。【可选值：Left、BottomLeft、BottomCenter、BottomRight、Right】",DefaultValue = "Left" },
                new DataSourceModel() { Name = "BorderCornerRadius" ,Type = "CornerRadius" ,Description = "获取或设置气泡的圆角大小。",DefaultValue = "0" },
                new DataSourceModel() { Name = "CoverBrush" ,Type = "Brush" ,Description = "获取或设置鼠标悬浮时遮罩层的背景颜色。",DefaultValue = "#555555" },
            };
            AnnotationList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "哪些属性会影响气泡的颜色？" ,Description = "气泡的边框受BorderBrush和BorderThickness影响，背景色受Background影响，前景色受Foreground影响。" },
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
