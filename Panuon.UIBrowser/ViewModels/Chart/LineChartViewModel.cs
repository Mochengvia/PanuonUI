using Caliburn.Micro;
using Panuon.UI.Charts;
using Panuon.UIBrowser.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Panuon.UIBrowser.ViewModels.Chart
{
    public class LineChartViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public LineChartViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            AnimationMode = AnimationModes.OneTime;
            XAxisGap = 0;
            
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

        public ObservableCollection<DataSourceModel> APIList
        {
            get { return _apiList; }
            set { _apiList = value; NotifyOfPropertyChange(() => APIList); }
        }
        private ObservableCollection<DataSourceModel> _apiList;

        /// <summary>
        /// X轴坐标
        /// </summary>
        public ObservableCollection<string> XAxis
        {
            get { return _xAxis; }
            set { _xAxis = value; NotifyOfPropertyChange(() => XAxis); }
        }
        private ObservableCollection<string> _xAxis;

        /// <summary>
        /// Y轴坐标
        /// </summary>
        public ObservableCollection<string> YAxis
        {
            get { return _yAxis; }
            set { _yAxis = value; NotifyOfPropertyChange(() => YAxis); }
        }
        private ObservableCollection<string> _yAxis;

        /// <summary>
        /// 点集合
        /// </summary>
        public ObservableCollection<PUChartPoint> Points
        {
            get { return _points; }
            set { _points = value; NotifyOfPropertyChange(() => Points); }
        }
        private ObservableCollection<PUChartPoint> _points;

        public int XAxisGap
        {
            get { return _xAxisGap; }
            set { _xAxisGap = value; NotifyOfPropertyChange(() => XAxisGap); }
        }
        private int _xAxisGap;

        public double PointSize
        {
            get { return _pointSize; }
            set { _pointSize = value; NotifyOfPropertyChange(() => PointSize); }
        }
        private double _pointSize;

        public AnimationModes AnimationMode
        {
            get { return _animationMode; }
            set { _animationMode = value; NotifyOfPropertyChange(() => AnimationMode); }
        }
        private AnimationModes _animationMode;
        #endregion

        #region Event
        /// <summary>
        /// 阻止滚轮事件传播给DataGrid。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        public void AddItem()
        {

        }
        #endregion

        #region Function
        public async void Init()
        {
            await Task.Delay(100);
            DependencyPropertyList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "" ,Type = "" ,Description = "",DefaultValue = "" },
            };
            APIList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "" ,Description = "" },
            };
            AnnotationList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "" ,Description = "" },
            };

            Points = new ObservableCollection<PUChartPoint>()
            {
                new PUChartPoint() { Value = 0.3, ValueTip = "3" },
                new PUChartPoint() { Value = 0.2, ValueTip = "2" },
                new PUChartPoint() { Value = 0.3, ValueTip = "3" },
                new PUChartPoint() { Value = 0.1, ValueTip = "1" },
                new PUChartPoint() { Value = 0.5, ValueTip = "5" }
            };

            XAxis = new ObservableCollection<string>()
            {
                "1","2","3","4","5"
            };

            YAxis = new ObservableCollection<string>()
            {
                "0","1","2","3","4","5","6","7","8","9","10"
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
