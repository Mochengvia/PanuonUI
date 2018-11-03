using Caliburn.Micro;
using Panuon.UI;
using Panuon.UIBrowser.ViewModels.Partial;
using System;
using System.ComponentModel.Composition;
using System.Windows.Threading;

namespace Panuon.UIBrowser.ViewModels
{
    [Export(typeof(IShell))]

    public class MainWindowViewModel : Conductor<IShell>.Collection.OneActive, IShell
    {
        public MainWindowViewModel()
        {
            ActivateItem(new IntroductionViewModel());
        }

        public void ChangeSelect(int select)
        {
            switch (select)
            {
                case 0:
                    ActivateItem(new IntroductionViewModel());
                    return;
                case 1:
                    ActivateItem(new WindowsViewModel());
                    return;
                case 2:
                    ActivateItem(new ButtonsViewModel());
                    return;
                case 3:
                    ActivateItem(new TextBoxsViewModel());
                    return;
                case 4:
                    ActivateItem(new ComboBoxsViewModel());
                    return;
                case 5:
                    ActivateItem(new PasswordBoxsViewModel());
                    return;
                case 6:
                    ActivateItem(new CheckBoxsViewModel());
                    return;
                case 7:
                    ActivateItem(new RadioButtonsViewModel());
                    return;
                case 8:
                    ActivateItem(new TreeViewsViewModel());
                    return;
                case 9:
                    ActivateItem(new ProgressBarsViewModel());
                    return;
                case 10:
                    ActivateItem(new TabControlsViewModel());
                    return;
                case 11:
                    ActivateItem(new SpecialViewModel());
                    return;
                case 12:
                    ActivateItem(new ListBoxsViewModel());
                    return;
                case 13:
                    ActivateItem(new SlidersViewModel());
                    return;
                case 14:
                    ActivateItem(new DataGridsViewModel());
                    return;
                case 15:
                    ActivateItem(new ImageCuterViewModel());
                    return;
                case 16:
                    ActivateItem(new DatePickersViewModel());
                    return;
                case 17:
                    ActivateItem(new PagingNavsViewModel());
                    return;
                case 18:
                    ActivateItem(new BubblesViewModel());
                    return;
                case 19:
                    ActivateItem(new ContextMenusViewModel());
                    return;
                case 101:
                    ActivateItem(new LineChartsViewModel());
                    return;

            }
        }

        public void ShowAwait()
        {
            (GetView() as PUWindow).IsAwaitShow = true;
        }
        public void CloseAwait()
        {
            (GetView() as PUWindow).IsAwaitShow = false;
        }
    }
}
