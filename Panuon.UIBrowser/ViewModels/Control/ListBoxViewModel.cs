using Caliburn.Micro;
using Panuon.UI;
using Panuon.UIBrowser.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Panuon.UIBrowser.ViewModels.Control
{
    public class ListBoxViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;

        private PUListBox _listBox;
        #endregion

        #region Constructor
        public ListBoxViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            SelectedValue = 1;
            SelectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E6E6E6"));
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#22E6E6E6"));
            SearchBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#44444444"));
            Init();
        }
        #endregion

        #region Bindings
        public ObservableCollection<PUListBoxItemModel> ListBoxItems
        {
            get { return _listBoxItems; }
            set { _listBoxItems = value; NotifyOfPropertyChange(() => ListBoxItems); }
        }
        private ObservableCollection<PUListBoxItemModel> _listBoxItems;

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

        public ObservableCollection<DataSourceModel> APIList
        {
            get { return _apiList; }
            set { _apiList = value; NotifyOfPropertyChange(() => APIList); }
        }
        private ObservableCollection<DataSourceModel> _apiList;

        public ObservableCollection<DataSourceModel> EventList2
        {
            get { return _eventList2; }
            set { _eventList2 = value; NotifyOfPropertyChange(() => EventList2); }
        }
        private ObservableCollection<DataSourceModel> _eventList2;

        public Brush SelectedBrush
        {
            get { return _selectedBrush; }
            set { _selectedBrush = value; NotifyOfPropertyChange(() => SelectedBrush); }
        }
        private Brush _selectedBrush;

        public Brush SearchBrush
        {
            get { return _searchBrush; }
            set { _searchBrush = value; NotifyOfPropertyChange(() => SearchBrush); }
        }
        private Brush _searchBrush;

        public Brush CoverBrush
        {
            get { return _coverBrush; }
            set { _coverBrush = value; NotifyOfPropertyChange(() => CoverBrush); }
        }
        private Brush _coverBrush;

        public int SelectedValue
        {
            get { return _selectedValue; }
            set { _selectedValue = value; NotifyOfPropertyChange(() => SelectedValue); }
        }
        private int _selectedValue;

        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; NotifyOfPropertyChange(() => SearchText); }
        }
        private string _searchText;
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

        public void ListBoxLoaded(object sender)
        {
            _listBox = sender as PUListBox;
        }

        public void Search()
        {
            if (_listBox == null)
                throw new Exception("未知异常：ListBox为Null。");

            SearchText = SearchText ?? "";

            _listBox.SearchItemByContent(SearchText, true);
        }

        public void AddItem()
        {
            ListBoxItems.Add(new PUListBoxItemModel()
            {
                Header = "Line" + (ListBoxItems.Count + 1),
                Value = (ListBoxItems.Count + 1),
            });
        }

        public void RemoveItem()
        {
            if (ListBoxItems.Count > 0)
                ListBoxItems.RemoveAt(ListBoxItems.Count - 1);
        }

        public void SelectedBrushChanged(string content)
        {
            SelectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void CoverBrushChanged(string content)
        {
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void SearchBrushChanged(string content)
        {
            SearchBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }
        #endregion

        #region Function
        private async void Init()
        {
            await Task.Delay(100);

            ListBoxItems = new ObservableCollection<PUListBoxItemModel>()
            {
                new PUListBoxItemModel() {  Header = "Line1", Value =1 },
                new PUListBoxItemModel() {  Header = "Line2", Value =2 },
                new PUListBoxItemModel() {  Header = "Line3", Value =3 },
                new PUListBoxItemModel() {  Header = "Line4", Value =4 },
                new PUListBoxItemModel() {  Header = "Line5", Value =5 },
            };

            DependencyPropertyList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "CoverBrush" ,Type = "Brush" ,Description = "获取或设置当鼠标悬浮时ListBoxItem的背景色。",DefaultValue = "#EEEEEE" },
                new DataSourceModel() { Name = "SelectedBrush" ,Type = "Brush" ,Description = "获取或设置当ListBoxItem被选中时的背景色。",DefaultValue = "#DDDDDD" },
                new DataSourceModel() { Name = "SearchBrush" ,Type = "Brush" ,Description = "获取或设置当搜索ListBoxItem时，ListBoxItem被找到时应呈现的背景色。",DefaultValue = "#EEEEEE" },
                new DataSourceModel() { Name = "SelectedValuePath" ,Type = "SelectedValuePaths枚举" ,Description = "该属性指定了当子项目被选中时，SelectedValue应呈现子项目的哪一个值。【可选值：Header（其实对应的是Content属性）、Value】",DefaultValue = "Header" },
                new DataSourceModel() { Name = "SelectedValue" ,Type = "Object" ,Description = "获取被选中PUTabItem的Header（即Content属性）或Value属性（这取决于SelectedValuePath），或反向选中子项目。",DefaultValue = "Null" },
                new DataSourceModel() { Name = "BindingItems" ,Type = "ObservableCollection<PUListBoxItemModel>" ,Description = "若使用MVVM绑定，请使用此依赖属性。详见注解”已禁用ItemSource属性“。",DefaultValue = "Null" },
            };

            DependencyPropertyList2 = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "Value" ,Type = "Object" ,Description = "获取或设置用以标记该项目的值（类似于Tag属性）。",DefaultValue = "#DDDDDD" },
            };
            APIList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "GetListBoxItemByContent(object content)" ,Description = "通过内容获取符合条件的第一个子项。" },
                new DataSourceModel() { Name = "GetListBoxItemByValue(object value)" ,Description = "通过Value获取符合条件的第一个子项。"},
                new DataSourceModel() { Name = "GetListBoxItemByUid(string uid)" ,Description = "通过Uid获取符合条件的第一个子项。"},
                new DataSourceModel() { Name = "SearchItemByContent(string content, bool allowFuzzySearch = true)" ,Description = "通过内容查询符合条件的第一个子项，滚动到该项目并高亮（子项的内容须为string类型）。" },
                new DataSourceModel() { Name = "SearchItemByValue(object value)" ,Description = "通过Value查询符合条件的第一个子项，滚动到该项目并高亮。" },
                new DataSourceModel() { Name = "SearchItemByUid(string uid)" ,Description = "通过Uid查询符合条件的第一个子项，滚动到该项目并高亮。" },
            };
            EventList2 = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "Searched" ,Description = "当子项被搜索到时，触发此事件。" },
            };
            AnnotationList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "已禁用ItemsSource属性" ,Description = "ItemsSource属性不能自动生成PUListBoxItem，因此隐藏了Set方法（为了防止误用）。若要使用MVVM绑定，请使用BindingItems属性。该属性已实现双向绑定，当BindingItems属性发生改变时，ListBox的Items将同步发生变化；反之亦然。请根据需要来选择是否设置Mode=TwoWay。\n此外，PUListBoxItemModel在初始化时会为其Uid属性（只读的）生成一个值；BindingItem在生成子控件时，会将该Model中的Uid值赋给它所对应子控件的Uid属性。这意味着你可以通过该Model的Uid属性来查找真实的控件。" },
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
