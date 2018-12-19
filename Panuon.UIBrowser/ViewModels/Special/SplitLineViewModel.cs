using Caliburn.Micro;
using Panuon.UIBrowser.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Panuon.UIBrowser.ViewModels.Special
{
    public class SplitLineViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public SplitLineViewModel(IWindowManager windowManager)
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

        #endregion

        #region Function
        public async void Init()
        {
            await Task.Delay(100);
            DependencyPropertyList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "Alignment" ,Type = "Alignments枚举" ,Description = "获取或设置分割线的位置。【可选项：Left、Top、Bottom、Right】",DefaultValue = "Top" },
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
