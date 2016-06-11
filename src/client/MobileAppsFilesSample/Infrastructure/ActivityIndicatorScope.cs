using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileAppsFilesSample
{
    class ActivityIndicatorScope : IDisposable
    {
        private ActivityIndicator indicator;
        private int delay;

        public ActivityIndicatorScope(ActivityIndicator indicator, int delay)
        {
            this.indicator = indicator;
            this.delay = delay;
            SetIndicatorActivity(true);
        }

        public void Dispose()
        {
            // add a delay after the scope has been disposed before hiding the indicator
            Task.Delay(delay).ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void SetIndicatorActivity(bool isActive)
        {
            this.indicator.IsVisible = isActive;
            this.indicator.IsRunning = isActive;
        }

    }
}
