using BarRaider.SdTools;

namespace sfx_100_streamdeck_plugin.PluginActions
{
    [PluginActionId("sfx-100-streamdeck-plugin.startstopmotiontoggle")]
    class StartStopMotionToggle : PluginBase
    {
        #region Private Members
        public bool ButtonIsCurrentlyPressed;
        #endregion

        private void ToggleJoystickButton()
        {
            if (PipeServerConnection.Instance.Channel.CheckConnection())
            {

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
        }

        public StartStopMotionToggle(SDConnection connection, InitialPayload payload) : base(connection, payload) { }


        public override void KeyPressed(KeyPayload payload) { }

        public override void KeyReleased(KeyPayload payload)
        {
            ToggleJoystickButton();
        }

        public override void ReceivedSettings(ReceivedSettingsPayload payload) { }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        public override void OnTick() { }

        public override void Dispose() { }
    }
}
