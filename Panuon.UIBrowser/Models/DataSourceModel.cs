/*==============================================================
*作者：ZEOUN
*时间：2018/11/15 13:16:48
*说明： 用于显示属性描述。
*日志：2018/11/15 13:16:48 创建。
*==============================================================*/
using System.ComponentModel;

namespace Panuon.UIBrowser.Models
{
    public class DataSourceModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyOfPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 名称。
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyOfPropertyChange("Name"); }
        }
        private string _name;


        /// <summary>
        /// 属性类型。
        /// </summary>
        public string Type
        {
            get { return _type; }
            set { _type = value; NotifyOfPropertyChange("Type"); }
        }
        private string _type;

        /// <summary>
        /// 描述。
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; NotifyOfPropertyChange("Description"); }
        }
        private string _description;

        /// <summary>
        /// 默认值。
        /// </summary>
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; NotifyOfPropertyChange("DefaultValue"); }
        }
        private string _defaultValue;
    }
}
