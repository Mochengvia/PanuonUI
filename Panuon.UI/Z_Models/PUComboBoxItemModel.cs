using System;
using System.ComponentModel;

namespace Panuon.UI
{
    public class PUComboBoxItemModel : INotifyPropertyChanged
    {
        protected internal virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #region Constructor
        public PUComboBoxItemModel()
        {
            Uid = Guid.NewGuid().ToString("N");
        }
        #endregion

        #region Property
        /// <summary>
        /// 要显示的名称。可以作为SelectValuePath的值。
        /// </summary>
        public string Header
        {
            get { return _header; }
            set { _header = value; OnPropertyChanged("Header"); }
        }
        private string _header = "";

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
        /// 是否显示删除按钮。
        /// </summary>
        public bool CanDelete
        {
            get { return _canDelete; }
            set { _canDelete = value; OnPropertyChanged("CanDelete"); }
        }
        private bool _canDelete = false;

        /// <summary>
        /// 生成该对象时，自动生成的唯一ID。该属性 与该对象生成的PUComboBoxItem的Uid属性值相等。
        /// </summary>
        public string Uid
        {
            get { return _uid; }
            private set { _uid = value; }
        }
        private string _uid;
        #endregion
    }
}
