using Caliburn.Micro;
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
            SetToday();
            SelectedDateString = ((DateTime)SelectedDate).ToString("yyyy-MM-dd HH:mm:ss");
        }

        #region Bindings
        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; NotifyOfPropertyChange(() => SelectedDate); }
        }
        private DateTime? _selectedDate;

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

        public string SelectedDateString
        {
            get { return _selectedDateString; }
            set { _selectedDateString = value; NotifyOfPropertyChange(() => SelectedDateString); }
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

        public void SetNow()
        {
            SelectedDate = DateTime.Now;
        }

        public void SetToday()
        {
            SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }

        public void DateTimeInputChanged()
        {
            if (String.IsNullOrEmpty(SelectedDateString))
            {
                return;
            }
            DateTime date;
            if(!DateTime.TryParse(SelectedDateString,out date))
            {
                SelectedDate = null;
                return;
            }
            SelectedDate = date;
        }
        #endregion
    }
}
