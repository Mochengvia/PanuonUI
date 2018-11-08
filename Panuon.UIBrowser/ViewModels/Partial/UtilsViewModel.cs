/*==============================================================
*作者：ZEOUN
*时间：2018/11/5 17:49:13
*说明： 
*日志：2018/11/5 17:49:13 创建。
*==============================================================*/
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Panuon.UI;
using Panuon.UI.Utils;
using System.Threading.Tasks;
using System.Threading;
using Caliburn.Micro;
using System.Collections.Concurrent;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class UtilsViewModel : Screen,IShell
    {
        private PUListBox _listbox;
        private ConcurrentBag<CancellationTokenSource> _cancellatonBag;

        public UtilsViewModel()
        {
            _cancellatonBag = new ConcurrentBag<CancellationTokenSource>();
            MaxTaskQuantity = 1;
        }

        #region Binding
        public int  MaxTaskQuantity
        {
            get { return _maxTaskQuantity; }
            set
            {
                _maxTaskQuantity = value;
                TaskPoll.MaxTaskQuantity = value;
                NotifyOfPropertyChange(() => MaxTaskQuantity);
            }
        }
        private int _maxTaskQuantity;
        #endregion

        #region Event
        public void ListBoxLoaded(object sender)
        {
             _listbox = sender as PUListBox;
        }

        public void Clear()
        {
            foreach(var cts in _cancellatonBag)
            {
                cts.Cancel();
            }
            _cancellatonBag = new ConcurrentBag<CancellationTokenSource>();
            _listbox.Items.Clear();
        }

        public void AddTask()
        {
            
            var cts = new CancellationTokenSource();

            var pgb = new PUProgressBar() { Height = 30, Width = 200 ,IsPercentShow = true};
            var task = new Task(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    if (cts.Token.IsCancellationRequested)
                        return;
                    App.Current.Dispatcher.Invoke(new System.Action(() =>
                    {
                        pgb.Percent += 0.1;
                    }));
                    Thread.Sleep(1000);
                }
            }, cts.Token);
            _listbox.Items.Add(new PUListBoxItem()
            {
                Height = 40,
                VerticalContentAlignment = VerticalAlignment.Center,
                Content = pgb,
            });
            _cancellatonBag.Add(cts);
            TaskPoll.StartNew(task);
        }
        #endregion
    }
}
