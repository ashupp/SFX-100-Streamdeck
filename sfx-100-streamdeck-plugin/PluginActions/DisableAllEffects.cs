using BarRaider.SdTools;
using NPCommunication;


namespace sfx_100_streamdeck_plugin.PluginActions
{
    [PluginActionId("sfx-100-streamdeck-plugin.disablealleffects")]
    public class DisableAllEffects : PluginBase
    {
        public DisableAllEffects(SDConnection connection, InitialPayload payload) : base(connection, payload) { }

        public override void Dispose() { }

        public override void KeyPressed(KeyPayload payload) { }

        public override void KeyReleased(KeyPayload payload)
        {
            var client = new NPClient("sfx100streamdeck", "sfx100streamdeck");
            client.Get("DisableAllEffects");
        }

        public override void OnTick() { }

        public override void ReceivedSettings(ReceivedSettingsPayload payload) { }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }
    }
}
