using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Shipwreck.ViewModelUtils
{
    public sealed class EventModel
    {
        #region static

        public static ObservableCollection<EventModel> Data { get; }
            = new ObservableCollection<EventModel>();

        public static object SyncRoot { get; } = new object();

        private static int _MaximumCount = 5000;

        public static int MaximumCount
        {
            get => _MaximumCount;
            set => _MaximumCount = value;
        }

        public static void Add(EventModel item)
        {
            var taken = false;

            try
            {
                Monitor.TryEnter(SyncRoot, 10, ref taken);

                if (taken)
                {
                    AddCore(item);

                    return;
                }
            }
            finally
            {
                if (taken)
                {
                    Monitor.Exit(SyncRoot);
                }
            }

            lock (_Adding)
            {
                var isAdding = _Adding.Any();

                _Adding.Add(item);

                if (!isAdding)
                {
                    var d = Application.Current?.Dispatcher;

                    if (d?.HasShutdownStarted == false)
                    {
                        d.BeginInvoke((Action)FlushAdding);
                    }
                    else
                    {
                        Task.Run(FlushAdding);
                    }
                }
            }
        }

        private static List<EventModel> _Adding = new List<EventModel>();

        private static void AddCore(EventModel item)
        {
            while (Data.Count >= _MaximumCount)
            {
                Data.RemoveAt(Data.Count - _MaximumCount);
            }
            Data.Add(item);
        }

        private static void FlushAdding()
        {
            EventModel[] adding;
            lock (_Adding)
            {
                adding = _Adding.ToArray();
                _Adding.Clear();
            }

            if (adding.Any())
            {
                lock (SyncRoot)
                {
                    foreach (var e in adding)
                    {
                        AddCore(e);
                    }
                }
            }
        }

        #endregion static

        internal EventModel(
            TraceEventCache eventCache,
            string source,
            TraceEventType eventType,
            int id,
            string message,
            object[] data)
        {
            ProcessId = eventCache.ProcessId;
            ThreadId = eventCache.ThreadId;
            DateTime = eventCache.DateTime;

            Source = source;

            EventType = eventType;
            Id = id;

            Message = message != null ? (data?.Length > 0 ? string.Format(message, data) : message)
                    : data != null ? string.Join(" ", data) : null;
        }

        public int ProcessId { get; }
        public string ThreadId { get; }
        public DateTime DateTime { get; }
        public string Source { get; }

        public TraceEventType EventType { get; }
        public int Id { get; }

        public string Message { get; }
    }
}
