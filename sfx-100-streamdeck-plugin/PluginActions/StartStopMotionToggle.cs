using BarRaider.SdTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using NPCommunication;

namespace sfx_100_streamdeck_plugin.PluginActions
{
    [PluginActionId("sfx-100-streamdeck-plugin.startstopmotiontoggle")]
    class StartStopMotionToggle : PluginBase
    {
        #region Private Members
        private PluginSettings settings;
        public bool ButtonIsCurrentlyPressed;
        #endregion

        #region PluginSettings
        private class PluginSettings
        {
            public static PluginSettings CreateDefaultSettings()
            {
                PluginSettings instance = new PluginSettings();
                return instance;
            }
        }
        #endregion


        private void ToggleJoystickButton()
        {
            var client = new NPClient("sfx100streamdeck", "sfx100streamdeck");
            if (ButtonIsCurrentlyPressed)
            {
                if (client.Get("Stop") == "ok")
                {
                    Connection.SetStateAsync(0);
                    ButtonIsCurrentlyPressed = false;
                }
                else
                {
                    Connection.SetStateAsync(1);
                }
            }
            else
            {
                if (client.Get("Start") == "ok")
                {
                    ButtonIsCurrentlyPressed = true;
                    Connection.SetStateAsync(1);
                }
                else
                {
                    Connection.SetStateAsync(0);
                }
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
            }
        }


        public override void KeyPressed(KeyPayload payload)
        {
        }

        public override void KeyReleased(KeyPayload payload)
        {
            ToggleJoystickButton();
        }

        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Tools.AutoPopulateSettings(settings, payload.Settings);
            SaveSettings();
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        public override void OnTick() { }

        public override void Dispose()
        {
        }

        private void SaveSettings()
        {
            Connection.SetSettingsAsync(JObject.FromObject(settings));
        }
    }
}
