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
    public class PagingNavViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public PagingNavViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            RadiusInteger = 3;
            CurrentPage = 1;
            TotalPage = 10;
            SelectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E3E3E"));
            ButtonBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AA3E3E3E"));
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

        public ObservableCollection<DataSourceModel> EventList
        {
            get { return _eventList; }
            set { _eventList = value; NotifyOfPropertyChange(() => EventList); }
        }
        private ObservableCollection<DataSourceModel> _eventList;

        public int RadiusInteger
        {
            get { return _radiusInteger; }
            set { _radiusInteger = value; ButtonCornerRadius = new CornerRadius(value); NotifyOfPropertyChange(() => RadiusInteger); }
        }
        private int _radiusInteger;

        public int TotalPage
        {
            get { return _totalPage; }
            set { _totalPage = value; NotifyOfPropertyChange(() => TotalPage); }
        }
        private int _totalPage;

        public int CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; NotifyOfPropertyChange(() => CurrentPage); }
        }
        private int _currentPage;

        public CornerRadius ButtonCornerRadius
        {
            get { return _buttonCornerRadius; }
            set { _buttonCornerRadius = value; NotifyOfPropertyChange(() => ButtonCornerRadius); }
        }
        private CornerRadius _buttonCornerRadius;

        public Brush SelectedBrush
        {
            get { return _selectedBrush; }
            set { _selectedBrush = value; NotifyOfPropertyChange(() => SelectedBrush); }
        }
        private Brush _selectedBrush;

        public Brush ButtonBrush
        {
            get { return _buttonBrush; }
            set { _buttonBrush = value; NotifyOfPropertyChange(() => ButtonBrush); }
        }
        private Brush _buttonBrush;

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

        public void SelectedBrushChanged(string content)
        {
            SelectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void ButtonBrushChanged(string content)
        {
            ButtonBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }
       
        #endregion

        #region Function
        private async void Init()
        {
            await Task.Delay(100);
            DependencyPropertyList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "TotalPage" ,Type = "Int" ,Description = "获取或设置当前的总页数。",DefaultValue = "1" },
                new DataSourceModel() { Name = "CurrentPage" ,Type = "Int" ,Description = "获取或设置当前的页码。",DefaultValue = "1" },
                new DataSourceModel() { Name = "ButtonBrush" ,Type = "Brush" ,Description = "获取或设置按钮的背景颜色。",DefaultValue = "#AA3E3E3E" },
                new DataSourceModel() { Name = "SelectedBrush" ,Type = "Brush" ,Description = "获取或设置按钮被选中或点击时的颜色。",DefaultValue = "#3E3E3E" },
                new DataSourceModel() { Name = "ButtonCornerRadius" ,Type = "CornerRadius" ,Description = "获取或设置按钮的圆角大小。",DefaultValue = "3" },
                new DataSourceModel() { Name = "IsSideButtonShow" ,Type = "Boolean" ,Description = "获取或设置左右两侧的翻页按钮是否显示。",DefaultValue = "True" },
            };
            EventList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "CurrentPageChanged" ,Description = "当CurrentPage（当前页码）属性发生变化时，触发此事件。事件参数：Percent属性值。" },
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
