using Caliburn.Micro;
using Panuon.UI.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class LineChartsViewModel : Screen, IShell
    {
        private int _currentQuantity = 6;
        private int _currentMode = 0;
        private int _currentColor = 0;
        public LineChartsViewModel()
        {
            XAxis = new string[] { "1", "2", "3", "4", "5", "6" };
            YAxis = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            Points = new ObservableCollection<PUChartPoint>()
            {
                new PUChartPoint() { Value = 0.1, ValueTip = "1" },
                new PUChartPoint() { Value = 0.2, ValueTip = "2" },
                new PUChartPoint() { Value = 0.3, ValueTip = "3" },
                new PUChartPoint() { Value = 0.4, ValueTip = "4" },
                new PUChartPoint() { Value = 0.5, ValueTip = "5" },
                new PUChartPoint() { Value = 0.6, ValueTip = "6" },
            };
        }

        #region Bindings
        public string[] XAxis
        {
            get { return _xAxis; }
            set { _xAxis = value; NotifyOfPropertyChange(() => XAxis); }
        }
        private string[] _xAxis;

        public string[] YAxis
        {
            get { return _yAxis; }
            set { _yAxis = value; NotifyOfPropertyChange(() => YAxis); }
        }
        private string[] _yAxis;

        public ObservableCollection<PUChartPoint> Points
        {
            get { return _points; }
            set { _points = value; NotifyOfPropertyChange(() => Points); }
        }
        private ObservableCollection<PUChartPoint> _points;


        public Brush AreaBrush
        {
            get { return _areaBrush; }
            set { _areaBrush = value; NotifyOfPropertyChange(() => AreaBrush); }
        }
        private Brush _areaBrush = new LinearGradientBrush(new GradientStopCollection() { new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#AAAAAAAA"), Offset = 0 }, new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#22AAAAAA"), Offset = 1 } }, 90);

        public Brush GridBrush
        {
            get { return _gridBrush; }
            set { _gridBrush = value; NotifyOfPropertyChange(() => GridBrush); }
        }
        private Brush _gridBrush = new SolidColorBrush(Colors.LightGray);

        public Brush LineBrush
        {
            get { return _lineBrush; }
            set { _lineBrush = value; NotifyOfPropertyChange(() => LineBrush); }
        }
        private Brush _lineBrush = new SolidColorBrush(Colors.Gray);

        public bool UsingAnimation
        {
            get { return _usingAnimation; }
            set { _usingAnimation = value; NotifyOfPropertyChange(() => UsingAnimation); }
        }
        private bool _usingAnimation = true;


        #endregion


        #region Event
        public void RandomValue()
        {
            if (_currentMode == 0)
            {
                var list = new List<PUChartPoint>();
                for (int i = 0; i < _currentQuantity; i++)
                {
                    var value = GetRandomDecimal1();
                    list.Add(new PUChartPoint()
                    {
                        Value = value,
                        ValueTip = (value * 10).ToString("f2"),
                    });
                }
                Points = new ObservableCollection<PUChartPoint>(list);
            }
            else if (_currentMode == 1)
            {
                var temp = new string[] { "等级一", "等级二", "等级三", "等级四", "等级五", "等级六" };
                var list = new List<PUChartPoint>();
                for (int i = 0; i < _currentQuantity; i++)
                {
                    var value = GetRandomDecimal2();
                    list.Add(new PUChartPoint()
                    {
                        Value = value,
                        ValueTip = temp[((int)(value * 5))],
                    });
                }
                Points = new ObservableCollection<PUChartPoint>(list);
            }

        }

        public void ChangeAxis()
        {
            if (_currentMode == 0)
            {
                _currentQuantity = 7;
                XAxis = new string[] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期天" };
                YAxis = new string[] { "等级一", "等级二", "等级三", "等级四", "等级五", "等级六" };
                _currentMode = 1;
                RandomValue();
            }
            else if (_currentMode == 1)
            {
                _currentQuantity = 6;
                XAxis = new string[] { "1", "2", "3", "4", "5", "6" };
                YAxis = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
                _currentMode = 0;
                RandomValue();
            }
        }

        public void AddValues()
        {
            if(_currentMode == 0)
            {
                _currentQuantity++;
                var list = XAxis.ToList();
                if (list.Count == 20)
                {
                    for (int i = 1; i < 20; i += 2)
                    {
                        list[i] = "";
                    }
                    list.Add(_currentQuantity.ToString());
                }
                else if(list.Count > 20)
                {
                    if (list.Count % 2 == 1)
                        list.Add("");
                    else
                        list.Add(_currentQuantity.ToString());
                }
                else
                    list.Add(_currentQuantity.ToString());

                XAxis = list.ToArray();

                var value = GetRandomDecimal1();
                var valuelist = Points.ToList();
                valuelist.Add(new PUChartPoint()
                {
                    Value = value,
                    ValueTip = (value * 10).ToString("f2"),
                });
               
                Points = new ObservableCollection<PUChartPoint>(valuelist);
            }
           
        }

        public void ChangeColor()
        {
            switch (_currentColor)
            {
                case 0:
                    GridBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AA49A9C0"));
                    AreaBrush = new LinearGradientBrush(new GradientStopCollection() { new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#AA49A9C0"), Offset = 0 }, new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#5549A9C0"), Offset = 1 } }, 90);
                    LineBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#49A9C0"));
                    _currentColor = 1;
                    break;
                case 1:
                    GridBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AAF4A758"));
                    AreaBrush = new LinearGradientBrush(new GradientStopCollection() { new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#AAF4A758"), Offset = 0 }, new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#55F4A758"), Offset = 1 } }, 90);
                    LineBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F4A758"));
                    _currentColor = 2;
                    break;
                case 2:
                    GridBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AAE089B8"));
                    AreaBrush = new LinearGradientBrush(new GradientStopCollection() { new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#AAE089B8"), Offset = 0 }, new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#55E089B8"), Offset = 1 } }, 90);
                    LineBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E089B8"));
                    _currentColor = 3;
                    break;
                case 3:
                    GridBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AAAAAAAA"));
                    AreaBrush = new LinearGradientBrush(new GradientStopCollection() { new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#AAAAAAAA"), Offset = 0 }, new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#55DDDDDD"), Offset = 1 } }, 90);
                    LineBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AAAAAA"));
                    _currentColor = 0;
                    break;
            }
        }

        #endregion

        #region Function
        Random _rand;
        private double GetRandomDecimal1()
        {
            if (_rand == null)
                _rand = new Random(DateTime.Now.Millisecond);
            return _rand.NextDouble();
        }

        private double GetRandomDecimal2()
        {
            var array = new double[6] { 0, 0.2, 0.4, 0.6, 0.8, 1 };
            if (_rand == null)
                _rand = new Random(DateTime.Now.Millisecond);
            return array[_rand.Next(0, 6)];
        }
        #endregion
    }
}
