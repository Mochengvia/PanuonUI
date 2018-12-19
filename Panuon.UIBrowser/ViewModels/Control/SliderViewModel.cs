using Caliburn.Micro;
using Panuon.UI;
using Panuon.UIBrowser.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Panuon.UIBrowser.ViewModels.Control
{
    public class SliderViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;

        #endregion

        #region Constructor
        public SliderViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            Maximuim = 10;
            Minimuim = 0;
            Value = 5;
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D3D3D3"));
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#696969"));
            ShadowColor = (Color)ColorConverter.ConvertFromString("#888888");
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

        public Color ShadowColor
        {
            get { return _shadowColor; }
            set { _shadowColor = value; NotifyOfPropertyChange(() => ShadowColor); }
        }
        private Color _shadowColor;

        public int Maximuim
        {
            get { return _maximuim; }
            set { _maximuim = value; NotifyOfPropertyChange(() => Maximuim); }
        }
        private int _maximuim;

        public int Minimuim
        {
            get { return _minimuim; }
            set { _minimuim = value; NotifyOfPropertyChange(() => Minimuim); }
        }
        private int _minimuim;

        public int Value
        {
            get { return _value; }
            set { _value = value; NotifyOfPropertyChange(() => Value); }
        }
        private int _value;
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

   
        public void CoverBrushChanged(string content)
        {
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void ForegroundChanged(string content)
        {
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void ShadowColorChanged(string content)
        {
            ShadowColor = (Color)ColorConverter.ConvertFromString(content);
        }

        #endregion

        #region Function
        private async void Init()
        {
            await Task.Delay(100);

            DependencyPropertyList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "CoverBrush" ,Type = "Brush" ,Description = "获取或设置滑块覆盖区域（左侧）的颜色。",DefaultValue = "#696969" },
                new DataSourceModel() { Name = "Maximuim" ,Type = "Int" ,Description = "获取或设置滑块的最大值。",DefaultValue = "100" },
                new DataSourceModel() { Name = "Minimuim" ,Type = "Int" ,Description = "获取或设置滑块的最小值。",DefaultValue = "0" },
                new DataSourceModel() { Name = "Value" ,Type = "Int" ,Description = "获取或设置滑块当前选择的值。",DefaultValue = "0" },
            };

            AnnotationList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "有关滑块的颜色" ,Description = "滑块右侧的颜色受CoverBrush影响，左侧的颜色受Foreground影响。滑块的边框（白色）不能更改。" },
                new DataSourceModel() { Name = "若要实现步长功能" ,Description = "步长功能可以由Value乘以某个值来展示给用户（这也可以应用于小数）。" },
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
