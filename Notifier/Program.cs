using System.Diagnostics;
using System.Text.Json;

namespace Notifier.Notifier
{
    internal class Program
    {
        static void Main()
        {
            // Validate user input
            UserData? userData;
            string jsonText;
            if (!File.Exists("UserData.json"))
            {
                NotificationHelper.ShowToastNotification("Missing config", "UserData.json does not exist.");
                return;
            }
            try
            {
                jsonText = File.ReadAllText("UserData.json");
            }
            catch (Exception ex)
            {
                NotificationHelper.ShowToastNotification("Unable to read UserData.json", ex.Message);
                return;
            }
            try
            {
                userData = JsonSerializer.Deserialize<UserData>(jsonText);
            }
            catch (Exception)
            {
                NotificationHelper.ShowToastNotification("Invalid JSON", "UserData.json has invalid JSON.");
                return;
            }
            if (userData == null ||
                string.IsNullOrWhiteSpace(userData.Uid) ||
                string.IsNullOrWhiteSpace(userData.Pass) ||
                string.IsNullOrWhiteSpace(userData.UserAgent) ||
                userData.PeriodInSeconds <= 0)
            {
                NotificationHelper.ShowToastNotification("Missing config", "UserData.json is missing some required data.");
                return;
            }

            // Start
            Watcher watcher = new(new InFire(userData));
            watcher.Start(userData.PeriodInSeconds);
            Process.GetCurrentProcess().WaitForExit();
        }
    }
}
