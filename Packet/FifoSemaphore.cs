using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace TeslaNE42Vision2D.Packet
{
    public class FifoSemaphore
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly ConcurrentQueue<TaskCompletionSource<bool>> _queue =
            new ConcurrentQueue<TaskCompletionSource<bool>>();

        public FifoSemaphore(int initialCount)
        {
            _semaphore = new SemaphoreSlim(initialCount);
        }

        public void Wait()
        {
            WaitAsync().Wait();
        }

        public Task WaitAsync()
        {
            var tcs = new TaskCompletionSource<bool>();
            _queue.Enqueue(tcs);
            _semaphore.WaitAsync().ContinueWith(t =>
            {
                if (_queue.TryDequeue(out TaskCompletionSource<bool> popped))
                    popped.SetResult(true);
            });
            return tcs.Task;
        }

        public void Release()
        {
            _semaphore.Release();
        }
    }
}
