using Caliburn.Micro;
using Panuon.UI;
using Panuon.UI.Utils;
using Panuon.UIBrowser.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Panuon.UIBrowser.ViewModels.Control
{
    public class TreeViewViewModel : Screen, IShell
    {
        #region Identity
        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public TreeViewViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            TreeViewStyle = TreeViewStyles.General;
            ExpandMode = ExpandModes.Click;
            ChoosedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DDDDDD"));
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#696969"));
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEEEEE"));
            Init();
            ChoosedValue = "1-1";
        }
        #endregion

        #region Bindings
        public ObservableCollection<PUTreeViewItemModel> TreeViewItems
        {
            get { return _treeViewItems; }
            set { _treeViewItems = value; NotifyOfPropertyChange(() => TreeViewItems); }
        }
        private ObservableCollection<PUTreeViewItemModel> _treeViewItems;

        public ObservableCollection<DataSourceModel> DependencyPropertyList
        {
            get { return _dependencyPropertyList; }
            set { _dependencyPropertyList = value; NotifyOfPropertyChange(() => DependencyPropertyList); }
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
            set { _annotationList = value; NotifyOfPropertyChange(() => AnnotationList); }
        }
        private ObservableCollection<DataSourceModel> _annotationList;

        public TreeViewStyles TreeViewStyle
        {
            get { return _treeViewStyle; }
            set { _treeViewStyle = value; NotifyOfPropertyChange(() => TreeViewStyle); }
        }
        private TreeViewStyles _treeViewStyle;

        public Brush ChoosedBrush
        {
            get { return _choosedBrush; }
            set { _choosedBrush = value; NotifyOfPropertyChange(() => ChoosedBrush); }
        }
        private Brush _choosedBrush;

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

        public string ChoosedValue
        {
            get { return _choosedValue; }
            set { _choosedValue = value; NotifyOfPropertyChange(() => ChoosedValue); }
        }
        private string _choosedValue;

        
        public ExpandModes ExpandMode
        {
            get { return _expandMode; }
            set { _expandMode = value; NotifyOfPropertyChange(() => ExpandMode); }
        }
        private ExpandModes _expandMode;
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
            TreeViewItems.Add(new PUTreeViewItemModel()
            {
                Header = "第" + (TreeViewItems.Count + 1) + "部分",
                Value = (TreeViewItems.Count + 1).ToString(),
                Items = new List<PUTreeViewItemModel>()
                {
                    new PUTreeViewItemModel() { Header = "Value : " + (TreeViewItems.Count + 1) + "-1" , Value =  (TreeViewItems.Count + 1) + "-1", },
                    new PUTreeViewItemModel() { Header = "Value : " + (TreeViewItems.Count + 1) + "-2", Value = (TreeViewItems.Count + 1) + "-2" , },
                }
            });
        }

        public void RemoveItem()
        {
            if (TreeViewItems.Count > 0)
                TreeViewItems.RemoveAt(TreeViewItems.Count - 1);
        }

        public void SelectedBrushChanged(string content)
        {
            ChoosedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void CoverBrushChanged(string content)
        {
            CoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void ForegroundChanged(string content)
        {
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(content));
        }

        public void StyleChanged(string content)
        {
            TreeViewStyle = (TreeViewStyles)Enum.Parse(typeof(TreeViewStyles), content);
        }

        public void ExpandModeChanged(string content)
        {
            ExpandMode = (ExpandModes)Enum.Parse(typeof(ExpandModes), content);
        }

        #endregion

        #region Function
        private async void Init()
        {
            await Task.Delay(100);

            TreeViewItems = new BindableCollection<PUTreeViewItemModel>()
            {
                new PUTreeViewItemModel()
                {
                    Header = "第1部分",
                    Value = "1",
                    Items = new List<PUTreeViewItemModel>()
                    {
                        new PUTreeViewItemModel() { Header = "Value : 1-1" , Value = "1-1", },
                        new PUTreeViewItemModel() { Header = "Value : 1-2", Value = "1-2" , },
                    }
                },
              new PUTreeViewItemModel()
                {
                    Header = "第2部分",
                    Value = "2",
                    Items = new List<PUTreeViewItemModel>()
                    {
                        new PUTreeViewItemModel() { Header = "Value : 2-1" , Value = "2-1", },
                        new PUTreeViewItemModel() { Header = "Value : 2-2", Value = "2-2" , },
                    }
                },
                new PUTreeViewItemModel()
                {
                    Header = "第3部分",
                    Value = "3",
                    Items = new List<PUTreeViewItemModel>()
                    {
                        new PUTreeViewItemModel() { Header = "Value : 3-1" , Value = "3-1", },
                        new PUTreeViewItemModel() { Header = "Value : 3-2", Value = "3-2" , },
                    }
                },
            };

            DependencyPropertyList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "TreeViewStyle" ,Type = "TreeViewStyles枚举类型" ,Description = "获取或设置树视图的基本样式。【可选值：General、Classic】",DefaultValue = "General" },
                new DataSourceModel() { Name = "InnerHeight" ,Type = "Double" ,Description = "获取或设置子项目的单行元素高度。",DefaultValue = "40" },
                new DataSourceModel() { Name = "ChoosedItem" ,Type = "PUTreeViewItem" ,Description = "获取被选中的元素。只读。",DefaultValue = "#3E3E3E" },
                new DataSourceModel() { Name = "ChoosedBrush" ,Type = "Brush" ,Description = "获取或设置当某个子项被选中时的前景色。",DefaultValue = "#3E3E3E" },
                new DataSourceModel() { Name = "CoverBrush" ,Type = "Brush" ,Description = "获取或设置鼠标悬浮时遮罩层的背景颜色。",DefaultValue = "0" },
                new DataSourceModel() { Name = "ChoosedValuePath" ,Type = "SelectedValuePaths枚举" ,Description = "该属性指定了当子项目被选中时，SelectedValue应呈现子项目的哪一个值。【可选值：Header、Value】",DefaultValue = "Header" },
                new DataSourceModel() { Name = "ChoosedValue" ,Type = "Object" ,Description = "获取被选中PUTabItem的Header或Value属性（这取决于SelectedValuePath），或反向选中子项目。",DefaultValue = "Null" },
                new DataSourceModel() { Name = "ExpandMode" ,Type = "ExpandModes枚举类型" ,Description = "获取或设置是否需要展开父项的方式。【可选值：Click、DoubleClick】",DefaultValue = "Click" },
                new DataSourceModel() { Name = "BindingItems" ,Type = "ObservableCollection<PUTabItemModel>" ,Description = "若使用MVVM绑定，请使用此依赖属性。详见注解”已禁用ItemSource属性“。",DefaultValue = "Null" },
            };
            DependencyPropertyList2 = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "IsChoosed" ,Type = "Boolean" ,Description = "获取或设置该子项是否已被选中，含有子项目的行项目无法被选中。",DefaultValue = "General" },
                new DataSourceModel() { Name = "Value" ,Type = "Object" ,Description = "获取或设置该子项可以携带的值，不会对前端显示造成影响。",DefaultValue = "Null" },
            };
            AnnotationList = new ObservableCollection<DataSourceModel>()
            {
                new DataSourceModel() { Name = "不要使用Select系列属性" ,Description = "由于原生TreeView中带有子项的行项目也能被选中，因此新建了ChoosedItem来区别它。IsSelected、SelectedValuePath、SelectedValue属性已被隐藏Set方法，请改用IsChoosed、ChoosedValuePath和ChoosedValue属性。" },
                new DataSourceModel() { Name = "已禁用ItemsSource属性" ,Description = "ItemsSource属性不能自动生成PUTreeViewItem，因此隐藏了Set方法（为了防止误用）。若要使用MVVM绑定，请使用BindingItems属性。该属性已实现双向绑定，当BindingItems属性发生改变时，TreeView的Items将同步发生变化；反之亦然。请根据需要来选择是否设置Mode=TwoWay。\n此外，PUTreeViewItemModel在初始化时会为其Uid属性（只读的）生成一个值；BindingItem在生成子控件时，会将该Model中的Uid值赋给它所对应子控件的Uid属性。这意味着你可以通过该Model的Uid属性来查找真实的控件。" },
                new DataSourceModel() { Name = "关于ChoosedValuePath" ,Description = "ChoosedValuePath只有两个选项：Header和Value。Header选项表示ChoosedValue属性应当呈现被选择项的Header属性，而Value选项则表示ChoosedValue属性应呈现被选择项的Value属性。" },
                new DataSourceModel() { Name = "关于ChoosedValue" ,Description = "当选中项发生改变时，该属性的值会依据ChoosedValuePath的设定发生变化。同时，你可以通过对该属性赋值来搜索并选中项目（至于是通过子项的Header还是Value进行搜索，这同样取决于ChoosedValuePath的值）。请注意，搜索时使用的是Equal方法，这意味着1和“1”是完全不同的（错误赋值可能会导致搜索不到子项）。" },
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
