using System;
using System.ComponentModel;

namespace Panuon.UI
{
    public class PUTabItemModel : INotifyPropertyChanged
    {
        protected internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #region Constructor
        public PUTabItemModel()
        {
            Uid = Guid.NewGuid().ToString("N");
        }
        #endregion

        #region Property
        /// <summary>
        /// 要显示的名称。可以作为SelectValuePath的值。
        /// </summary>
        public object Header
        {
            get { return _header; }
            set { _header = value; OnPropertyChanged("Header"); }
        }
        private object _header = "";

        /// <summary>
        /// 该对象的值。可以作为SelectValuePath的值。
        /// <para>若Value不是值类型，使用Value作为匹配时会逐一比较每一个可写属性的值（参见PanuonUI.Utils扩展方法IsEqual）。</para>
        /// </summary>
        public object Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged("Value"); }
        }
        private object _value;

        /// <summary>
        /// 显示在标题前的图标，为空时不显示。
        /// </summary>
        public object Icon
        {
            get { return _icon; }
            set { _icon = value; OnPropertyChanged("Icon"); }
        }
        private object _icon;

        /// <summary>
        /// 是否显示删除按钮。
        /// </summary>
        public bool CanDelete
        {
            get { return _canDelete; }
            set { _canDelete = value; OnPropertyChanged("CanDelete"); }
        }
        private bool _canDelete = false;


        /// <summary>
        /// TabItem的内容。
        /// </summary>
        public object Content
        {
            get { return _content; }
            set { _content = value; OnPropertyChanged("Content"); }
        }
        private object _content = 0;

        /// <summary>
        /// 高度，默认值为30。
        /// </summary>
        public double Height
        {
            get { return _height; }
            set { _height = value; OnPropertyChanged("Height"); }
        }
        private double _height = 30;
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
