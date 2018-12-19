using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Panuon.UI
{
    /// <summary>
    /// 用于ListBox绑定的模型。
    /// </summary>

    public class PUListBoxItemModel : INotifyPropertyChanged
    {
        protected internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #region Constructor
        public PUListBoxItemModel()
        {
            Uid = Guid.NewGuid().ToString("N");
        }
        #endregion

        #region Property
        /// <summary>
        /// 要显示的内容。可以作为SelectValuePath的值。用于设置ListBoxItem的Content属性。
        /// </summary>
        public object Header
        {
            get { return _header; }
            set
            {
                _header = value; OnPropertyChanged("Header");
            }
        }
        private object _header = "";

        /// <summary>
        /// 该对象的值。可以作为SelectValuePath的值。必须是数字、字符串或布尔值，其他类型可能会导致选择内容出现错误。
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
