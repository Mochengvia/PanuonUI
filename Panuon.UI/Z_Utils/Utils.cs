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
            if (_runningTaskQuantity < MaxTaskQuantity)
                RecheckQueue();
        }
        #endregion

    }

    /// <summary>
    /// 伸缩缓存池。当你需要在缓存中保存大量的数据时（一次性读取可能占用较高内存），只需读取数据的唯一键并储存该键到缓存池中即可。
    /// 当你使用Get或GetAll方法访问缓存值时，您可以指定当缓存中不存在某个（或某些）键的值，或某些键对应的值为Default值时，应该进行的后续处理操作。
    /// </summary>
    /// <typeparam name="TKey">标识该缓存的唯一键。</typeparam>
    /// <typeparam name="TValue">该缓存的实际值。</typeparam>
    public class CachePoll<TKey, TValue>
    {
        #region Constructor
        /// <summary>
        /// 初始化伸缩缓存池实例。该缓存池将不会定时回收资源。
        /// </summary>
        public CachePoll()
        {
            _caches = new ConcurrentDictionary<TKey, CacheModel<TValue>>();
        }

        /// <summary>
        /// 初始化伸缩缓存池实例，并指定缓存的生命周期时间。该缓存池将不会定时回收资源，但您可以使用Collect()方法手动回收缓存。
        /// </summary>
        /// <param name="ttl">每个缓存的生命周期。当调用Collect()方法时，若该缓存的最后一次访问（更新或获取）时间超过此期限，则其缓存值将被释放（但不会移除键）。</param>
        public CachePoll(TimeSpan ttl)
        {
            _caches = new ConcurrentDictionary<TKey, CacheModel<TValue>>();
        }

        /// <summary>
        /// 初始化伸缩缓存池实例，并指定缓存的生命周期时间，以及定时回收缓存的时间间隔。您也可以使用Collect()方法立即回收缓存。
        /// </summary>
        /// <param name="ttl">每个缓存的生命周期。当调用Collect()方法时，若该缓存的最后一次访问（更新或获取）时间超过此期限，则其缓存值将被释放（但不会移除键）。</param>
        /// <param name="collectInterval">定时回收缓存的时间间隔，计时器将定时调用Collect()方法。建议值为1倍~2倍的缓存生命周期时间。</param>
        public CachePoll(TimeSpan ttl, TimeSpan collectInterval)
        {
            _caches = new ConcurrentDictionary<TKey, CacheModel<TValue>>();
            _ttl = ttl;
            _collectInterval = collectInterval;
            _timer = new Timer(OnTick, null, (int)_collectInterval.Value.TotalMilliseconds, Timeout.Infinite);
        }

        #endregion

        #region  Identity
        private static ConcurrentDictionary<TKey, CacheModel<TValue>> _caches { get; set; }

        private static Timer _timer;

        private static TimeSpan? _ttl;

        private static TimeSpan? _collectInterval;

        private int _isCollecting;
        #endregion

        #region EventHandler
        /// <summary>
        /// 表示缓存值已更新（仅更新键时将不会触发此事件）。当指定此事件的后续处理时，该处理将以同步模式执行。事件参数: sender = List<TKey>, e = null
        /// </summary>
        public EventHandler Updated;

        /// <summary>
        /// 缓存已回收。当指定此事件的后续处理时，该处理将以同步模式执行。事件参数：sender = List<TKey>, e = null
        /// </summary>
        public EventHandler Collected;
        #endregion

        #region Property
        /// <summary>
        /// 获取当前的所有缓存。
        /// </summary>
        public IEnumerable<KeyValuePair<TKey, TValue>> Caches
        {
            get
            {
                return _caches.Select(x => new KeyValuePair<TKey, TValue>(x.Key, x.Value.Value));
            }
        }

        /// <summary>
        /// 获取真实缓存的数量（TValue为默认值的缓存不会计入）。
        /// </summary>
        public int RealCount
        {
            get
            {
                return _caches.Count(x => x.Value.Value != null);
            }
        }

        /// <summary>
        /// 获取所有缓存的数量。
        /// </summary>
        public int Count
        {
            get
            {
                return _caches.Count;
            }
        }

        /// <summary>
        /// 获取是否正在回收缓存。
        /// </summary>
        public bool IsCollecting
        {
            get
            {
                return _isCollecting == 1;
            }
        }

        /// <summary>
        /// 获取缓存的生命周期。
        /// </summary>
        public TimeSpan? CacheTTL
        {
            get
            {
                return _ttl;
            }
        }

        /// <summary>
        /// 获取缓存的回收间隔。
        /// </summary>
        public TimeSpan? CollectInterval
        {
            get
            {
                return _collectInterval;
            }
        }
        #endregion

        #region APIs
        public bool ContainsKey(TKey key)
        {
            return _caches.ContainsKey(key);
        }

        /// <summary>
        /// 向缓存池中添加一个占位缓存，该缓存的实际值为Default值。
        /// <para>若该键已存在，则不会有任何操作。</para>
        /// </summary>
        /// <param name="key">要添加的键。</param>
        public void AddKey(TKey key)
        {
            _caches.TryAdd(key, new CacheModel<TValue>());
        }

        /// <summary>
        /// 向缓存池中添加一组占位缓存，该缓存只有键而不具有实际值。、
        /// <para>若该键已存在，则不会有任何操作。</para>
        /// </summary>
        /// <param name="keys">要添加的一组键。</param>
        public void AddKeys(IEnumerable<TKey> keys)
        {
            foreach (var key in keys)
            {
                _caches.TryAdd(key, new CacheModel<TValue>());
            }
        }

        /// <summary>
        /// 向缓存池中添加一个键值对，若添加成功，将触发Updated事件。若键已存在于缓存池中，将替换原有的值。
        /// <para>请勿在循环中调用此方法，这将频繁触发Updated事件，并严重影响性能。若要一次性添加多个值，请使用AddOrUpdates方法。</para>
        /// </summary>
        /// <param name="key">要添加的键。</param>
        /// <param name="value">该键对应的值。</param>
        public void AddOrUpdate(TKey key, TValue value)
        {
            var newModel = new CacheModel<TValue>(value);
            if (_caches.AddOrUpdate(key, newModel, (k, v) => newModel) != null)
            {
                Updated?.Invoke(new List<TKey> { key }, null);
            };
        }

        /// <summary>
        /// 向缓存池中添加一组键值对，若有至少一个值添加成功，将在添加结束后触发Updated事件。若键已存在于缓存池中，将替换原有的值。
        /// </summary>
        /// <param name="keyValuePairs">要添加的一组键值对。</param>
        public void AddOrUpdates(IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            var updatedKeys = new List<TKey>();

            foreach (var pair in keyValuePairs)
            {
                var newModel = new CacheModel<TValue>(pair.Value);
                if(_caches.AddOrUpdate(pair.Key, newModel, (k, v) => newModel) != null)
                {
                    updatedKeys.Add(pair.Key);
                }
            }
            if(updatedKeys.Count != 0)
                Updated?.Invoke(updatedKeys, null);
        }

        /// <summary>
        /// 获取指定键的缓存值。若该键不存在，则返回默认值。
        /// </summary>
        /// <param name="key">要查找的键。</param>
        /// <returns></returns>
        public TValue Get(TKey key)
        {
            if (_caches.ContainsKey(key))
            {
                var cache = _caches[key];
                cache.LastTime = DateTime.Now.ToTimeStamp();
                return cache.Value;
            }
            else
            {
                return default(TValue);
            }
        }

        /// <summary>
        /// 获取指定键的缓存值。若该键不存在或该键对应的值为Default值，则将在调用事件处理方法后继续尝试返回该键的值。
        /// <para>若连续5次调用事件处理后仍无法找到该键或该键对应的值始终为Default值，将抛出CacheNotExistsException异常。</para>
        /// </summary>
        /// <param name="key">要查找的键。</param>
        /// <param name="lackItemCallback">若该键不存在或该键对应的值为Default值，则调用此回调方法来处理后续内容。您应当在此事件处理中向缓存池添加该键（若不存在）及其实际值，否则该处理事件将将被继续调用。</param>
        /// <returns></returns>
        public TValue Get(TKey key, EventHandler lackItemCallback)
        {
            int count = 0;

            while (!_caches.ContainsKey(key) || IsDefault(_caches[key].Value))
            {
                if (count == 5)
                {
                    return default(TValue);
                }
                lackItemCallback.Invoke(key, null);
                count++;
            }
            var cache = _caches[key];
            cache.LastTime = DateTime.Now.ToTimeStamp();
            return cache.Value;
        }



        public IEnumerable<KeyValuePair<TKey, TValue>> GetAll(IEnumerable<TKey> keys)
        {
            var dictionary = keys.ToDictionary(k => k, v => default(TValue));
            var timeStamp = DateTime.Now.ToTimeStamp();
            foreach (var key in keys)
            {
                var cache = _caches[key];
                dictionary[key] = cache.Value;
                cache.LastTime = timeStamp;
            }
            return dictionary;
        }

        /// <summary>
        /// 获取所有指定键的缓存值。若至少一个键不存在，或该键的缓存值为默认值，则触发事件回调处理。
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="lackItemsCallback">若至少一个键不存在，或其缓存值为默认值，则触发此回调方法处理后续操作。</param>
        /// <returns></returns>
        public IDictionary<TKey, TValue> GetAll(IEnumerable<TKey> keys, EventHandler lackItemsCallback)
        {
            var resultDic = keys.ToDictionary(k => k, v => default(TValue));
            var lackKeys = new List<TKey>();

            var timeStamp = DateTime.Now.ToTimeStamp();

            foreach (var key in keys)
            {
                if (_caches.ContainsKey(key))
                {
                    var cache = _caches[key];
                    if (!IsDefault(cache.Value))
                    { 
                        resultDic[key] = cache.Value;
                        cache.LastTime = timeStamp;
                        continue;
                    }
                }
                lackKeys.Add(key);
                
            }

            while (lackKeys.Count != 0)
            {
                lackItemsCallback.Invoke(lackKeys, null);
                var lackKeysCopy = lackKeys.ToList();
                lackKeys.Clear();

                foreach(var key in lackKeysCopy)
                {
                    var cache = _caches[key];
                    if (IsDefault(cache.Value))
                    {
                        lackKeys.Add(key);
                    }
                    else
                    {
                        resultDic[key] = cache.Value;
                        cache.LastTime = timeStamp;
                    }
                }
            }
            return resultDic;
        }

        public void ReleaseCache(TKey key)
        {
            if (_caches.ContainsKey(key))
            {
                _caches[key].Value = default(TValue);
            }
        }

        public void ReleaseCaches(IEnumerable<TKey> keys)
        {
            foreach(var key in keys)
            {
                if (_caches.ContainsKey(key))
                {
                    _caches[key].Value = default(TValue);
                }
            }
        }

        public void Remove(TKey key)
        {
            CacheModel<TValue> outValue;
            _caches.TryRemove(key, out outValue);
        }

        public void RemoveAll(IEnumerable<TKey> keys)
        {
            foreach(var key in keys)
            {
                CacheModel<TValue> outValue;
                _caches.TryRemove(key, out outValue);
            }
        }

        public bool ChangeKey(TKey oldKey, TKey newKey)
        {
            if (_caches.ContainsKey(newKey) || !_caches.ContainsKey(oldKey))
                return false;
            else
            {
                CacheModel<TValue> outValue;
                if (!_caches.TryRemove(oldKey, out outValue))
                    return false;
                if (!_caches.TryAdd(newKey, new CacheModel<TValue>(outValue.Value)))
                    return false;

                return true;
            }
        }

        /// <summary>
        /// 立即回收缓存池。最后一次访问（更新或获取）时间超过缓存生命周期的缓存值将被释放（但不会移除其键）。
        /// 若至少有一个缓存被成功回收，将调用一次GC的垃圾回收方法来强制清理内存。
        /// <para>频繁回收缓存将严重影响性能。建议调用时间间隔为1倍~2倍的缓存生命周期时间。</para>
        /// </summary>
        public void Collect()
        {
            if (IsCollecting || _ttl == null)
                return;

            var collectIDs = new List<TKey>();

            Interlocked.Exchange(ref _isCollecting, 1);

            var timeStamp = DateTime.Now.ToTimeStamp();

            foreach(var cache in _caches)
            {
                if(!IsDefault(cache.Value.Value) && timeStamp - cache.Value.LastTime > _ttl.Value.TotalMilliseconds)
                {
                    collectIDs.Add(cache.Key);
                    cache.Value.Value = default(TValue);
                }
            }

            Interlocked.Exchange(ref _isCollecting, 0);

            if(collectIDs.Count != 0)
            {
                Collected?.Invoke(collectIDs, null);
                Updated?.Invoke(collectIDs, null);
                GC.Collect();
            }
        }
        #endregion

        #region Function
        private void OnTick(object state)
        {
            Collect();
            _timer.Change((int)_collectInterval.Value.TotalMilliseconds, Timeout.Infinite);
        }

        private bool IsDefault( TValue value)
        {
            return EqualityComparer<TValue>.Default.Equals(value, default(TValue));
        }
        #endregion

    }

    class CacheModel<T>
    {
        #region Constructor
        public CacheModel()
        {
            LastTime = DateTime.Now.ToTimeStamp();
        }

        public CacheModel(T value)
        {
            LastTime = DateTime.Now.ToTimeStamp();
            Value = value;
        }
        #endregion

        #region Property
        public T Value { get; set; }


        public long LastTime { get; set; }
        #endregion
    }

    public class CacheNotExistsException : Exception
    {
        #region Constructor
        public CacheNotExistsException()
        {

        }

        public CacheNotExistsException(Modes mode)
        {
            Mode = mode;
        }
        #endregion

        #region Property
        public Modes Mode { get; set; }
        #endregion

        public enum Modes
        {
            /// <summary>
            /// 多次无法从缓存中找到指定键。
            /// </summary>
            KeyNotFound,
            /// <summary>
            /// 该键的缓存值始终为Default值。
            /// </summary>
            DefaultValue,
        }
    }

}
