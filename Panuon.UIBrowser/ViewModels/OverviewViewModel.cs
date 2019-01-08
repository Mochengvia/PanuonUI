using Caliburn.Micro;
using Panuon.UI;
using Panuon.UIBrowser.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Panuon.UIBrowser.ViewModels
{
    public class OverviewViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public OverviewViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            ChangeThemeBrush(new RoutedPropertyChangedEventArgs<int>(1, 1));
        }
        #endregion

        #region Bindings
        public Brush DeepCoverBrush
        {
            get { return _deepCoverBrush; }
            set { _deepCoverBrush = value; NotifyOfPropertyChange(() => DeepCoverBrush); }
        }
        private Brush _deepCoverBrush;

        public Brush LightCoverBrush
        {
            get { return _lightCoverBrush; }
            set { _lightCoverBrush = value; NotifyOfPropertyChange(() => LightCoverBrush); }
        }
        private Brush _lightCoverBrush;

        public Color LightColor
        {
            get { return _lightColor; }
            set { _lightColor = value; NotifyOfPropertyChange(() => LightColor); }
        }
        private Color _lightColor; 
        #endregion

        #region Event
        public void Detail(string category)
        {
            switch (category)
            {
                case "Button":
                    PUMessageBox.ShowDialog("123");
                    break;
            }
        }
        public void ChangeThemeBrush(RoutedPropertyChangedEventArgs<int> e)
        {
            switch (e.NewValue)
            {
                case 1:
                    DeepCoverBrush = new LinearGradientBrush()
                    {
                        GradientStops = new GradientStopCollection()
                        {
                            new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#444444"), Offset = 0 },
                            new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#CC444444"), Offset = 1 }
                        }
                    };
                    LightCoverBrush = new LinearGradientBrush()
                    {
                        GradientStops = new GradientStopCollection()
                        {
                            new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#66444444"), Offset = 0 },
                            new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#44444444"), Offset = 1 }
                        }
                    };
                    LightColor = (Color)ColorConverter.ConvertFromString("#44444444");
                    break;
                case 2:
                    DeepCoverBrush = new LinearGradientBrush()
                    {
                        GradientStops = new GradientStopCollection()
                        {
                            new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#49A9C0"), Offset = 0 },
                            new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#CC49A9C0"), Offset = 1 }
                        }
                    };
                    LightCoverBrush = new LinearGradientBrush()
                    {
                        GradientStops = new GradientStopCollection()
                        {
                            new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#6649A9C0"), Offset = 0 },
                            new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#4449A9C0"), Offset = 1 }
                        }
                    };
                    LightColor = (Color)ColorConverter.ConvertFromString("#4449A9C0");
                    break;
                case 3:
                    DeepCoverBrush = new LinearGradientBrush()
                    {
                        GradientStops = new GradientStopCollection()
                        {
                            new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#E089B8"), Offset = 0 },
                            new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#CCE089B8"), Offset = 1 }
                        }
                    };
                    LightCoverBrush = new LinearGradientBrush()
                    {
                        GradientStops = new GradientStopCollection()
                        {
                            new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#66E089B8"), Offset = 0 },
                            new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#44E089B8"), Offset = 1 }
                        }
                    };
                    LightColor = (Color)ColorConverter.ConvertFromString("#44E089B8");
                    break;
                case 4:
                    DeepCoverBrush = new LinearGradientBrush()
                    {
                        GradientStops = new GradientStopCollection()
                        {
                            new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#F4A758"), Offset = 0 },
                            new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#CCF4A758"), Offset = 1 }
                        }
                    };
                    LightCoverBrush = new LinearGradientBrush()
                    {
                        GradientStops = new GradientStopCollection()
                        {
                            new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#66F4A758"), Offset = 0 },
                            new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#44F4A758"), Offset = 1 }
                        }
                    };
                    LightColor = (Color)ColorConverter.ConvertFromString("#44F4A758");
                    break;
            }
        }
        #endregion

        #region Function

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
