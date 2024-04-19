using Microsoft.Toolkit.Uwp.Notifications;

namespace Notifier.Notifier
{
    internal static class NotificationHelper
    {
        internal static void ShowToastNotification(string title, string description)
        {
            //Uri img = new Uri("");
            new ToastContentBuilder()
                .AddText(title)
                .AddText(description)
                //.AddAppLogoOverride(img)
                .Show();
        }
    }
}
