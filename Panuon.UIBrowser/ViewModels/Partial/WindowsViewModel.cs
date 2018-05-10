using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class WindowsViewModel : Screen, IShell
    {

        public void OpenDialog(string type)
        {
            switch (type)
            {
                case "scale":
                    UI.PUMessageBox.ShowDialog("这是一个PUMessageBox对话框。");
                    return;
                case "gradual":
                    UI.PUMessageBox.ShowDialog("这是一个PUMessageBox对话框。", "提示", true, UI.PUWindow.AnimationStyles.Gradual);
                    return;
                case "fade":
                    UI.PUMessageBox.ShowDialog("这是一个PUMessageBox对话框。", "提示", true, UI.PUWindow.AnimationStyles.Fade);
                    return;
            }
        }
    }
}
