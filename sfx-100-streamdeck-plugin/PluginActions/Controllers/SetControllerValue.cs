using System;
using System.Drawing;
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
                instance.showValueAfterChange = false;
                return instance;
            }

            [JsonProperty(PropertyName = "ControllerName")]
            public string ControllerName { get; set; }

            [JsonProperty(PropertyName = "Value")]
            public int Value { get; set; }

            [JsonProperty(PropertyName = "ValueToChange")]
            public string ValueToChange { get; set; }

            [JsonProperty(PropertyName = "showValueAfterChange")]
            public bool showValueAfterChange { get; set; }
        }

        #region Private Members

        private PluginSettings settings;
        private bool _valueShown;
        private int _valueShownTimeout = 1;
        private DateTime _valueShownDateTime = DateTime.Now;

        #endregion
        public SetControllerValue(SDConnection connection, InitialPayload payload) : base(connection, payload)
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
        {
            try
            {
                var returnVal = -1;
                PipeServerConnection.Instance.RestartChannel();
                switch (settings.ValueToChange)
                {
                    case "Intensity":
                        returnVal = PipeServerConnection.Instance.Channel.ControllerIntensitySet(settings.ControllerName, settings.Value);
                        break;

                    case "Smoothness":
                        returnVal = PipeServerConnection.Instance.Channel.ControllerSmoothnessSet(settings.ControllerName, settings.Value);
                        break;

                    case "Acceleration":
                        returnVal = PipeServerConnection.Instance.Channel.ControllerAccelerationSet(settings.ControllerName, settings.Value);
                        break;

                    case "MinSpeed":
                        returnVal = PipeServerConnection.Instance.Channel.ControllerMinSpeedSet(settings.ControllerName, settings.Value);
                        break;

                    case "MaxSpeed":
                        returnVal = PipeServerConnection.Instance.Channel.ControllerMaxSpeedSet(settings.ControllerName, settings.Value);
                        break;

                    default:
                        Logger.Instance.LogMessage(TracingLevel.ERROR, "Error: ValueToChange not set correctly: " + settings.ValueToChange);
                        break;
                }

                if (settings.showValueAfterChange)
                {
                    DrawValueData(returnVal);
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

        public override void OnTick()
        {
            if (_valueShown && DateTime.Now - _valueShownDateTime > TimeSpan.FromSeconds(_valueShownTimeout))
            {
                RestoreImage();
            }
        }

        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Tools.AutoPopulateSettings(settings, payload.Settings);
            SaveSettings();
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        #region Private Methods
        private async Task DrawValueData(int value)
        {

            try
            {
                var ForegroundColor = "#ffffff";
                var BackgroundColor = "#000000";

                Bitmap bmp = Tools.GenerateGenericKeyImage(out Graphics graphics);
                int height = bmp.Height;
                int width = bmp.Width;

                SizeF stringSize;
                float stringPos;
                var fontDefault = new Font("Verdana", 30, FontStyle.Bold);

                // Background
                var bgBrush = new SolidBrush(ColorTranslator.FromHtml(BackgroundColor));
                var fgBrush = new SolidBrush(ColorTranslator.FromHtml(ForegroundColor));
                graphics.FillRectangle(bgBrush, 0, 0, width, height);

                // Top title
                string title = "";
                stringSize = graphics.MeasureString(title, fontDefault);
                stringPos = Math.Abs((width - stringSize.Width)) / 2;
                graphics.DrawString(title, fontDefault, fgBrush, new PointF(stringPos, 5));

                string currStr = value.ToString();
                int buffer = 0;

                stringSize = graphics.MeasureString(currStr, fontDefault);
                stringPos = Math.Abs((width - stringSize.Width)) / 2;
                graphics.DrawString(currStr, fontDefault, fgBrush, new PointF(stringPos, 50));
                Connection.SetImageAsync(bmp);
                graphics.Dispose();
                _valueShown = true;
                _valueShownDateTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.ERROR, $"Error drawing image data {ex}");
            }
        }
        private async Task RestoreImage()
        {
            await Connection.SetDefaultImageAsync();
        }
        private Task SaveSettings()
        {
            return Connection.SetSettingsAsync(JObject.FromObject(settings));
        }

        #endregion
    }
}
