using Caliburn.Micro;
using Panuon.UI;
using Panuon.UIBrowser.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Panuon.UIBrowser.ViewModels.Control
{
    public class ComboBoxViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public ComboBoxViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            SelectedValue = 1;
            RadiusInteger = 3;
            SelectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DDDDDD"));
            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D3D3D3"));
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#696969"));
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEEEEE"));
            ShadowColor = (Color)ColorConverter.ConvertFromString("#888888");
            SearchMode = SearchModes.None;
            DeleteMode = DeleteModes.Delete;
            Init();
        }
        #endregion

        #region Bindings
        public ObservableCollection<PUComboBoxItemModel> ComboBoxItems
        {
            get { return _comboBoxItems; }
            set { _comboBoxItems = value; NotifyOfPropertyChange(() => ComboBoxItems); }
        }
        private ObservableCollection<PUComboBoxItemModel> _comboBoxItems;

        public ObservableCollection<DataSourceModel> DependencyPropertyList
        {
            get { return _dependencyPropertyList; }
            set { _dependencyPropertyList = value; NotifyOfPropertyChange(() => DependencyPropertyList); }
        }
        private ObservableCollection<DataSourceModel> _dependencyPropertyList;

        public ObservableCollection<DataSourceModel> DependencyPropertyList2
        {
            get { return _dependencyPropertyList2; }
            set { _dependencyPropertyList2 = value; NotifyOfPropertyChange(() => DependencyPropertyList2); }
        }
        private ObservableCollection<DataSourceModel> _dependencyPropertyList2;

        public ObservableCollection<DataSourceModel> AnnotationList
        {
            get { return _annotationList; }
            set { _annotationList = value; NotifyOfPropertyChange(() => AnnotationList); }
        }
        private ObservableCollection<DataSourceModel> _annotationList;

        public int RadiusInteger
        {
            get { return _radiusInteger; }
            set { _radiusInteger = value; BorderCornerRadius = new CornerRadius(value); NotifyOfPropertyChange(() => RadiusInteger); }
        }
        private int _radiusInteger;

        public bool EditableIsChecked
        {
            get { return _editableIsChecked; }
            set { _editableIsChecked = value; NotifyOfPropertyChange(() => EditableIsChecked); }
        }
        private bool _editableIsChecked;

        public bool CanDeleteIsChecked
        {
            get { return _canDeleteIsChecked; }
            set
            {
                _canDeleteIsChecked = value;
                ComboBoxItems.Apply(x => x.CanDelete = value);
                NotifyOfPropertyChange(() => CanDeleteIsChecked);
            }
        }
        private bool _canDeleteIsChecked;

        public CornerRadius BorderCornerRadius
        {
            get { return _borderCornerRadius; }
            set { _borderCornerRadius = value; NotifyOfPropertyChange(() => BorderCornerRadius); }
        }
        private CornerRadius _borderCornerRadius;

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

        public Brush CoverBrush
        {
            get { return _coverBrush; }
            set { _coverBrush = value; NotifyOfPropertyChange(() => CoverBrush); }
        }
        private Brush _coverBrush;

        public Brush Foreground
        {
            get { return _foreground; }
            set { _foreground = value; NotifyOfPropertyChange(() => Foreground); }
        }
        private Brush _foreground;

        public Color ShadowColor
        {
            get { return _shadowColor; }
            set { _shadowColor = value; NotifyOfPropertyChange(() => ShadowColor); }
        }
        private Color _shadowColor;

        public SearchModes SearchMode
        {
            get { return _searchMode; }
            set { _searchMode = value; NotifyOfPropertyChange(() => SearchMode); }
        }
        private SearchModes _searchMode;

        public DeleteModes DeleteMode
        {
            get { return _deleteMode; }
            set { _deleteMode = value; NotifyOfPropertyChange(() => DeleteMode); }
        }
        private DeleteModes _deleteMode;

        public int SelectedValue
        {
            get { return _selectedValue; }
            set { _selectedValue = value; NotifyOfPropertyChange(() => SelectedValue); }
        }
        private int _selectedValue;
        #endregion

        #region Event
        /// <summary>
        /// 阻止滚轮事件传播给DataGrid。
        /// </summary>
        public void HandleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        public void AddItem()
        {
            ComboBoxItems.Add(new PUComboBoxItemModel()
            {
                CanDelete = CanDeleteIsChecked,
                Header = "Item" + (ComboBoxItems.Count + 1),
                Value = (ComboBoxItems.Count + 1),
            });
        }

        public void RemoveItem()
        {
            if (ComboBoxItems.Count > 0)
                ComboBoxItems.RemoveAt(ComboBoxItems.Count - 1);
        }

        public void SelectedBrushChanged(string content)
        {
            SelectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void CoverBrushChanged(string content)
        {
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void ForegroundChanged(string content)
        {
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void ShadowColorChanged(string content)
        {
            ShadowColor = (Color)ColorConverter.ConvertFromString(content);
        }

        public void BorderBrushChanged(string content)
        {
            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void SearchModeChanged(string content)
        {
            SearchMode = (SearchModes)Enum.Parse(typeof(SearchModes), content);
        }

        public void DeleteModeChanged(string content)
        {
            DeleteMode = (DeleteModes)Enum.Parse(typeof(DeleteModes), content);
        }

        public void DeleteItem(RoutedPropertyChangedEventArgs<PUComboBoxItem> e)
        {
            if (DeleteMode == DeleteModes.Delete)
                return;
            var comboBoxItem = e.NewValue;

            //移除绑定值的Model即可，切记不能直接操作ComboBox的Items属性，否则会出现混乱
            var model = ComboBoxItems.FirstOrDefault(x => x.Uid == comboBoxItem.Uid);

            if (model != null && PUMessageBox.ShowConfirm("确认要删除该选项吗？") == true)
            {
                ComboBoxItems.Remove(model);
            }
        }
        #endregion

        #region Function
        private async void Init()
        {
            await Task.Delay(100);

            ComboBoxItems = new ObservableCollection<PUComboBoxItemModel>()
            {
                new PUComboBoxItemModel() {  Header = "Item1", Value =1 },
                new PUComboBoxItemModel() {  Header = "Item2", Value =2 },
            };

            DependencyPropertyList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "CoverBrush" ,Type = "Brush" ,Description = "获取或设置鼠标悬浮时子项的背景颜色。",DefaultValue = "#EEEEEE" },
                new DataSourceModel() { Name = "SelectedBrush" ,Type = "Brush" ,Description = "获取或设置子项被选中时的背景颜色。",DefaultValue = "#DDDDDD" },
                new DataSourceModel() { Name = "ShadowColor" ,Type = "Color" ,Description = "获取或设置下拉框激活时阴影的颜色。",DefaultValue = "#888888" },
                new DataSourceModel() { Name = "BorderCornerRadius" ,Type = "CornerRadius" ,Description = "获取或设置显示框和下拉框的圆角大小。",DefaultValue = "0" },
                new DataSourceModel() { Name = "DeleteMode" ,Type = "DeleteModes枚举" ,Description = "获取或设置当子项目可删除时，用户点击删除按钮后的操作。【可选项：Delete、EventOnly】",DefaultValue = "Delete" },
                new DataSourceModel() { Name = "BindingItems" ,Type = "IList<PUComboBoxItemModel>" ,Description = "若使用MVVM绑定，请使用此依赖属性。",DefaultValue = "NULL" },
                new DataSourceModel() { Name = "SearchMode" ,Type = "SearchModes枚举" ,Description = "获取或设置搜索模式。【可选项：None、TextChanged、Enter】",DefaultValue = "None" },
                new DataSourceModel() { Name = "SelectedValuePath" ,Type = "SelectedValuePaths枚举" ,Description = "获取或设置当子项目被选中时，SelectedValue应呈现子项目的哪一个值。【可选项：Header、Value】",DefaultValue = "None" },
            };

            DependencyPropertyList2 = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "CanDelete" ,Type = "Boolean" ,Description = "获取或设置是否显示删除按钮。",DefaultValue = "False" },
                new DataSourceModel() { Name = "Value" ,Type = "Object" ,Description = "获取或设置用以标记该项目的值（类似于Tag属性）。",DefaultValue = "#DDDDDD" },
            };

            AnnotationList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "控件可能会在设计器中突然变透明" ,Description = "目前尚未发现是何种原因导致设计器渲染时出现了问题，但控件依旧能如期运行。" },
                new DataSourceModel() { Name = "已禁用ItemsSource属性" ,Description = "ItemsSource属性不能自动生成PUComboBoxItem，因此隐藏了Set方法（为了防止误用）。若要使用MVVM绑定，请使用BindingItems属性。该属性已实现双向绑定，当BindingItems属性发生改变时，ComboBox的Items将同步发生变化；反之亦然。请根据需要来选择是否设置Mode=TwoWay。\n此外，PUComboBoxItemModel在初始化时会为其Uid属性（只读的）生成一个值；BindingItem在生成子控件时，会将该Model中的Uid值赋给它所对应子控件的Uid属性。这意味着你可以通过该Model的Uid属性来查找真实的控件。" },
                new DataSourceModel() { Name = "如果你想在删除前进行验证" ,Description = "DeleteMode提供了两种方式来处理删除事件。Delete选项表示当用户点击删除按钮时，应当立即删除项目并触发DeleteItem路由事件；而EventOnly选项则只会触发DeleteItem路由事件，不会删除项目。你可以监听该事件的发生，并判断是否应该删除该项目。" },
                new DataSourceModel() { Name = "关于SelectedValuePath" ,Description = "PanuonUI中的大多数组合型容器控件都重写了此属性。重写后的SelectedValuePath只有两个选项：Header和Value。Header选项表示SelectedValue属性应当呈现被选择项的Content属性，而Value选项则表示SelectedValue属性应呈现被选择项的Value属性。" },
                new DataSourceModel() { Name = "关于SelectedValue" ,Description = "当选中项发生改变时，该属性的值会依据SelectedValuePath的设定发生变化。同时，你可以通过对该属性赋值来搜索并选中项目（至于是通过子项的Content还是Value进行搜索，这同样取决于SelectedValuePath的值）。请注意，搜索时使用的是Equal方法，这意味着1和“1”是完全不同的（错误赋值可能会导致搜索不到子项）。" },
            };
        }

        private void SetMaskCover(bool toOpen)
        {
            var parent = Parent as ShellWindowViewModel;
            if (toOpen)
                parent.ShowCoverMask();
            else
                parent.CloseCoverMask();
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
