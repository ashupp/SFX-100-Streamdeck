using System;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;
using BarRaider.SdTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace sfx_100_streamdeck_plugin.PluginActions
{
    [PluginActionId("sfx-100-streamdeck-plugin.startstopmotiontoggle")]
    class StartStopMotionToggle : PluginBase
    {
        private class PluginSettings
        {
            public static PluginSettings CreateDefaultSettings()
            {
                PluginSettings instance = new PluginSettings();
                instance.PrimaryImageFilename = Path.GetFullPath("Images/MotionStop@2x.png");
                instance.SecondaryImageFilename = Path.GetFullPath("Images/MotionStart@2x.png");
                instance.CheckInterval = 10;
                return instance;
            }

            [FilenameProperty]
            [JsonProperty(PropertyName = "primaryImage")]
            public string PrimaryImageFilename { get; set; }

            [FilenameProperty]
            [JsonProperty(PropertyName = "secondaryImage")]
            public string SecondaryImageFilename { get; set; }

            [JsonProperty(PropertyName = "checkInterval")]
            public int CheckInterval { get; set; }
        }

        #region Private Members
        private PluginSettings settings;
        string primaryFile;
        string secondaryFile;
        DateTime lastRefresh;
        public bool ButtonIsCurrentlyPressed;
        #endregion

        private async void ToggleStartStop()
        {
            try {
                PipeServerConnection.Instance.RestartChannel();
                if (ButtonIsCurrentlyPressed || PipeServerConnection.Instance.Channel.IsRunning())
                {
                    if (PipeServerConnection.Instance.Channel.StopMotion())
                    {
                        SetNotRunning();
                    }
                }
                else
                {
                    if (PipeServerConnection.Instance.Channel.StartMotion())
                    {
                        SetRunning();
                    }
                }
            }
            catch (EndpointNotFoundException endpointNotFoundException)
            {
                Logger.Instance.LogMessage(TracingLevel.ERROR, "Error: Endpoint not found - Is SimFeedback available and is the Plugin enabled? " + endpointNotFoundException.Message);
            }
            catch (CommunicationObjectFaultedException communicationObjectFaultedException)
            {
                Logger.Instance.LogMessage(TracingLevel.ERROR, "Error: communicationObjectFaultedException: " + communicationObjectFaultedException.Message);
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.ERROR, "Error during Key processing: " + ex.Message);
            }
        }

        public StartStopMotionToggle(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            if (payload.Settings == null || payload.Settings.Count == 0)
            {
                settings = PluginSettings.CreateDefaultSettings();
            }
            else
            {
                settings = payload.Settings.ToObject<PluginSettings>();
                HandleFilenames();
            }
            HandleFilenames();
            SetNotRunning();
        }


        public override void KeyPressed(KeyPayload payload) { }

        public override void KeyReleased(KeyPayload payload)
        {
            ToggleStartStop();
        }

        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Tools.AutoPopulateSettings(settings, payload.Settings);
            HandleFilenames();
            SaveSettings();
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        public override async void OnTick()
        {
            try
            {
                if ((DateTime.Now - lastRefresh).TotalSeconds >= settings.CheckInterval)
                {
                    try
                    {
                        PipeServerConnection.Instance.RestartChannel();
                        var isRunning = PipeServerConnection.Instance.Channel.IsRunning();
                        if (isRunning)
                        {
                            await SetRunning();
                        }
                        else
                        {
                            await SetNotRunning();
                        }
                    }
                    catch (EndpointNotFoundException endpointNotFoundException)
                    {
                        await SetError();
                    }
                    catch (CommunicationObjectFaultedException communicationObjectFaultedException)
                    {
                        await SetError();
                    }
                    catch (Exception ex)
                    {
                        await SetError();
                    }

                    lastRefresh = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.ERROR, $"OnTick error: {ex}");
            }
        }

        public override void Dispose() { }



        #region Private Methods
        private void HandleFilenames()
        {
            primaryFile = Tools.FileToBase64(settings.PrimaryImageFilename, true);
            secondaryFile = Tools.FileToBase64(settings.SecondaryImageFilename, true);
            Connection.SetSettingsAsync(JObject.FromObject(settings));
        }

        private async Task SetError()
        {
            await SetNotRunning();
        }

        private async Task SetNotRunning()
        {
            if (!String.IsNullOrWhiteSpace(primaryFile))
            {

                await Connection.SetImageAsync(primaryFile);
                await Connection.SetTitleAsync("");
            }
            else
            {
                await Connection.SetTitleAsync("Not running");
            }
            ButtonIsCurrentlyPressed = false;
        }

        private async Task SetRunning()
        {
            if (!String.IsNullOrWhiteSpace(secondaryFile))
            {
                await Connection.SetImageAsync(secondaryFile);
                await Connection.SetTitleAsync("");
            }
            else
            {
                await Connection.SetTitleAsync("Running");
            }
            ButtonIsCurrentlyPressed = true;
        }

        private Task SaveSettings()
        {
            return Connection.SetSettingsAsync(JObject.FromObject(settings));
        }

        #endregion
    }
}
