using System;
using System.Threading;

namespace TeslaNE42Vision2D.Packet
{
    public class TimeoutHandler : IDisposable
    {
        private readonly Timer _timer;
        private readonly TimeSpan _timeout;
        private readonly Action _syncAction;
        private bool _started = false;
        private bool _disposed = false;

        public TimeoutHandler(TimeSpan timeout, Action action)
        {
            _timeout = timeout;
            _syncAction = action;
            _timer = new Timer(OnTimedEvent, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }

        public void Start()
        {
            if (_started) return;
            Restart();
            _started = true;
        }

        public void Stop()
        {
            _timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            _started = false;
        }

        public void Restart()
        {
            _timer.Change(_timeout, Timeout.InfiniteTimeSpan);
        }

        private void OnTimedEvent(object state)
        {
            _syncAction?.Invoke();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing) _timer?.Dispose();
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TimeoutHandler()
        {
            Dispose(false);
        }
    }
}
