using System;
using System.ServiceModel;
using BarRaider.SdTools;

namespace sfx_100_streamdeck_plugin.PluginActions
{
    [PluginActionId("sfx-100-streamdeck-plugin.startstopmotiontoggle")]
    class StartStopMotionToggle : PluginBase
    {
        #region Private Members
        public bool ButtonIsCurrentlyPressed;
        #endregion

        private void ToggleStartStop()
        {
            try {
                PipeServerConnection.Instance.RestartChannel();
                if (ButtonIsCurrentlyPressed || PipeServerConnection.Instance.Channel.IsRunning())
                {
                    PipeServerConnection.Instance.Channel.StopMotion();
                    Connection.SetStateAsync(0);
                    ButtonIsCurrentlyPressed = false;
                }
                else
                {
                    PipeServerConnection.Instance.Channel.StartMotion();
                    ButtonIsCurrentlyPressed = true;
                    Connection.SetStateAsync(1);
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

        public StartStopMotionToggle(SDConnection connection, InitialPayload payload) : base(connection, payload) { }


        public override void KeyPressed(KeyPayload payload) { }

        public override void KeyReleased(KeyPayload payload)
        {
            ToggleStartStop();
        }

        public override void ReceivedSettings(ReceivedSettingsPayload payload) { }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        public override void OnTick() { }

        public override void Dispose() { }
    }
}
