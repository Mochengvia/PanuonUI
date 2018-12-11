using Caliburn.Micro;
using Panuon.UI;
using Panuon.UIBrowser.Views.Partial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class WindowsViewModel : Screen, IShell
    {

        public void OpenDialog(string type)
        {
            switch (type)
            {
                case "scale":
                    PUMessageBox.ShowDialog("这是一个PUMessageBox对话框。");
                    return;
                case "gradual":
                    PUMessageBox.ShowDialog("这是一个PUMessageBox对话框。", "提示", PUMessageBox.Buttons.Sure, true, UI.PUWindow.AnimationStyles.Gradual);
                    return;
                case "fade":
                    PUMessageBox.ShowDialog("这是一个PUMessageBox对话框。", "提示", PUMessageBox.Buttons.Sure, true, UI.PUWindow.AnimationStyles.Fade);
                    return;
            }
        }

        public void ShowAwait()
        {
            var parent = Parent as MainWindowViewModel;
            parent.ShowAwait();
            var task = new Task(() =>
            {
                Thread.Sleep(2000);
                App.Current.Dispatcher.BeginInvoke(new System.Action(() =>
                {
                    parent.CloseAwait();
                }));
            });
            task.Start();
        }

        public void ShowCancelableAwait()
        {
            PUMessageBox.ShowAwait("正在执行......", delegate
            {
                PUMessageBox.CloseAwait(delegate 
                {
                    PUMessageBox.ShowDialog("任务已取消。");
                });
            });
        }
    }
}
