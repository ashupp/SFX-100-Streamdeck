using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BarRaider.SdTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ServiceModel;
using System.Threading;
using System.Xml.Linq;


namespace sfx_100_streamdeck_plugin.PluginActions
{
    [PluginActionId("sfx-100-streamdeck-plugin.startsimfeedback")]
    public class StartSimFeedback : PluginBase
    {
        private class PluginSettings
        {
            public static PluginSettings CreateDefaultSettings()
            {
                PluginSettings instance = new PluginSettings();
                instance.ProfileToLoad = string.Empty;
                instance.SfbExe = string.Empty;
                instance.StartSfbMinimized = false;
                instance.StartMotionAfterProfileLoaded = false;
                return instance;
            }

            [FilenameProperty]
            [JsonProperty(PropertyName = "profileToLoad")]
            public string ProfileToLoad { get; set; }

            [FilenameProperty]
            [JsonProperty(PropertyName = "sfbExe")]
            public string SfbExe { get; set; }

            [JsonProperty(PropertyName = "startMotionAfterProfileLoaded")]
            public bool StartMotionAfterProfileLoaded { get; set; }            
            
            [JsonProperty(PropertyName = "startSfbMinimized")]
            public bool StartSfbMinimized { get; set; }

        }

        #region Private Members

        private PluginSettings settings;
        private bool actionInProgress = false;

        #endregion
        public StartSimFeedback(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            if (payload.Settings == null || payload.Settings.Count == 0)
            {
                settings = PluginSettings.CreateDefaultSettings();
                Connection.SetSettingsAsync(JObject.FromObject(settings));
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
            if (actionInProgress)
            {
                Logger.Instance.LogMessage(TracingLevel.INFO, "Error: action already in Progress");
                return;
            }
            actionInProgress = true;

            Logger.Instance.LogMessage(TracingLevel.INFO, "Begin StartProfile action");

            // Stop if Motion is Running

            Logger.Instance.LogMessage(TracingLevel.INFO, "Motion currently running - will stopped during exit");
            //PipeServerConnection.Instance.Channel.StopMotion();
            // TODO: Warten bis Motion beendet ist


            bool processFound = false;

            // Shutdown SimFeedback
            foreach (var process in Process.GetProcessesByName("SimFeedbackStart"))
            {
                processFound = true;
            }

            if (!processFound)
            {
                StartSFB();
                actionInProgress = false;
            }
            else
            {
                actionInProgress = false;
            }
        }

        private void WaitStartMotion()
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "WaitStartMotion");
            bool connectionAvailable = false;
            DateTime start = DateTime.Now;
            while (!connectionAvailable && DateTime.Now - start <= TimeSpan.FromSeconds(15))
            {
                try
                {
                    // Wait for Pipe - no logging here 
                    try
                    {
                        PipeServerConnection.Instance.RestartChannel();
                        connectionAvailable = PipeServerConnection.Instance.Channel.CheckConnection();
                        if (connectionAvailable)
                        {
                            PipeServerConnection.Instance.Channel.StartMotion();
                            break;
                        }
                    }
                    catch (EndpointNotFoundException endpointNotFoundException)
                    { }
                    catch (CommunicationObjectFaultedException communicationObjectFaultedException)
                    { }
                    catch (Exception ex)
                    { }
                }
                catch
                {
                    Logger.Instance.LogMessage(TracingLevel.ERROR, "In loop, trying to Start motion");
                }
                Thread.Sleep(500);
            }
        }

        private void StartSFB()
        {
            // Start SFB
            ProcessStartInfo psi = new ProcessStartInfo(settings.SfbExe);
            psi.UseShellExecute = true;
            if(settings.StartSfbMinimized)
                psi.WindowStyle = ProcessWindowStyle.Minimized;
            psi.WorkingDirectory = Path.GetDirectoryName(settings.SfbExe);
            var newProc = Process.Start(psi);
            newProc.WaitForInputIdle();
            actionInProgress = false;
            if (settings.StartMotionAfterProfileLoaded)
                WaitStartMotion();
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
