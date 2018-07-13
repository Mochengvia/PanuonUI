using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PUTreeViewItem : TreeViewItem
    {
        #region Identity
        private PUTreeView ParentTreeView
        {
            get
            {
                if (_parentTreeView == null)
                {
                    var parent = this.Parent;
                    while (parent != null && parent.GetType() != typeof(PUTreeView))
                    {
                        parent = (parent as PUTreeViewItem).Parent;
                    }
                    _parentTreeView = (parent as PUTreeView);
                }
                return _parentTreeView;
            }
        }
        private PUTreeView _parentTreeView;
        #endregion

        #region Property
        /// <summary>
        ///  边角的圆滑程度，默认值为0。
        /// </summary>
        public int BorderCornerRadius
        {
            get { return (int)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register("BorderCornerRadius", typeof(int), typeof(PUTreeViewItem), new PropertyMetadata(0));


        /// <summary>
        /// 行元素高度，默认值为40。
        /// </summary>
        public double InnerHeight
        {
            get { return (double)GetValue(InnerHeightProperty); }
            set { SetValue(InnerHeightProperty, value); }
        }
        public static readonly DependencyProperty InnerHeightProperty = DependencyProperty.Register("InnerHeight", typeof(double), typeof(PUTreeViewItem), new PropertyMetadata((double)40));

        /// <summary>
        /// 鼠标悬浮时遮罩层的背景颜色，默认值为#AA666666。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }
        public static readonly DependencyProperty CoverBrushProperty = DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUTreeViewItem), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#22666666"))));

        /// <summary>
        /// 该元素是否被选择（即使上级子元素没有展开）。使用此属性而非IsSelected
        /// </summary>
        public bool IsChoosed
        {
            get { return (bool)GetValue(IsChoosedProperty); }
            set
            {
                SetValue(IsChoosedProperty, value);
                if (value == true)
                {
                    if (ParentTreeView.ChoosedItem != null)
                        ParentTreeView.ChoosedItem.IsChoosed = false;
                    ParentTreeView.ChoosedItem = this;
                }
            }
        }
        public static readonly DependencyProperty IsChoosedProperty = DependencyProperty.Register("IsChoosed", typeof(bool), typeof(PUTreeViewItem), new PropertyMetadata(false));

        /// <summary>
        /// Icon
        /// </summary>
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(string), typeof(PUTreeViewItem), new PropertyMetadata(""));
        #endregion

        static PUTreeViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUTreeViewItem), new FrameworkPropertyMetadata(typeof(PUTreeViewItem)));
        }

        public override void OnApplyTemplate()
        {
            //初始选中时，将treeview的ChoosedItem设置为此
            if (IsChoosed)
            {
                var parent = this.Parent;
                while (parent != null && parent.GetType() != typeof(PUTreeView))
                {
                    parent = (parent as PUTreeViewItem).Parent;
                }
                var view = (parent as PUTreeView);
                view.ChoosedItem = this;
            }

            var stk = VisualTreeHelper.GetChild(this, 0) as StackPanel;
            stk.MouseLeftButtonDown += Stk_MouseLeftButtonDown;
        }

        private void Stk_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (!HasItems)
            {
                IsSelected = true;

                if (IsChoosed == false)
                {
                    IsChoosed = true;
                }
                else
                {
                    e.Handled = true;
                    return;
                }
                ParentTreeView.OnChoosedItemChanged();
            }
            else
            {
                IsSelected = false;
                if (!ParentTreeView.IsExpandDoubleClick)
                    IsExpanded = !IsExpanded;
            }
        }


    }
}
