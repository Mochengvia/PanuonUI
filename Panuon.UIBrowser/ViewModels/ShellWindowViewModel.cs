using Caliburn.Micro;
using Panuon.UI;
using Panuon.UI.Utils;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Threading;

namespace Panuon.UIBrowser.ViewModels
{
    [Export(typeof(IShell))]

    public class ShellWindowViewModel : Conductor<IShell>.Collection.OneActive, IShell
    {
        #region Identity
        private PUWindow CurrentWindow
        {
            get
            {
                if (_currentWindow == null)
                    _currentWindow = (GetView()) as PUWindow;
                return _currentWindow;
            }
        }
        private PUWindow _currentWindow;
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        [ImportingConstructor]
        public ShellWindowViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public void BtnTest()
        {
            ChoosedValue = "1234";
        }
        #endregion

        #region Bindings
        public string ChoosedValue
        {
            get { return _choosedValue; }
            set
            {
                _choosedValue = value;
                UpdateActivePage();
                NotifyOfPropertyChange(() => ChoosedValue);
            }
        }
        private string  _choosedValue;
        #endregion

        #region Event
        public void UpdateActivePage()
        {
            switch (ChoosedValue)
            {
                case "Window":
                    ActivateItem(new Control.WindowViewModel(_windowManager));
                    break;
                case "Button":
                    ActivateItem(new Control.ButtonViewModel(_windowManager));
                    break;
                case "TextBox":
                    ActivateItem(new Control.TextBoxViewModel(_windowManager));
                    break;
                case "PasswordBox":
                    ActivateItem(new Control.PasswordBoxViewModel(_windowManager));
                    break;
                case "ComboBox":
                    ActivateItem(new Control.ComboBoxViewModel(_windowManager));
                    break;
                case "CheckBox":
                    ActivateItem(new Control.CheckBoxViewModel(_windowManager));
                    break;
                case "RadioButton":
                    ActivateItem(new Control.RadioButtonViewModel(_windowManager));
                    break;
                case "ProgressBar":
                    ActivateItem(new Control.ProgressBarViewModel(_windowManager));
                    break;
                case "TreeView":
                    ActivateItem(new Control.TreeViewViewModel(_windowManager));
                    break;
                case "TabControl":
                    ActivateItem(new Control.TabControlViewModel(_windowManager));
                    break;
                case "DatePicker":
                    ActivateItem(new Control.DatePickerViewModel(_windowManager));
                    break;
                case "ListBox":
                    ActivateItem(new Control.ListBoxViewModel(_windowManager));
                    break;
                case "Slider":
                    ActivateItem(new Control.SliderViewModel(_windowManager));
                    break;
                case "ContextMenu":
                    ActivateItem(new Control.ContextMenuViewModel(_windowManager));
                    break;
                case "ResizeGrid":
                    ActivateItem(new Special.ResizeGridViewModel(_windowManager));
                    break;
                case "Bubble":
                    ActivateItem(new Special.BubbleViewModel(_windowManager));
                    break;
                case "Loading":
                    ActivateItem(new Special.LoadingViewModel(_windowManager));
                    break;
                case "SplitLine":
                    ActivateItem(new Special.SplitLineViewModel(_windowManager));
                    break;
                case "ImageCuter":
                    ActivateItem(new Special.ImageCuterViewModel(_windowManager));
                    break;
                case "LineChart":
                    ActivateItem(new Chart.LineChartViewModel(_windowManager));
                    break;
            }

        }
        #endregion

        #region Function
        #endregion

        #region APIs
        public Window GetCurrentWindow()
        {
            return GetView() as Window;
        }

        /// <summary>
        /// 打开等待控件。
        /// </summary>
        public void ShowAwait()
        {
            CurrentWindow.IsAwaitShow = true;
        }

        /// <summary>
        /// 关闭等待控件。
        /// </summary>
        public void CloseAwait()
        {
            CurrentWindow.IsAwaitShow = false;
        }

        /// <summary>
        /// 打开遮罩层。
        /// </summary>
        public void ShowCoverMask()
        {
            CurrentWindow.IsCoverMaskShow = true;
        }

        /// <summary>
        /// 关闭遮罩层。
        /// </summary>
        public void CloseCoverMask()
        {
            CurrentWindow.IsCoverMaskShow = false;
        }
        #endregion
    }

}
