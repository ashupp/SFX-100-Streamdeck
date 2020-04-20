using System;
using System.ServiceModel;
using System.Threading.Tasks;
using BarRaider.SdTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace sfx_100_streamdeck_plugin.PluginActions
{
    [PluginActionId("sfx-100-streamdeck-plugin.isrunning")]
    public class IsRunning : PluginBase
    {
        private class PluginSettings
        {
            public static PluginSettings CreateDefaultSettings()
            {
                PluginSettings instance = new PluginSettings();
                instance.PrimaryImageFilename = string.Empty;
                instance.SecondaryImageFilename = string.Empty;
                instance.CheckInterval = "10";
                return instance;
            }

            [FilenameProperty]
            [JsonProperty(PropertyName = "primaryImage")]
            public string PrimaryImageFilename { get; set; }

            [FilenameProperty]
            [JsonProperty(PropertyName = "secondaryImage")]
            public string SecondaryImageFilename { get; set; }

            [JsonProperty(PropertyName = "checkInterval")]
            public string CheckInterval { get; set; }
        }

        private PluginSettings settings;
        string primaryFile = null;
        string secondaryFile = null;
        DateTime lastRefresh;

        public IsRunning(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            if (payload.Settings == null || payload.Settings.Count == 0)
            {
                settings = PluginSettings.CreateDefaultSettings();
                Connection.SetSettingsAsync(JObject.FromObject(settings));
            }
            else
            {
                settings = payload.Settings.ToObject<PluginSettings>();
                HandleFilenames();
            }
        }

        public override void Dispose() { }

        public override void KeyPressed(KeyPayload payload) { }

        public override void KeyReleased(KeyPayload payload) { }

        public override async void OnTick()
        {
            try
            {
                if ((DateTime.Now - lastRefresh).TotalSeconds >= Convert.ToInt32(settings.CheckInterval))
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
                        Logger.Instance.LogMessage(TracingLevel.ERROR, "Error: Endpoint not found - Is SimFeedback available and is the Plugin enabled? " + endpointNotFoundException.Message);
                        await SetError();
                    }
                    catch (CommunicationObjectFaultedException communicationObjectFaultedException)
                    {
                        Logger.Instance.LogMessage(TracingLevel.ERROR, "Error: communicationObjectFaultedException: " + communicationObjectFaultedException.Message);
                        await SetError();
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.LogMessage(TracingLevel.ERROR, "Error during Key processing: " + ex.Message);
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

        private async Task SetError()
        {
            await Connection.SetTitleAsync("Error");
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
        }


        private void CheckStatus()
        {

        }

        private void HandleFilenames()
        {
            primaryFile = Tools.FileToBase64(settings.PrimaryImageFilename, true);
            secondaryFile = Tools.FileToBase64(settings.SecondaryImageFilename, true);
            Connection.SetSettingsAsync(JObject.FromObject(settings));
        }


        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Tools.AutoPopulateSettings(settings, payload.Settings);
            HandleFilenames();
            SaveSettings();
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }


        #region Private Methods

        private Task SaveSettings()
        {
            return Connection.SetSettingsAsync(JObject.FromObject(settings));
        }

        #endregion
    }
}
