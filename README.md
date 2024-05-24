# Notifier
A windows app which will display a notification when new torrents are available on https://infire.si.

![Notifier screenshot](https://github.com/Doctorslo/InFire-Notifier/blob/master/Images/NotifierExample.png?raw=true)

## Configuration
You need to change values in **UserData.json**.

- **Uid**: get it from cookies
- **Pass**: get it from cookies
- **UserAgent**: preferred user agent
- **PeriodInSeconds**: a delay between requests in seconds. Don't set this too low. Recommended is 300 seconds (5 minutes).

![Notifier screenshot](https://github.com/Doctorslo/InFire-Notifier/blob/master/Images/DevToolsInfo.png?raw=true)

## Start With Windows
Run "shell:startup" and place a shortcut to that folder.

## Exiting The App
Currently there is no appropriate way to stop the app. Open Task Manager and end the task.

## Plan For The Future
- Add minimum size option
