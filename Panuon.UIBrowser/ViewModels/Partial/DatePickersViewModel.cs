using Caliburn.Micro;
using Panuon.UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class DatePickersViewModel : Screen, IShell
    {
        public DatePickersViewModel()
        {
            SelectedDateTimeString = ((DateTime)SelectedDateTime).ToString("yyyy-MM-dd HH:mm:ss");
        }

        #region Bindings
        public DateTime SelectedDateTime
        {
            get { return _selectedDate; }
            set {
                _selectedDate = value;
                SelectedDateTimeString = ((DateTime)SelectedDateTime).ToString("yyyy-MM-dd HH:mm:ss");
                NotifyOfPropertyChange(() => SelectedDateTime); }
        }
        private DateTime _selectedDate = DateTime.Now.ToDateOnly();

        public DateTime? MaxDateTime
        {
            get { return _maxDateTime; }
            set { _maxDateTime = value; NotifyOfPropertyChange(() => MaxDateTime); }
        }
        private DateTime? _maxDateTime;

        public DateTime? MinDateTime
        {
            get { return _minDateTime; }
            set { _minDateTime = value; NotifyOfPropertyChange(() => MinDateTime); }
        }
        private DateTime? _minDateTime;

        public string SelectedDateTimeString
        {
            get { return _selectedDateString; }
            set { _selectedDateString = value; NotifyOfPropertyChange(() => SelectedDateTimeString); }
        }
        private string _selectedDateString;

        public bool LimitMaxDateIsChecked
        {
            get { return _limitMaxDateIsChecked; }
            set { _limitMaxDateIsChecked = value; NotifyOfPropertyChange(() => LimitMaxDateIsChecked); }
        }
        private bool _limitMaxDateIsChecked;

        public bool LimitMinDateIsChecked
        {
            get { return _limitMinDateIsChecked; }
            set { _limitMinDateIsChecked = value; NotifyOfPropertyChange(() => LimitMinDateIsChecked); }
        }
        private bool _limitMinDateIsChecked;
        #endregion

        #region Event
        public void LimitMaxDate(bool toLimit)
        {
            if (toLimit)
            {
                MaxDateTime = DateTime.Now.AddMonths(1);
            }
            else
            {
                MaxDateTime = null;
            }
        }

        public void LimitMinDate(bool toLimit)
        {
            if (toLimit)
            {
                MinDateTime = DateTime.Now.AddMonths(-1);
            }
            else
            {
                MinDateTime = null;
            }
        }

        public void DateTimeInputChanged()
        {
            if (String.IsNullOrEmpty(SelectedDateTimeString))
            {
                return;
            }
            DateTime date;
            if(!DateTime.TryParse(SelectedDateTimeString,out date))
            {
                SelectedDateTime = DateTime.Now.ToDateOnly();
                return;
            }
            SelectedDateTime = date;
        }
        #endregion
    }
}
