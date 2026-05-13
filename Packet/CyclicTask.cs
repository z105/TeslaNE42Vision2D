using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TeslaNE42Vision2D.Packet
{
    public class CyclicTask
    {
        private readonly Func<CancellationToken, Task> _action;
        private readonly TimeSpan _interval;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private CancellationTokenSource _cancellationTokenSource;
        private Task _cyclicTask;

        public bool IsActive => _cyclicTask != null && !_cyclicTask.IsCompleted;

        public CyclicTask(Action action, TimeSpan interval)
        {
            _action = token =>
            {
                action();
                return Task.CompletedTask;
            };
            _interval = interval;
        }

        public void Start()
        {
            if (IsActive) return;
            _stopwatch.Restart();
            _cancellationTokenSource = new CancellationTokenSource();
            _cyclicTask = Task.Run(() => CyclicTaskRun(_cancellationTokenSource.Token));
        }

        public void Restart()
        {
            if (IsActive)
                _stopwatch.Restart();
            else
                Start();
        }

        public Task Stop()
        {
            if (!IsActive) return Task.CompletedTask;
            _cancellationTokenSource.Cancel();
            _stopwatch.Reset();
            return _cyclicTask;
        }

        private async Task CyclicTaskRun(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                _stopwatch.Restart();
                try
                {
                    await _action(token).ConfigureAwait(false);
                    await WaitForStopwatch(token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
            _stopwatch.Reset();
        }

        private async Task WaitForStopwatch(CancellationToken token)
        {
            TimeSpan delay = _interval - _stopwatch.Elapsed;
            while (!token.IsCancellationRequested && delay > TimeSpan.Zero)
            {
                await Task.Delay(delay, token).ConfigureAwait(false);
                delay = _interval - _stopwatch.Elapsed;
            }
        }
    }
}
