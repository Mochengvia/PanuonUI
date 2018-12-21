using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Panuon.UI
{
    /// <summary>
    /// 用于TreeView绑定的模型。
    /// </summary>

    public class PUTreeViewItemModel : INotifyPropertyChanged
    {
        protected internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #region Constructor
        public PUTreeViewItemModel()
        {
            Uid = Guid.NewGuid().ToString("N");
            Padding = new Thickness(10, 0, 0, 0);
        }
        #endregion

        #region Property
        /// <summary>
        /// 要显示的名称。可以作为SelectValuePath的值。
        /// </summary>
        public string Header
        {
            get { return _header; }
            set
            {
                _header = value; OnPropertyChanged("Header");
            }
        }
        private string _header = "";

        /// <summary>
        /// 该对象的值。可以作为SelectValuePath的值。
        /// <para>若Value不是值类型，使用Value作为匹配时会逐一比较每一个可写属性的值（参见PanuonUI.Utils扩展方法IsEqual）。</para>
        /// </summary>
        public object Value
        {
            get { return _value; }
            set
            {
                _value = value; OnPropertyChanged("Value");
            }
        }
        private object _value;


        /// <summary>
        /// 该项的子项目。
        /// </summary>
        public List<PUTreeViewItemModel> Items
        {
            get { return _items; }
            set
            {
                _items = value; OnPropertyChanged("Items");
            }
        }
        private List<PUTreeViewItemModel> _items;

        /// <summary>
        /// 缩进。默认值为10,0,0,0。
        /// </summary>
        public Thickness Padding
        {
            get { return _padding; }
            set
            {
                _padding = value; OnPropertyChanged("Padding");
            }
        }
        private Thickness _padding;

        /// <summary>
        /// 悬浮时显示的内容。
        /// </summary>
        public object ToolTip
        {
            get { return _toolTip; }
            set
            {
                _toolTip = value; OnPropertyChanged("ToolTip");
            }
        }
        private object _toolTip;
        #endregion

        #region Internal Property
        internal string Uid
        {
            get { return _uid; }
            set { _uid = value; }
        }
        private string _uid;


        #endregion

    }

}
