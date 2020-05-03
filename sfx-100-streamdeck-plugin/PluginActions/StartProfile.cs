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
    [PluginActionId("sfx-100-streamdeck-plugin.startprofile")]
    public class StartProfile : PluginBase
    {
        private class PluginSettings
        {
            public static PluginSettings CreateDefaultSettings()
            {
                PluginSettings instance = new PluginSettings();
                instance.ProfileToLoad = string.Empty;
                instance.SfbExe = string.Empty;
                instance.SfbWindowStyle = "Normal";
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
            
            [JsonProperty(PropertyName = "sfbWindowStyle")]
            public string SfbWindowStyle { get; set; }

        }

        #region Private Members

        private PluginSettings settings;
        private bool actionInProgress = false;

        #endregion
        public StartProfile(SDConnection connection, InitialPayload payload) : base(connection, payload)
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
                
                Logger.Instance.LogMessage(TracingLevel.INFO, "Shutting down SimFeedback Process");
                var processPath = process.MainModule.FileName;
                process.EnableRaisingEvents = true;

                process.Exited += delegate (object o, EventArgs args)
                {
                    Logger.Instance.LogMessage(TracingLevel.INFO, "Shutting down SimFeedback Process completed. Exit Code: " + process.ExitCode);

                    // Setting active profile now:

                    SetProfileActive(settings.ProfileToLoad);
                    Logger.Instance.LogMessage(TracingLevel.INFO, "Starting SFB...");
                    StartSFB();
                    actionInProgress = false;
                };
                
                DateTime start = DateTime.Now; 
                while (!process.HasExited && DateTime.Now - start <= TimeSpan.FromSeconds(15))
                {
                    process.WaitForInputIdle();
                    process.CloseMainWindow();
                    Thread.Sleep(500);
                }
            }


            if (!processFound)
            {
                SetProfileActive(settings.ProfileToLoad);
                StartSFB();
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
            if (settings.SfbWindowStyle != "")
            {
                var windowStyle = (ProcessWindowStyle) Enum.Parse(typeof(ProcessWindowStyle), settings.SfbWindowStyle);
                psi.WindowStyle = windowStyle;
            }
            
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

        private void SetProfileActive(string theProfile)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "SetProfileActive: " + theProfile);
            if (!String.IsNullOrWhiteSpace(theProfile))
            {
                var profileDir = Path.GetDirectoryName(theProfile);
                var profileFiles = Directory.GetFiles(profileDir, "*.xml", SearchOption.AllDirectories);

                foreach (var xmlFile in profileFiles)
                {
                    var fullFilePath = Path.GetFullPath(xmlFile);

                    try
                    {
                        string xmlContents = File.ReadAllText(fullFilePath);
                        if (Path.GetFullPath(theProfile) == fullFilePath)
                        {
                            // Set desired profile active
                            xmlContents = xmlContents.Replace("Active=\"false\"", "Active=\"true\"");
                        }
                        else
                        {
                            // Set all other profiles inactive
                            xmlContents = xmlContents.Replace("Active=\"true\"", "Active=\"false\"");
                        }

                        File.WriteAllText(fullFilePath, xmlContents);

                        /* XML - Manipulation was not possible since the files can be defective by SFB
                        var doc = XDocument.Load(Path.GetFullPath(xmlFile));
                        if (Path.GetFullPath(theProfile) == Path.GetFullPath(xmlFile))
                        {
                            // Set desired profile active
                            doc.Root.Attribute("Active").Value = "true";
                        }
                        else
                        {
                            // Set all other profiles inactive
                            doc.Root.Attribute("Active").Value = "false";
                        }
                        doc.Save(xmlFile);
                        */
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.LogMessage(TracingLevel.ERROR,
                            "Error during Manipulation of: " + xmlFile + "Error:" + ex.Message);
                    }

                }
            }
        }

        private Task SaveSettings()
        {
            return Connection.SetSettingsAsync(JObject.FromObject(settings));
        }

        #endregion
    }
}
