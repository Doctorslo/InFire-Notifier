using System.Text;

namespace Notifier
{
    internal class Watcher(InFire inFire)
    {
        private readonly InFire mInFire = inFire;
        private List<string> mExistingTorrents = [];
        private Timer? mTimer;

        public void Start(int timerPeriodInSeconds)
        {
            mExistingTorrents = mInFire.GetTorrents().Result;
            mTimer = new Timer(TimerTask, null, 0, timerPeriodInSeconds * 1000);
        }

        public void Stop()
        {
            mTimer?.Dispose();
        }

        private void TimerTask(object? state)
        {
            var availableTorrents = mInFire.GetTorrents().Result;
            var newTorrents = ParseNewTorrents(availableTorrents);
            if(newTorrents.Count == 0)
            {
                //NotificationHelper.ShowToastNotification("New", "nothing");
                return;
            }
            var notificationText = new StringBuilder();
            foreach (var torrent in newTorrents)
                notificationText.AppendLine(torrent);
            NotificationHelper.ShowToastNotification("New", notificationText.ToString());
        }

        private List<string> ParseNewTorrents(List<string> torrentsList)
        {
            var newTorrents = new List<string>();
            foreach (var torrent in torrentsList)
            {
                if (mExistingTorrents.Contains(torrent)) continue;
                mExistingTorrents.Add(torrent);
                newTorrents.Add(torrent);
            }
            return newTorrents;
        }
    }
}
