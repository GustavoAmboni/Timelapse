# Timelapse

Timelapse is a simple time-tracker console application that helps you keep track of the time spent on the recording items.

## Important Disclaimer

This application is under development, so expect some bugs, breaking changes and loss of data.

## How it works?

By using commands, you can track, stop and list items that you inform.
Any command used in the first run, will trigger the console application to create a folder in the `%appdata%` path of your system.
This folder contains the database necessary to save the information required to track the items and will be migrated automatically.
The information saved includes the name of the items, description, periods that the items were run and stopped, etc. 
As this application runs in separeted commands (not like a background service), the database is necessary to keep the information to track.
It will also help with future implementations, like generation of reports and analysis.

## Commands

The avaiable commands are:

* Start: Starts a new item and tracks it or gets and old item by the name and tracks it.
* Stop: Stop the last running item.
* List: Lists the last 5 items by default or lists items based on the given date time range.

## Usage

The application has two main entities: Item and Period. The item is the "task" or "activity" that englobles the periods. It has a name and description and accepts a link that, when diplayed on the console, can be clicked.
The periods are the periods that the time is being tracked. For example, you can track something in the morning, stop tracking and get back in the same activity in the late night.

```
$> Timelapse.exe track "Doing a really important task" "something that can't be said out loud"
*** tracks until 11:30am
$> Timelapse.exe stop
*** proceds to do something else and gets back at 08:00pm of the same day
$> Timelapse.exe track "Doing a really important task"
```

| # | Name                          | Started | Stopped | Duration | Comment |
|---|-------------------------------|---------|---------|----------|---------|
| 1 | Doing a really important task | 08:00pm | -       | 01:15:00 |         |
| 2 | Doing a really important task | 08:20am | 11:30am | 03:10:00 |         |



### Start

Start will create an item or start the tracking in an already existing one by the name (name is mandatory). 
It has the option of receiving a description and also a given date so you can manually input a earlier time if you forgot to start tracking.
It also can receive a link so the item becomes clickable when listing.

```
$> Timelapse.exe track Sleeping ZzzZZZzz --date 22:40 --anchor "https://en.wikipedia.org/wiki/Counting_sheep"
```

| # | Name     | Status  | Total Duration |
|---|----------|---------|----------------|
| 1 | Sleeping | Running | 01:15:00       |

```
$> Timelapse.exe list
```

| # | Name     | Started| Stopped | Duration | Comment |
|---|----------|--------|---------|----------|---------|
| 1 | [Sleeping](https://en.wikipedia.org/wiki/Counting_sheep) | 22:40  | -       | 02:25:00 |         |

### Stop

Stop will stop the last running task. You can optionally provide a comment to adress in the future the reason of stopping.
As the start, it can receive a date to manually set the stopping time, it is most used when you forget to stop your tracking (happens a lot).
It does nothing when there is nothing to stop.

```
$> Timelapse.exe stop "forgot to stop earlier" -d 23:10
```

| # | Name     | Status  | Total Duration |
|---|----------|---------|----------------|
| 1 | Sleeping | Stopped | 1d 01:10       |

```
$> Timelapse.exe list
```

| # |                                Name                      | Started| Stopped | Duration |         Comment       |
|---|----------------------------------------------------------|--------|---------|----------|-----------------------|
| 1 | [Sleeping](https://en.wikipedia.org/wiki/Counting_sheep) | 22:40  | 23:10   | 1d 01:10 | Forgot to stop earlier|

### List

Lists as default, the last 5 tracked Periods. You can pass in the parameters an starting period and ending period and it will list items between this date range.

```
$> Timelapse.exe list -s "01/01/2022" -e "22:05"
```

| # | Name                          | Started | Stopped | Duration | Comment |
|---|-------------------------------|---------|---------|----------|---------|
| 1 | Doing a something in 2023     | 08:00pm | 09:15pm | 13:15:00 |         |
| 2 | Doing a something in 2022     | 05:20am | 17:10pm | 13:50:00 |         |