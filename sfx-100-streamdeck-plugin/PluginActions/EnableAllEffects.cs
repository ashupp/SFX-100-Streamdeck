using BarRaider.SdTools;
using NPCommunication;


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
            var client = new NPClient("sfx100streamdeck", "sfx100streamdeck");
            client.Get("EnableAllEffects");
        }

        public override void OnTick() { }

        public override void ReceivedSettings(ReceivedSettingsPayload payload) { }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }
    }
}
