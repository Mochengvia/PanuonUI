/*==============================================================
*作者：ZEOUN
*时间：2018/11/15 13:12:35
*说明： 样板。
*日志：2018/11/15 13:12:35 创建。
*==============================================================*/
using Caliburn.Micro;
using Panuon.UIBrowser.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Panuon.UIBrowser.ViewModels
{
    public class Example : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public Example(IWindowManager windowManager)
        {
            _windowManager = windowManager;
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
            set  { _apiList = value; NotifyOfPropertyChange(() => APIList); }
        }
        private ObservableCollection<DataSourceModel> _apiList;
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
