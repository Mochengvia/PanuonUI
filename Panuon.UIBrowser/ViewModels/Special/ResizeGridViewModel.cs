using Caliburn.Micro;

namespace Panuon.UIBrowser.ViewModels.Special
{
    public class ResizeGridViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public ResizeGridViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;

        }
        #endregion
    }
}
