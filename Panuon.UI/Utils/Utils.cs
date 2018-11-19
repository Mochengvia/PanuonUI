using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Panuon.UI.Utils
{
    /// <summary>
    /// 提供Task的简易池化管理，并发任务数量可控。
    /// </summary>
    public class TaskPoll
    {
        #region Identity
        private static ConcurrentQueue<Task> _taskQueue;

        private static int _runningTaskQuantity;
        #endregion

        #region Property
        /// <summary>
        /// 获取或设置可以允许同时运行的最大任务数量。
        /// <para>若值为Null，则不限制最大执行数量。</para>
        /// </summary>
        public static int? MaxTaskQuantity
        {
            get { return _maxTaskQuantity; }
            set { _maxTaskQuantity = value; RecheckQueue(); }
        }
        private static int? _maxTaskQuantity;

        /// <summary>
        /// 获取当前任务池中的任务总数（包括排队中和正在执行的）。
        /// </summary>
        public static int CurrentTaskQuantity
        {
            get { return _taskQueue == null ? 0 : _taskQueue.Count; }
        }

        /// <summary>
        /// 获取当前正在执行的任务数量。
        /// </summary>
        public static int RunningTaskQuantity
        {
            get { return _runningTaskQuantity; }
        }

        #endregion

        #region APIs
        /// <summary>
        /// 使用默认值初始化TaskManager。首次调用StartNew方法时，TaskManager将自动执行初始化。
        /// </summary>
        public static void Init()
        {
            _taskQueue = new ConcurrentQueue<Task>();
            _runningTaskQuantity = 0;
        }

        /// <summary>
        /// 使用指定值初始化TaskManager。
        /// <param name="maxTaskQuantity">允许同时运行的最大任务数量。</param>
        /// </summary>
        public static void Init(int maxTaskQuantity)
        {
            MaxTaskQuantity = maxTaskQuantity;
            _taskQueue = new ConcurrentQueue<Task>();
            _runningTaskQuantity = 0;
        }

        /// <summary>
        /// 向任务队列中插入一个待执行的新任务，该任务将在合适的时机执行。
        /// </summary>
        public static void StartNew(Task task)
        {
            if (_taskQueue == null)
                Init();

            _taskQueue.Enqueue(task);
            RecheckQueue();
        }

        /// <summary>
        /// 向任务队列中插入一个待执行的新任务，该任务将在合适的时机执行。
        /// </summary>
        public static void StartNew<T>(Task<T> task)
        {
            if (_taskQueue == null)
                Init();

            _taskQueue.Enqueue(task);
            RecheckQueue();
        }

        #endregion

        #region Funtion
        private static void RecheckQueue()
        {
            if (_taskQueue == null)
                return;
            if (_taskQueue.Count == 0 || (MaxTaskQuantity != null && _runningTaskQuantity >= MaxTaskQuantity))
                return;
            Task task;
            if (!_taskQueue.TryDequeue(out task))
            {
                RecheckQueue();
                return;
            }
            if (task.Status != TaskStatus.Created && task.Status != TaskStatus.WaitingToRun)
            {
                RecheckQueue();
                return;
            }
            Interlocked.Increment(ref _runningTaskQuantity);
            task.ContinueWith((t) =>
            {
                Interlocked.Decrement(ref _runningTaskQuantity);
                RecheckQueue();
            });
            task.Start();
            if(_runningTaskQuantity < MaxTaskQuantity)
                RecheckQueue();
        }
        #endregion

    }

    /// <summary>
    /// 提供缓存管理（线程安全的），便于缓存数量逐步扩展。
    /// </summary>
    public class PUCache<T>
    {

        #region Identity
        /// <summary>
        /// 缓存集合。
        /// </summary>
        private static ConcurrentDictionary<string, T> Caches { get; set; }
        #endregion

        #region EventHandle
        /// <summary>
        /// 当FindAny方法提供的ID数组中有部分值不存在时，触发此事件。
        /// <para>事件参数(UID列表)类型：IList<string></para>
        /// </summary>
        public static EventHandler LackItems;
        #endregion

        #region Constructor
        static PUCache()
        {
            Caches = new ConcurrentDictionary<string, T>();
        }
        #endregion

        #region Property
        /// <summary>
        /// 获取当前缓存的数量。
        /// </summary>
        public static int Count
        {
            get { return Caches.Count; }
        }
        #endregion

        #region APIs
        /// <summary>
        /// 向缓存中添加数据。若ID已存在，则覆盖原有的值。
        /// </summary>
        /// <param name="uid">标识该对象的唯一ID。</param>
        /// <param name="obj">要添加的对象。</param>
        public static void Add(string uid, T obj)
        {
            Caches.AddOrUpdate(uid, obj, (key, oldValue) => obj);
        }

        /// <summary>
        /// 查找指定ID的值。若找不到，则返回默认值。
        /// </summary>
        /// <param name="uid">标识该对象的唯一ID。</param>
        /// <returns></returns>
        public static T Find(string uid)
        {
            var obj = Caches.FirstOrDefault(x => x.Key == uid);
            if (obj.Equals(default(KeyValuePair<string, T>)))
                return default(T);
            else
                return obj.Value;
        }

        /// <summary>
        /// 查找多个指定ID的值。不存在的ID不会存在于返回值列表中，并且会触发LackItems事件。
        /// </summary>
        /// <param name="uids">要查找的唯一ID数组。</param>
        /// <returns></returns>
        public static IList<KeyValuePair<string, T>> FindAny(params string[] uids)
        {
            var resultList = Caches.Where(x => uids.Contains(x.Key));
            var lackuids = uids.Except(resultList.Select(x => x.Key).ToList()).ToList();

            if (LackItems != null && lackuids.Count != 0)
                LackItems(lackuids, null);

            return resultList.ToList();
        }

        /// <summary>
        /// 清除所有缓存。
        /// </summary>
        public static void Clear()
        {
            Caches.Clear();
        }

        /// <summary>
        /// 移除指定ID的值。若移除成功，则返回True。
        /// </summary>
        /// <param name="uid"></param>
        public static bool Remove(string uid)
        {
            T obj;
            return Caches.TryRemove(uid, out obj);
        }

        /// <summary>
        /// 获取指定的ID是否存在于缓存中。
        /// </summary>
        /// <param name="uid"></param>
        public static bool Exists(string uid)
        {
            return !String.IsNullOrEmpty(Caches.FirstOrDefault(x => x.Key == uid).Key);
        }

        /// <summary>
        /// 获取指定的ID是否存在于缓存中，并返回不存在的ID集合。
        /// </summary>
        public static IList<string> Exists(params string[] uids)
        {
            return Caches.Where(x => !uids.Contains(x.Key)).Select(x => x.Key).ToList();
        }
        #endregion

    }

}
