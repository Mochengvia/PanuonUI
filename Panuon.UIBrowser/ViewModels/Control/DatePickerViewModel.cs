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
    public class DatePickerViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;

        #endregion

        #region Constructor
        public DatePickerViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#696969"));
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E3E3E"));
            DatePickerMode = DatePickerModes.DateOnly;
            Init();
            SelectedDateTime = DateTime.Now;
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

        public DatePickerModes DatePickerMode
        {
            get { return _datePickerModes; }
            set { _datePickerModes = value; NotifyOfPropertyChange(() => DatePickerMode); }
        }
        private DatePickerModes _datePickerModes;

        public DateTime? MaxDateTime
        {
            get { return _maxDateTime; }
            set { _maxDateTime = value; NotifyOfPropertyChange(() => MaxDateTime); }
        }
        private DateTime? _maxDateTime;

        public DateTime? MinDateTime
        {
            get { return _minDateTime; }
            set { _minDateTime = value; NotifyOfPropertyChange(() => MinDateTime); }
        }
        private DateTime? _minDateTime;

        public DateTime SelectedDateTime
        {
            get { return _selectedDateTime; }
            set { _selectedDateTime = value; NotifyOfPropertyChange(() => SelectedDateTime); }
        }
        private DateTime _selectedDateTime; 
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

        public void LimitMinDateTime(bool toLimit)
        {
            if (toLimit)
            {
                MinDateTime = DateTime.Now.AddMonths(-1);
            }
            else
            {
                MinDateTime = null;
            }
        }

        public void LimitMaxDateTime(bool toLimit)
        {
            if (toLimit)
            {
                MaxDateTime = DateTime.Now.AddMonths(1);
            }
            else
            {
                MaxDateTime = null;
            }
        }

        public void CoverBrushChanged(string content)
        {
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void ForegroundChanged(string content)
        {
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void DatePickerModeChanged(string content)
        {
            DatePickerMode = (DatePickerModes)Enum.Parse(typeof(DatePickerModes), content);
        }

        #endregion

        #region Function
        private async void Init()
        {
            await Task.Delay(100);

            DependencyPropertyList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "CoverBrush" ,Type = "Brush" ,Description = "获取或设置主题颜色。",DefaultValue = "#3E3E3E" },
                new DataSourceModel() { Name = "MaxDateTime" ,Type = "DateTime?" ,Description = "获取或设置可以选择的最大日期。该属性不能限制用户选择的时间。",DefaultValue = "Null" },
                new DataSourceModel() { Name = "MinDateTime" ,Type = "DateTime?" ,Description = "获取或设置可以选择的最小日期。该属性不能限制用户选择的时间。",DefaultValue = "Null" },
                new DataSourceModel() { Name = "SelectedDateTime" ,Type = "DateTime" ,Description = "获取或设置当前选中的日期和时间。",DefaultValue = "0" },
                new DataSourceModel() { Name = "DatePickerMode" ,Type = "DatePickerModes枚举" ,Description = "获取或设置日期选择器的模式。【可选项：DateOnly、TimeOnly、DateTime】",DefaultValue = "DateTime" },
            };
          
            AnnotationList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "不能限制时间选择" ,Description = "MaxDateTime和MinDateTime属性只对日期有效，不能限制用户选择时间。" },
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
