using Caliburn.Micro;
using Panuon.UI;
using Panuon.UIBrowser.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;

namespace Panuon.UIBrowser.ViewModels.Control
{
    public class TabControlViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;

        #endregion

        #region Constructor
        public TabControlViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            TabControlStyle = TabControlStyles.General;
            SelectedValue = 1;
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#696969"));
            SelectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E3E3E"));
            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D3D3D3"));
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

        public ObservableCollection<DataSourceModel> DependencyPropertyList2
        {
            get { return _dependencyPropertyList2; }
            set
            { _dependencyPropertyList2 = value; NotifyOfPropertyChange(() => DependencyPropertyList2); }
        }
        private ObservableCollection<DataSourceModel> _dependencyPropertyList2;

        public ObservableCollection<DataSourceModel> AnnotationList
        {
            get { return _annotationList; }
            set
            { _annotationList = value; NotifyOfPropertyChange(() => AnnotationList); }
        }
        private ObservableCollection<DataSourceModel> _annotationList;

        public TabControlStyles TabControlStyle
        {
            get { return _tabControlStyles; }
            set { _tabControlStyles = value; NotifyOfPropertyChange(() => TabControlStyle); }
        }
        private TabControlStyles _tabControlStyles;

        public bool CanDeleteIsChecked
        {
            get { return _canDeleteIsChecked; }
            set
            {
                _canDeleteIsChecked = value;
                TabItems.Apply(x => x.CanDelete = value);
                NotifyOfPropertyChange(() => CanDeleteIsChecked);
            }
        }
        private bool _canDeleteIsChecked;

        public Brush BorderBrush
        {
            get { return _borderBrush; }
            set { _borderBrush = value; NotifyOfPropertyChange(() => BorderBrush); }
        }
        private Brush _borderBrush;

        public Brush SelectedBrush
        {
            get { return _selectedBrush; }
            set { _selectedBrush = value; NotifyOfPropertyChange(() => SelectedBrush); }
        }
        private Brush _selectedBrush;

        public Brush Foreground
        {
            get { return _foreground; }
            set { _foreground = value; NotifyOfPropertyChange(() => Foreground); }
        }
        private Brush _foreground;

        public ObservableCollection<PUTabItemModel> TabItems
        {
            get { return _tabItems; }
            set { _tabItems = value; NotifyOfPropertyChange(() => TabItems); }
        }
        private ObservableCollection<PUTabItemModel> _tabItems;

        public int SelectedValue
        {
            get { return _selectedValue; }
            set { _selectedValue = value; NotifyOfPropertyChange(() => SelectedValue); }
        }
        private int _selectedValue;
        #endregion

        #region Event
        public void HandleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        public void AddItem()
        {
            TabItems.Add(new PUTabItemModel()
            {
                CanDelete = CanDeleteIsChecked,
                Header = "TabItem" + (TabItems.Count + 1),
                Value = (TabItems.Count + 1),
                Content = "Page " + (TabItems.Count + 1),
            });
        }

        public void RemoveItem()
        {
            if (TabItems.Count > 0)
                TabItems.RemoveAt(TabItems.Count - 1);
        }

        public void StyleChanged(string content)
        {
            TabControlStyle = (TabControlStyles)Enum.Parse(typeof(TabControlStyles), content);
        }

        public void SelectedBrushChanged(string content)
        {
            SelectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void ForegroundChanged(string content)
        {
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void BorderBrushChanged(string content)
        {
            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }
        #endregion

        #region Function
        private async void Init()
        {
            await Task.Delay(100);

            TabItems = new BindableCollection<PUTabItemModel>()
            {
                new PUTabItemModel() {  Header = "TabItem1",Value =1, Content = "Page 1",  },
                new PUTabItemModel() {  Header = "TabItem2",Value =2, Content = "Page 2",  },
                new PUTabItemModel() {  Header = "TabItem3",Value =3, Content = "Page 3",  },
            };

            DependencyPropertyList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "TabControlStyle" ,Type = "TabControlStyles枚举类型" ,Description = "获取或设置选项卡的基本样式。【可选值：General、Classic】",DefaultValue = "General" },
                new DataSourceModel() { Name = "DeleteMode" ,Type = "DeleteModes枚举类型" ,Description = "获取或设置当子项设置为可删除时，用户点击删除按钮后应执行的操作。【可选值：Delete（立即删除项目并触发DeleteItem事件）、EventOnly（不删除项目，仅触发DeleteItem事件）】",DefaultValue = "Delete" },
                new DataSourceModel() { Name = "SelectedBrush" ,Type = "Brush" ,Description = "获取或设置当某个子项被选中时的前景色。",DefaultValue = "#3E3E3E" },
                new DataSourceModel() { Name = "CoverBrush" ,Type = "Brush" ,Description = "获取或设置鼠标悬浮时遮罩层的背景颜色",DefaultValue = "0" },
                new DataSourceModel() { Name = "SelectedValuePath" ,Type = "SelectedValuePaths枚举" ,Description = "该属性指定了当子项目被选中时，SelectedValue应呈现子项目的哪一个值。【可选值：Header、Value】",DefaultValue = "Header" },
                new DataSourceModel() { Name = "SelectedValue" ,Type = "Object" ,Description = "获取被选中PUTabItem的Header或Value属性（这取决于SelectedValuePath），或反向选中子项目。",DefaultValue = "Null" },
                new DataSourceModel() { Name = "BindingItems" ,Type = "ObservableCollection<PUTabItemModel>" ,Description = "若使用MVVM绑定，请使用此依赖属性。详见注解”已禁用ItemSource属性“。",DefaultValue = "Null" },
            };
            DependencyPropertyList2 = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "CanDelete" ,Type = "Boolean" ,Description = "获取或设置选项卡的基本样式。【可选值：General、Classic】",DefaultValue = "General" },
                new DataSourceModel() { Name = "Icon" ,Type = "Object" ,Description = "获取或设置显示在选项卡前的图标。",DefaultValue = "Null" },
                new DataSourceModel() { Name = "Value" ,Type = "Object" ,Description = "获取或设置该子项可以携带的值，不会对前端显示造成影响。",DefaultValue = "Null" },
            };
            AnnotationList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "TabItem的样式" ,Description = "TabItem的前景色、边框颜色都会使用TabControl的前景色和边框颜色，无需再为每个TabItem单独设置。" },
                new DataSourceModel() { Name = "已禁用ItemsSource属性" ,Description = "ItemsSource属性不能自动生成PUTabItem，因此隐藏了Set方法（为了防止误用）。若要使用MVVM绑定，请使用BindingItems属性。该属性已实现双向绑定，当BindingItems属性发生改变时，TabControl的Items将同步发生变化；反之亦然。请根据需要来选择是否设置Mode=TwoWay。\n此外，PUTabItemModel在初始化时会为其Uid属性（只读的）生成一个值；BindingItem在生成子控件时，会将该Model中的Uid值赋给它所对应子控件的Uid属性。这意味着你可以通过该Model的Uid属性来查找真实的控件。" },
                new DataSourceModel() { Name = "如果你想在删除前进行验证" ,Description = "DeleteMode提供了两种方式来处理删除事件。Delete选项表示当用户点击删除按钮时，应当立即删除项目并触发DeleteItem路由事件；而EventOnly选项则只会触发DeleteItem路由事件，不会删除项目。你可以监听该事件的发生，并判断是否应该删除该项目。" },
                new DataSourceModel() { Name = "关于SelectedValuePath" ,Description = "PanuonUI中的大多数组合型容器控件都重写了此属性。重写后的SelectedValuePath只有两个选项：Header和Value。Header选项表示SelectedValue属性应当呈现被选择项的Header属性，而Value选项则表示SelectedValue属性应呈现被选择项的Value属性。" },
                new DataSourceModel() { Name = "关于SelectedValue" ,Description = "当选中项发生改变时，该属性的值会依据SelectedValuePath的设定发生变化。同时，你可以通过对该属性赋值来搜索并选中项目（至于是通过子项的Header还是Value进行搜索，这同样取决于SelectedValuePath的值）。请注意，搜索时使用的是Equal方法，这意味着1和“1”是完全不同的（错误赋值可能会导致搜索不到子项）。" },
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
