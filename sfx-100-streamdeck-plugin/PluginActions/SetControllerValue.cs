using System;
using System.Threading.Tasks;
using BarRaider.SdTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ServiceModel;


namespace sfx_100_streamdeck_plugin.PluginActions
{
    [PluginActionId("sfx-100-streamdeck-plugin.setcontrollervalue")]
    public class SetControllerValue : PluginBase
    {
        private class PluginSettings
        {
            public static PluginSettings CreateDefaultSettings()
            {
                PluginSettings instance = new PluginSettings();
                instance.ControllerName = "SFX";
                instance.ValueToChange = "Intensity";
                instance.Value = 1;
                return instance;
            }

            [JsonProperty(PropertyName = "ControllerName")]
            public string ControllerName { get; set; }

            [JsonProperty(PropertyName = "Value")]
            public int Value { get; set; }

            [JsonProperty(PropertyName = "ValueToChange")]
            public string ValueToChange { get; set; }
        }

        #region Private Members

        private PluginSettings settings;

        #endregion
        public SetControllerValue(SDConnection connection, InitialPayload payload) : base(connection, payload)
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

        public override void Dispose() { }

        public override void KeyPressed(KeyPayload payload) { }

        public override void KeyReleased(KeyPayload payload)
        {
            try
            {
                PipeServerConnection.Instance.RestartChannel();
                switch (settings.ValueToChange)
                {
                    case "Intensity":
                        PipeServerConnection.Instance.Channel.ControllerIntensitySet(settings.ControllerName, settings.Value);
                        break;

                    case "Smoothness":
                        PipeServerConnection.Instance.Channel.ControllerSmoothnessSet(settings.ControllerName, settings.Value);
                        break;

                    case "Acceleration":
                        PipeServerConnection.Instance.Channel.ControllerAccelerationSet(settings.ControllerName, settings.Value);
                        break;

                    case "MinSpeed":
                        PipeServerConnection.Instance.Channel.ControllerMinSpeedSet(settings.ControllerName, settings.Value);
                        break;

                    case "MaxSpeed":
                        PipeServerConnection.Instance.Channel.ControllerMaxSpeedSet(settings.ControllerName, settings.Value);
                        break;

                    default:
                        Logger.Instance.LogMessage(TracingLevel.ERROR, "Error: ValueToChange not set correctly: " + settings.ValueToChange);
                        break;
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

        public override void OnTick() { }

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
