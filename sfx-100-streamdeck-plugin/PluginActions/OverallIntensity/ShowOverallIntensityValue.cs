﻿using System;
using System.Threading.Tasks;
using BarRaider.SdTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ServiceModel;


namespace sfx_100_streamdeck_plugin.PluginActions
{
    [PluginActionId("sfx-100-streamdeck-plugin.showoverallintensityvalue")]
    public class ShowOverallIntensityValue : PluginBase
    {
        private class PluginSettings
        {
            public static PluginSettings CreateDefaultSettings()
            {
                PluginSettings instance = new PluginSettings();
                instance.RefreshRate = 1000;
                return instance;
            }

            [JsonProperty(PropertyName = "RefreshRate")]
            public int RefreshRate { get; set; }

        }

        #region Private Members

        private PluginSettings settings;
        private DateTime lastRefresh;

        #endregion
        public ShowOverallIntensityValue(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            if (payload.Settings == null || payload.Settings.Count == 0)
            {
                settings = PluginSettings.CreateDefaultSettings();
                SaveSettings();
            }
            else
            {
                settings = payload.Settings.ToObject<PluginSettings>();
            }
        }

        public override void Dispose() { }

        public override void KeyPressed(KeyPayload payload) { }

        public override void KeyReleased(KeyPayload payload)
        { }

        public override async void OnTick()
        {
            try
            {
                if ((DateTime.Now - lastRefresh).TotalMilliseconds >= settings.RefreshRate)
                {
                    PipeServerConnection.Instance.RestartChannel();

                    int currentValue = Int32.MaxValue;
                    currentValue = PipeServerConnection.Instance.Channel.GetOverallIntensity();
                    await setDisplayData(currentValue);
                    lastRefresh = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                // Empty because of spamming if pipe not accessible :)
            }
        }

        private async Task setDisplayData(int currentValue)
        {
            await Connection.SetTitleAsync(currentValue.ToString());
        }

        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Tools.AutoPopulateSettings(settings, payload.Settings);
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
