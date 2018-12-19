using Caliburn.Micro;
using Panuon.UI;
using Panuon.UIBrowser.Models;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Panuon.UIBrowser.ViewModels.Control
{
    public class WindowViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public WindowViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            Init();
        }
        #endregion

        #region Bindings
        public ObservableCollection<DataSourceModel> DependencyPropertyList
        {
            get { return _dependencyPropertyList; }
            set
            { _dependencyPropertyList = value; NotifyOfPropertyChange(() => DependencyPropertyList); }
        }
        private ObservableCollection<DataSourceModel> _dependencyPropertyList;

        public ObservableCollection<DataSourceModel> APIList
        {
            get { return _apiList; }
            set
            { _apiList = value; NotifyOfPropertyChange(() => APIList); }
        }
        private ObservableCollection<DataSourceModel> _apiList;

        public ObservableCollection<DataSourceModel> AnnotationList
        {
            get { return _annotationList; }
            set
            { _annotationList = value; NotifyOfPropertyChange(() => AnnotationList); }
        }
        private ObservableCollection<DataSourceModel> _annotationList;
        #endregion

        #region Event
        public void HandleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        /// <summary>
        /// 使用数字作为参数不是一个好方法，这里为了方便
        /// </summary>
        /// <param name="category"></param>
        public void Display(int category)
        {
            switch (category)
            {
                case 1:
                    var window1 = new Views.Control.Examples.MultiNavWindow();
                    window1.ShowDialog();
                    if (!window1.Result.Equals(0))
                        PUMessageBox.ShowDialog("最后点击的按钮（缩放和关闭按钮不算）是第" + window1.Result + "个");
                    break;
                case 2:
                    var window2 = new Views.Control.Examples.LoginWindow();
                    window2.Owner = (Parent as ShellWindowViewModel).GetCurrentWindow();
                    window2.ShowDialog();
                    break;
                case 3:
                    var window4 = new Views.Control.Examples.ChatWindow();
                    window4.ShowDialog();
                    break;
                case 4:
                    SetAwait(true);
                    Task.Run(() =>
                    {
                        Thread.Sleep(2000);
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            SetAwait(false);
                        });
                    });
                    break;
                case 5:
                    PUMessageBox.ShowDialog("Hello World");
                    break;
                case 6:
                    PUMessageBox.ShowConfirm("Hello World");
                    break;
                case 7:
                    PUMessageBox.ShowAwait("正在执行......", delegate
                        {
                            PUMessageBox.CloseAwait(delegate
                            {
                                PUMessageBox.ShowDialog("已取消。");
                            });
                        });
                    break;
            }
        }
        #endregion

        #region Function
        private async void Init()
        {
            await Task.Delay(100);
            DependencyPropertyList = new ObservableCollection<DataSourceModel>()
                {
                    new DataSourceModel() { Name = "AnimationStyle" ,Type = "AnimationStyles枚举" ,Description = "获取或设置窗体动画类型。【可选值：Gradual（从上到下的渐变显示）、Scale（淡入淡出+缩放）、Fade（淡入淡出）】",DefaultValue = "Scale" },
                    new DataSourceModel() { Name = "AnimateIn" ,Type = "Boolean" ,Description = "获取或设置是否在窗体打开时使用动画。",DefaultValue = "True" },
                    new DataSourceModel() { Name = "AnimateOut" ,Type = "Boolean" ,Description = "获取或设置是否在窗体关闭时使用动画。",DefaultValue = "True" },
                    new DataSourceModel() { Name = "AllowShowDelay" ,Type = "Boolean" ,Description = "获取或设置是否允许延迟显示内容。当页面内容较为复杂时，启用此选项有助于减少启动动画卡顿。",DefaultValue = "False" },
                    new DataSourceModel() { Name = "IsCoverMaskShow" ,Type = "Boolean" ,Description = "获取或设置是否打开窗体的遮罩层。",DefaultValue = "False" },
                    new DataSourceModel() { Name = "IsAwaitShow" ,Type = "Boolean" ,Description = "获取或设置是否打开窗体的遮罩层，以及等待控件（显示为多个旋转的小球）。",DefaultValue = "False" },
                    new DataSourceModel() { Name = "NavButtonVisibility" ,Type = "Visibility" ,Description = "获取或设置右侧标题栏按钮的显示状态。",DefaultValue = "Visible" },
                    new DataSourceModel() { Name = "NavbarBackground" ,Type = "Brush" ,Description = "获取或设置标题栏的背景颜色。",DefaultValue = "White" },
                    new DataSourceModel() { Name = "NavbarHeight" ,Type = "Double" ,Description = "获取或设置标题栏的高度。",DefaultValue = "30" },
                    new DataSourceModel() { Name = "NavButtonHeight" ,Type = "Double" ,Description = "获取或设置标题栏按钮的高度。",DefaultValue = "30" },
                    new DataSourceModel() { Name = "NavButtonWidth" ,Type = "Double" ,Description = "获取或设置标题栏按钮的宽度。",DefaultValue = "40" },
                    new DataSourceModel() { Name = "BorderCornerRadius" ,Type = "CornerRadius" ,Description = "获取或设置窗体的圆角大小。",DefaultValue = "0" },
                    new DataSourceModel() { Name = "Header" ,Type = "Object" ,Description = "获取或设置窗体的标题。若为空，则窗体标题将使用Title的值。",DefaultValue = "Null" },
                    new DataSourceModel() { Name = "AllowAutoOwner" ,Type = "Boolean" ,Description = "获取或设置是否允许窗体在初始化时，尝试将系统中排在最前面的活动窗口设置为自己的Owner。",DefaultValue = "True" },
                    new DataSourceModel() { Name = "AllowAutoCoverMask" ,Type = "Boolean" ,Description = "获取或设置是否允许在调用Show或ShowDialog方法时自动打开父窗体（若Owner为Null或Owner不是PUWindow类型，则无效）的遮罩层，并在Close时将其关闭。",DefaultValue = "False" },
                    new DataSourceModel() { Name = "AllowForcingClose" ,Type = "Boolean" ,Description = "获取或设置是否允许用户使用Alt + F4组合键强制关闭当前窗体。",DefaultValue = "False" },
                    new DataSourceModel() { Name = "Result" ,Type = "Object" ,Description = "获取或设置窗体的返回结果，不会对前端显示造成影响。",DefaultValue = "Null" },
                };
            APIList = new ObservableCollection<DataSourceModel>()
                {
                    new DataSourceModel() { Name = "AppendNavButton(object content, RoutedEventHandler clickHandler)" ,Description = "向标题栏右侧控制按钮组中添加一个新的按钮，该按钮将被添加在按钮组的最左侧。" },
                };
            AnnotationList = new ObservableCollection<DataSourceModel>()
                {
                    new DataSourceModel() { Name = "Owner属性" ,Description = "在PUWindow初始化时，Owner会被自动设置为当前的首个活动窗口（若AllowAutoOwner为False，不会执行此操作；如果失败，不会触发异常），以便于使用AllowAutoCoverMask属性。如果你在窗体Show或ShowDialog前手动指定了窗体的Owner，则将以你的为准。请注意以下情况：当你希望关闭当前窗体并立即打开一个新窗体时，您需要注意新窗体的Owner问题。AutoOwner可能会把即将关闭的窗体设置为自己的Owner，从而导致新窗体一打开就被立即关闭（因为它的Owner关闭了）。" },
                    new DataSourceModel() { Name = "Result" ,Description = "DialogResult属性只有True、False和Null三种选项。若返回结果较为复杂，可以使用该属性。" },
                    new DataSourceModel() { Name = "窗体背景色" ,Description = "若要设置覆盖全窗体的背景色，需要在设置Background的同时将NavBackground设置为Transparent。" },
                    new DataSourceModel() { Name = "有关Header属性" ,Description = "Title属性会同时作用于窗体的左上角标题和任务栏标题。如果你期望使用不同的值，使用Header属性可以单独设置左上角的标题。若Header属性为空，PanuonUI将使用Title属性的值作为左上角标题。若不希望使用任何左上角标题，请将Header属性设置为空格（而不是为空）" },
                    new DataSourceModel() { Name = "PanuonUI中带有“Brush”字样的属性" ,Description = "你可以使用渐变画刷或图像画刷来设置它们（这些属性都是Brush类型的）。" },
                    new DataSourceModel() { Name = "PanuonUI中的“Icon”属性" ,Description = "Icon属性通常都是Object类型的。所有Icon的默认字体样式（因为它的本质是一个Label）都是 {DynamicResource IconFont}，你可以直接将FontAwesome图标字体赋值给Icon属性，也可以将该属性设置为任意一个控件。" },
                };
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
