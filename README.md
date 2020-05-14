# SFX-100-Streamdeck

Simfeedback extension and Streamdeck plugin to Start/Stop and control your Sim-Rig 
## Please read the installation instructions below BEFORE download 
**See it in action here** (Thx @hoihman for the video!)  
[![Video showing usage](http://img.youtube.com/vi/9pzWQWNlzzM/0.jpg)](http://www.youtube.com/watch?v=9pzWQWNlzzM)



![In action](doc/screenshot.JPG?raw=true|width=200)

## Version 0.0.2.6 New Features: Live Tuning and show values
- It is now possible to change the values of every effect and controller
- Optionally show the new value on button after changing it
- Added display-only buttons to display current values  

![Edit and show Effect values](doc/showNewValue.JPG?raw=true|width=200)

## Version 0.0.1.8 New Feature: StartProfile / StartSimFeedback
It is now possible to start SimFeedback with a desired profile and enable Motion by clicking a single Button.  
- If SimFeedback is not startet it will start it for you.  
- If SimFeedback is already running it will be restarted with the (new) desired profile automatically.

:warning: **The selected profile MUST be located in SimFeedback profiles folder** 

![Start with profile](doc/screenshot2.JPG?raw=true|width=200)

## Requirements

- Stream Deck for Windows  
https://www.elgato.com/de/gaming/downloads

- SimFeedback Software with Expert mode enabled  
https://github.com/SimFeedback/SimFeedback-AC-Servo


## Installation
- Do not click on Download on this page. Download the files from the release tab or use the following links:
  - [latest StreamDeckExtension.zip](https://github.com/ashupp/SFX-100-Streamdeck/releases/latest/download/StreamdeckExtension.zip)
  - [latest StreamDeckPlugin](https://github.com/ashupp/SFX-100-Streamdeck/releases/latest/download/sfx-100-streamdeck-plugin.streamDeckPlugin)
- Unzip the extension into the SimFeedback extension dir.
- Enable the plugin and enable autorun of the extension in SimFeedback
- Install the Streamdeck extension by double-click
- If it does not work close Simfeedback, run remove_blocking.bat in Simfeedback folder with admin privileges and restart

## Updating
Prepare by downloading the new versions (see above)

**Update SimFeedback extension**
- Close SimFeedback if it is running
- Locate Extensions folder. Delete Folder "StreamdeckExtension"
- Unzip the new version of the extension into the Simfeedback extension dir
- Start SimFeedback, re-enable the extension and autostart and check if the version is the new one
- Click on the extensions tab to see if the extension is running

**Update StreamDeck plugin**
- Open the windows StreamDeck application and click the lower right button "More actions"
- Locate sfx-100-streamdeck-plugin and click Uninstall
- Install the new version of the StreamDeck plugin by double-click  
- Your buttons should be still be there after installing the new version  

## Known issues
- "Is telemetry provider connected?" is set to true as soon as any telemetry provider is connected but it will never be false again. Even if the telemetry provider changes or is disconnected. This seems to be a problem of the SimFeedback API.

## Ideas / Future implementation

- ✓ Change / activate Profile with click on button (Available since 0.0.1.5)
- ✓ Display of current intensity value on button face (thx @Flag Ghost) (Available since 0.0.2.6)
- Backup current profile to chosen backup directory with timestamp (thx @HoiHman)
- Display current enabled/disabled state on (toggle) buttons (thx @J.R.)
- Show values on incement/decrement buttons constantly (thx @HoiHman)

## Third party Libraries
**This tool makes use of the following great projects:**

BarRaider's Stream Deck Tools  
https://github.com/BarRaider/streamdeck-tools
