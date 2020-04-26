# SFX-100-Streamdeck
Simfeedback extension and Streamdeck plugin to Start/Stop and control your Sim-Rig 

## Please read the installation instructions below BEFORE download 

![In action](doc/screenshot.JPG?raw=true|width=200)

## New Feature StartProfile / StartSimFeedback
It is now possible to start SimFeedback with a desired profile and enable Motion by clicking a single Button.  
- If SimFeedback is not startet it will start it for you.  
- If SimFeedback is already running it will be restarted with the (new) desired profile automatically.

:warning: **The selected profile MUST be located in SimFeedback profiles folder** 

![In action](doc/screenshot2.JPG?raw=true|width=200)

## Requirements

- Stream Deck for Windows  
https://www.elgato.com/de/gaming/downloads

- SimFeedback Software with Expert mode enabled  
https://github.com/SimFeedback/SimFeedback-AC-Servo


## Installation
- Do not click on Download on this page. Download the files from the release tab or use the following links:
  - [latest StreamDeckExtension.zip](https://github.com/ashupp/SFX-100-Streamdeck/releases/latest/download/StreamdeckExtension.zip)
  - [latest StreamDeckPlugin](https://github.com/ashupp/SFX-100-Streamdeck/releases/latest/download/sfx-100-streamdeck-plugin.streamDeckPlugin)
- Unzip the extension into the Simfeedback extension dir.
- Enable the plugin and enable autorun of the extension in SimFeedback
- Install the Streamdeck extension by double-click
- If it does not work close Simfeedback, run remove_blocking.bat in Simfeedback folder with admin privileges and restart

## Known issues
- "Is telemetry provider connected?" is set to true as soon as any telemetry provider is connected but it will never be false again. Even if the telemetry provider changes or is disconnected. This seems to be a problem of the SimFeedback API.

## Ideas / Future implementation
- ~~Change / activate Profile with click on button~~ (Available since 0.0.1.5)
- Display of current intensity value on button face (thx @Flag Ghost)

## Third party Libraries
**This tool makes use of the following great projects:**

BarRaider's Stream Deck Tools  
https://github.com/BarRaider/streamdeck-tools
