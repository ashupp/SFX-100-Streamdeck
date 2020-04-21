using System;
using System.Diagnostics;
using BarRaider.SdTools;

namespace sfx_100_streamdeck_plugin.PluginActions
{
    [PluginActionId("sfx-100-streamdeck-plugin.exitsimfeedback")]
    public class ExitSimFeedback : PluginBase
    {
        bool _actionInProgress = false;

        public ExitSimFeedback(SDConnection connection, InitialPayload payload) : base(connection, payload) { }

        public override void Dispose() { }

        public override void KeyPressed(KeyPayload payload) { }

        public override void KeyReleased(KeyPayload payload)
        {
            if (!_actionInProgress)
            {
                _actionInProgress = true;
                foreach (var process in Process.GetProcessesByName("SimFeedbackStart"))
                {
                    Logger.Instance.LogMessage(TracingLevel.INFO, "Shutting down SimFeedback Process");
                    process.EnableRaisingEvents = true;

                    process.Exited += delegate (object o, EventArgs args)
                    {
                        Logger.Instance.LogMessage(TracingLevel.INFO, "Shutting down SimFeedback Process completed. Exit Code: " + process.ExitCode);
                        _actionInProgress = false;
                    };

                    DateTime start = DateTime.Now;
                    while (!process.HasExited && DateTime.Now - start <= TimeSpan.FromSeconds(15))
                    {
                        process.WaitForInputIdle();
                        process.CloseMainWindow();
                    }
                }
                _actionInProgress = false;
            }
        }

        public override void OnTick() { }

        public override void ReceivedSettings(ReceivedSettingsPayload payload) { }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

    }
}
