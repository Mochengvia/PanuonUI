/*==============================================================
*作者：ZEOUN
*时间：2018/10/15 12:26:44
*说明： 辅助工具。
*日志：2018/10/15 12:26:44 创建。
*==============================================================*/
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Panuon.UI.Utils
{
    /// <summary>
    /// 提供Task的池化管理，并发任务数量可控。
    /// </summary>
    public class TaskFactory
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
            set { _maxTaskQuantity = value; }
        }
        private static int? _maxTaskQuantity;

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
        }
        #endregion

    }
}
