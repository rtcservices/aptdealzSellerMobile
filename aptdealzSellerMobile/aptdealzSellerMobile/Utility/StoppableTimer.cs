using System;
using System.Threading;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Utility
{
    public class StoppableTimer
    {
        #region [ Objects ]
        private readonly TimeSpan timespan;
        private readonly Action callback;
        private CancellationTokenSource cancellation;
        #endregion

        #region [ Ctor ]
        public StoppableTimer(TimeSpan timespan, Action callback)
        {
            this.timespan = timespan;
            this.callback = callback;
            this.cancellation = new CancellationTokenSource();
        }
        #endregion

        #region [ Methods ]
        public void Start()
        {
            CancellationTokenSource cts = this.cancellation; // safe copy
            Device.StartTimer(this.timespan,
                () =>
                {
                    if (cts.IsCancellationRequested) return false;
                    this.callback.Invoke();
                    return true; // or true for periodic behavior
                });
        }

        public void Stop()
        {
            Interlocked.Exchange(ref this.cancellation, new CancellationTokenSource()).Cancel();
        }

        public void Dispose()
        {

        }
        #endregion
    }
}
