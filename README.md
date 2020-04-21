# SFX-100-Streamdeck
Simfeedback extension and Streamdeck plugin to Start/Stop and control your Sim-Rig 

### Prerelease on the release tab (And click on "assets") 
https://github.com/ashupp/SFX-100-Streamdeck/releases/latest

![In action](doc/screenshot.JPG?raw=true|width=200)

## New Feature StartProfile / StartSimFeedback
It is now possible to start SimFeedback with a desired profile and enable Motion by clicking a single Button.  
- If SimFeedback is not startet it will start it for you.  
- If SimFeedback is already running it will be restarted with the (new) desired profile automatically.

![In action](doc/screenshot2.JPG?raw=true|width=200)
![In action](doc/screenshot3.JPG?raw=true|width=200)

## Requirements

- Stream Deck for Windows  
https://www.elgato.com/de/gaming/downloads

- SimFeedback Software with Expert mode enabled  
https://github.com/SimFeedback/SimFeedback-AC-Servo


## Installation
- Unzip the Extension into the Simfeedback Extension dir and Enable in SimFeedback
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
