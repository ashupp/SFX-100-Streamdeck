using BarRaider.SdTools;


namespace sfx_100_streamdeck_plugin.PluginActions
{
    [PluginActionId("sfx-100-streamdeck-plugin.enablealleffects")]
    public class EnableAllEffects : PluginBase
    {
        public EnableAllEffects(SDConnection connection, InitialPayload payload) : base(connection, payload) { }

        public override void Dispose() { }

        public override void KeyPressed(KeyPayload payload) { }

        public override void KeyReleased(KeyPayload payload)
        {
            if (PipeServerConnection.Instance.Channel.CheckConnection())
            {
                PipeServerConnection.Instance.Channel.EnableAllEffects();
            }
        }

        public override void OnTick() { }

        public override void ReceivedSettings(ReceivedSettingsPayload payload) { }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }
    }
}
