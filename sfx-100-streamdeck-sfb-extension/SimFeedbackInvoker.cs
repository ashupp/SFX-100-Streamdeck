using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Xml.Linq;
using sfx_100_streamdeck_sfb_extension.Properties;
using WpfApp2;

namespace sfx_100_streamdeck_sfb_extension
{
    sealed class SimFeedbackInvoker
    {
        #region Singleton

        private static readonly Lazy<SimFeedbackInvoker> lazy
            = new Lazy<SimFeedbackInvoker>(() => new SimFeedbackInvoker());

        public static SimFeedbackInvoker Instance
            => lazy.Value;

        private SimFeedbackInvoker() { }
        #endregion


        public ActionElements actionElements;
        private const uint WM_COMMAND = 0x0111;
        private const int BN_CLICKED = 245;
        private const int IDOK = 1;
        private const UInt32 WM_GETTEXTLENGTH = 0x000E;
        private const UInt32 WM_GETTEXT = 0x000D;
        public AutomationElement profilePanel;

        private Process _currProcess;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [Out] StringBuilder lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);


        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public IntPtr GetHandleWindow(string title)
        {
            return FindWindow(null, title);
        }

        public enum WMessages : uint
        {
            BM_CLICK = 0x00F5,          // Click like on a Button

            TBM_GETRANGEMAX = 0x0402,   // Get max range
            TBM_GETRANGEMIN = 0x0401,   // Get min range
            TBM_GETPOS = 0x0400,        // Trackbar get position
            TBM_SETPOS = 0x0405,        // Trackbar set position
            TBM_SETPOSNOTIFY = 0x0422,  // Trackbar set position with notify
        }


        public void ClickElement(IntPtr hwndChild)
        {
            SendMessage(hwndChild, (uint)WMessages.BM_CLICK, 0, 0);
        }

        public int GetSliderMinRange(IntPtr hwndChild)
        {
            var val = SendMessage(hwndChild, (uint)WMessages.TBM_GETRANGEMIN, 0, 0);
            return val;
        }

        public int GetSliderMaxRange(IntPtr hwndChild)
        {
            var val = SendMessage(hwndChild, (uint)WMessages.TBM_GETRANGEMAX, 0, 0);
            return val;
        }

        public int GetCurrentSliderValue(IntPtr hwndChild)
        {
            GuiLoggerProvider.Instance.Log("Get value from id: " + hwndChild);
            var val = SendMessage(hwndChild, (uint)WMessages.TBM_GETPOS, 0, 0);
            GuiLoggerProvider.Instance.Log("Value retrieved: " + val);
            return val;
        }

        public void SetCurrentSliderValue(IntPtr hwndChild, int valueToSet)
        {
            GuiLoggerProvider.Instance.Log("Try set value: " + valueToSet + " for id: " + hwndChild);
            var val = SendMessage(hwndChild, (uint)WMessages.TBM_SETPOSNOTIFY, 1, valueToSet);
            GuiLoggerProvider.Instance.Log("Value set!");
        }


        public void LoadElements()
        {

            try
            {
                GuiLoggerProvider.Instance.Log("Loading Elements for UI Automation");

                //var mainWindowHandle = GetHandleWindow("SimFeedback - 00.09.08");
                _currProcess = Process.GetCurrentProcess();
                var mainWindowHandle = _currProcess.MainWindowHandle;

                GuiLoggerProvider.Instance.Log("Aktuelles Window handle: " + mainWindowHandle);

                actionElements = new ActionElements();


                if ((int)mainWindowHandle != 0)
                {

                    GuiLoggerProvider.Instance.Log("Loading..");
                    
                    
                    AutomationElement root = AutomationElement.FromHandle(mainWindowHandle);

                    GuiLoggerProvider.Instance.Log("Root Elem: " + root.Current.Name);


                    root = AutomationElement.FromHandle(mainWindowHandle);
                    //Instance.profilePanel = root.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, "groupBox2"));
                    Instance.profilePanel = root.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, "profileView"));

                    Instance.actionElements = new ActionElements();

                    GetActiveProfileEffects();

                    loadEffects(root);

                    loadControllerObjects(root);

                    loadEffectOverallSettings(root);

                    AddProfileChangeDetector();
                }

                GuiLoggerProvider.Instance.Log("Controller keys available: " + String.Join(", ", Instance.actionElements.Controllers.Keys));
                GuiLoggerProvider.Instance.Log("Effect keys available: " + String.Join(", ", Instance.actionElements.Effects.Keys));
                GuiLoggerProvider.Instance.Log("Finished Loading Elements for UI Automation");
            }
            catch (Exception e)
            {
                GuiLoggerProvider.Instance.Log("Error during loading Elements for UI Automation. Try Reload button.");
            }
        }

        private bool checkForElements(AutomationElement root)
        {

            AutomationElementCollection effectPanels = null;
            AutomationElementCollection controllerConfigControls = null;
            AutomationElement elt = null;
            int checkCounter = 0;
            do
            {
                GuiLoggerProvider.Instance.Log("Checking availability of AutomationElements...");

                // Effekte
                effectPanels = root.FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, "ChartControl"));
                // Das hier sind die Einstellungen pro Effekt


                // Controller 
                elt = root.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, "tableLayoutPanel1"));
                if (elt != null)
                {
                    controllerConfigControls = elt.FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, "ControllerConfigControl"));
                }


                GuiLoggerProvider.Instance.Log("Finished availability of AutomationElements...");
             Thread.Sleep(500);
                checkCounter++;

            } while ((effectPanels == null || elt == null || controllerConfigControls == null) && checkCounter < 10);
           
            if (effectPanels != null && elt != null && controllerConfigControls != null)
            {
                GuiLoggerProvider.Instance.Log("AutomationElements found...");
                return true;
            }

            GuiLoggerProvider.Instance.Log("Error: No AutomationElements found...");
            return false;
        }

        private void AddProfileChangeDetector()
        {
            try
            {
                Automation.AddStructureChangedEventHandler(Instance.profilePanel, TreeScope.Element, async delegate (object o, StructureChangedEventArgs args)
                {
                    AutomationElement element = o as AutomationElement;

                    if (args.StructureChangeType == StructureChangeType.ChildRemoved)
                    {
                        SimFeedbackFacadeProvider.Instance.DispatcherHelper.Invoke((Action)(() =>
                        {
                            GuiLoggerProvider.Instance.Log("Profile Change detected...");
                            Instance.SelectProfileTab();
                            Instance.LoadElements();
                        }));

                    }
                });
            }
            catch (Exception ex)
            {
                GuiLoggerProvider.Instance.Log("Error during loading of AddProfileChangeDetector: " + ex.Message);
            }
        }

        private static void GetActiveProfileEffects()
        {

            string path = Directory.GetCurrentDirectory();
            var profileDir = Path.Combine(path,"profiles");

            var profileFiles = Directory.GetFiles(profileDir, "*.xml", SearchOption.AllDirectories);

            // Herausfinden welches Profil aktiv ist
            foreach (var xmlFile in profileFiles)
            {
                var fullFilePath = Path.GetFullPath(xmlFile);

                try
                {
                    string xmlContents = File.ReadAllText(fullFilePath);
                    if (xmlContents.Contains("Active=\"true\""))
                    {
                        GuiLoggerProvider.Instance.Log("Active Profile: " + fullFilePath);


                        // Jetzt noch die Effekte auslesen


                        // Effekte laden
                        var xmlEffectsStartStringId = "<FeedbackEffectList>";
                        var xmlEffectsEndStringId = "</FeedbackEffectList>";
                        int pFromE = xmlContents.IndexOf(xmlEffectsStartStringId);
                        int pToE = xmlContents.LastIndexOf(xmlEffectsEndStringId) + xmlEffectsEndStringId.Length;

                        String resultE = xmlContents.Substring(pFromE, pToE - pFromE);
                        //GuiLoggerProvider.Instance.Log(resultE);
                        var docE = XDocument.Parse(resultE);
                        SimFeedbackInvoker.Instance.actionElements.EffectIndexAssignment.Clear();
                        foreach (var ctrlElem in docE.Root.Elements())
                        {
                            var effectName = ctrlElem.Element("name").Value;
                            GuiLoggerProvider.Instance.Log("XML - Effect found: " + effectName);

                            //SimFeedbackInvoker.Instance.actionElements.Effects.Add( effectName, null);
                            SimFeedbackInvoker.Instance.actionElements.EffectIndexAssignment.Add(effectName);
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    GuiLoggerProvider.Instance.Log("Error during Reading of: " + xmlFile + "Error:" + ex.Message);
                }

            }

            var pathSFbXml = Path.Combine(Directory.GetCurrentDirectory(), "SimFeedback.xml");
            try
            {

                // Verfügbare Controller laden
                string xmlContentsSfbXml = File.ReadAllText(pathSFbXml);

                var xmlControllerStartStringId = "<MotionControllerList>";
                var xmlControllerEndStringId = "</MotionControllerList>";
                int pFrom = xmlContentsSfbXml.IndexOf(xmlControllerStartStringId);
                int pTo = xmlContentsSfbXml.LastIndexOf(xmlControllerEndStringId) + xmlControllerEndStringId.Length;

                String result = xmlContentsSfbXml.Substring(pFrom, pTo - pFrom);
                //GuiLoggerProvider.Instance.Log(result);
                var doc = XDocument.Parse(result);
                SimFeedbackInvoker.Instance.actionElements.ControllerIndexAssignment.Clear();
                foreach (var ctrlElem in doc.Root.Elements())
                {
                    var controllerName = ctrlElem.Element("type").Value;
                    GuiLoggerProvider.Instance.Log("XML - Controller-Type found: " + controllerName);

                    //SimFeedbackInvoker.Instance.actionElements.Controllers.Add(controllerName, null);
                    SimFeedbackInvoker.Instance.actionElements.ControllerIndexAssignment.Add(controllerName);
                }
            }
            catch (Exception ex)
            {
                GuiLoggerProvider.Instance.Log("Error during Reading of: " + pathSFbXml + "Error:" + ex.Message);
            }

        }

        private static void loadEffects(AutomationElement root)
        {
            try
            {
                Instance.actionElements.Effects.Clear();

                //WalkControlElements(root);
                var effectPanels = root.FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, "ChartControl"));

                // Das hier sind die Einstellungen pro Effekt

                GuiLoggerProvider.Instance.Log("Effects found: " + effectPanels.Count);

                int i = 0;
                foreach (AutomationElement effectPanel in effectPanels)
                {
                    var effectActionElement = new EffectActionElement();

                    // Effect Name from XML
                    effectActionElement.Name = Instance.actionElements.EffectIndexAssignment[i];

                    // Effect intensity 
                    var trackBarIntensity = effectPanel.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, "trackBarIntensity"));
                    effectActionElement.Intensity = trackBarIntensity;

                    // Effect smoothing
                    var trackBarSmoothing = effectPanel.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, "trackBarSmooth"));
                    effectActionElement.Smoothing = trackBarSmoothing;

                    // Checkbox Linear
                    var checkBoxLinearInterpolate = effectPanel.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, "checkBoxLinearInterpolate"));
                    effectActionElement.LinearInterpolated = checkBoxLinearInterpolate;

                    // Checkbox Enabled
                    var checkBoxEnableEffect = effectPanel.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, "enableCheckBox"));
                    effectActionElement.Enabled = checkBoxEnableEffect;

                    // Checkbox Muted
                    var checkBoxMute = effectPanel.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, "muteCheckBox"));
                    effectActionElement.Muted = checkBoxMute;

                    GuiLoggerProvider.Instance.Log("Add Effect: " + effectActionElement.Name);
                    Instance.actionElements.Effects.Add(effectActionElement.Name, effectActionElement);
                    i++;
                }
            }
            catch (Exception e)
            {
                GuiLoggerProvider.Instance.Log("Error during loading of effects: " + e.Message);
            }

        }

        private void loadEffectOverallSettings(AutomationElement root)
        {
            try
            {
                var elt = root.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, "tableLayoutPanel1"));

                var trackBarOverallIntensity = elt.FindFirst(TreeScope.Subtree,
                    new PropertyCondition(AutomationElement.AutomationIdProperty, "trackBarOverallIntensity"));
                actionElements.OverallIntensity = trackBarOverallIntensity;

                var buttonResetOverallIntensity = elt.FindFirst(TreeScope.Subtree,
                    new PropertyCondition(AutomationElement.AutomationIdProperty, "button3"));
                actionElements.BtnResetOverallIntensity = buttonResetOverallIntensity;

                var buttonEnableAllEffects = elt.FindFirst(TreeScope.Subtree,
                    new PropertyCondition(AutomationElement.AutomationIdProperty, "buttonEnableAllEffects"));
                actionElements.BtnEnableAllEffects = buttonEnableAllEffects;

                var buttonDisableAllEffects = elt.FindFirst(TreeScope.Subtree,
                    new PropertyCondition(AutomationElement.AutomationIdProperty, "buttonDisableAllEffects"));
                actionElements.BtnDisableAllEffects = buttonDisableAllEffects;
            }
            catch (Exception e)
            {
                GuiLoggerProvider.Instance.Log("Error during loading of OverallSettings: " + e.Message);
            }
        }

        private static void loadControllerObjects(AutomationElement root)
        {
            Instance.actionElements.Controllers.Clear();
            
            try
            {
                var elt = root.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, "tableLayoutPanel1"));
                var controllerConfigControls = elt.FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, "ControllerConfigControl"));


                GuiLoggerProvider.Instance.Log("Controllers found: " + controllerConfigControls.Count);

                int i = 0;
                // Alle Controller
                foreach (AutomationElement controllerConfigControl in controllerConfigControls)
                {
                    ControllerActionElement controllerActionElements = new ControllerActionElement();
                    // Controller name            GuiLoggerProvider.Instance.Log("Checking availability of AutomationElements...");

                    var controllerName = controllerConfigControl.FindFirst(TreeScope.Children,
                        new PropertyCondition(AutomationElement.AutomationIdProperty, "labelIdType"));
                    var tmpName = AutomationExtensions.GetText(controllerName);
                    GuiLoggerProvider.Instance.Log("UI Controller name: " + tmpName);


                    controllerActionElements.Name = Instance.actionElements.ControllerIndexAssignment[i];
                    GuiLoggerProvider.Instance.Log("Desired Controller name: " + Instance.actionElements.ControllerIndexAssignment[i]);

                    // Controller intensity 
                    var trackBarIntensity = controllerConfigControl.FindFirst(TreeScope.Children,
                        new PropertyCondition(AutomationElement.AutomationIdProperty, "trackBarIntensity"));
                    controllerActionElements.Intensity = trackBarIntensity;


                    // Button controller intensity reset
                    var buttonControllerIntensityReset = controllerConfigControl.FindFirst(TreeScope.Children,
                        new PropertyCondition(AutomationElement.AutomationIdProperty, "buttonIntensityReset"));
                    controllerActionElements.IntensityResetButton = buttonControllerIntensityReset;

                    // Checkbox controller smoothness
                    var checkboxControllerSmoothness = controllerConfigControl.FindFirst(TreeScope.Children,
                        new PropertyCondition(AutomationElement.AutomationIdProperty, "checkBoxOverallSmoothness"));
                    controllerActionElements.SmoothnessEnabled = checkboxControllerSmoothness;

                    // Controller smoothness 
                    var trackBarControllerSmoothness = controllerConfigControl.FindFirst(TreeScope.Children,
                        new PropertyCondition(AutomationElement.AutomationIdProperty, "trackBarSmoothness"));
                    controllerActionElements.Smoothness = trackBarControllerSmoothness;

                    // Controller acceleration 
                    var trackBarControllerAcceleration = controllerConfigControl.FindFirst(TreeScope.Children,
                        new PropertyCondition(AutomationElement.AutomationIdProperty, "trackBarAcceleration"));
                    controllerActionElements.Acceleration = trackBarControllerAcceleration;

                    // Controller min speed 
                    var trackBarControllerMinSpeed = controllerConfigControl.FindFirst(TreeScope.Children,
                        new PropertyCondition(AutomationElement.AutomationIdProperty, "trackBarMinSpeed"));
                    controllerActionElements.MinSpeed = trackBarControllerMinSpeed;

                    // Controller max speed 
                    var trackBarControllerMaxSpeed = controllerConfigControl.FindFirst(TreeScope.Children,
                        new PropertyCondition(AutomationElement.AutomationIdProperty, "trackBarMaxSpeed"));
                    controllerActionElements.MaxSpeed = trackBarControllerMaxSpeed;

                    GuiLoggerProvider.Instance.Log("Add Controller: " + controllerActionElements.Name);
                    Instance.actionElements.Controllers.Add(controllerActionElements.Name, controllerActionElements);
                    i++;
                }
            }
            catch (Exception e)
            {
                GuiLoggerProvider.Instance.Log("Error during loading of Controller: " + e.Message);
            }
        }

        public void SelectProfileTab()
        {

            var process = System.Diagnostics.Process.GetProcessesByName("SimFeedbackStart").FirstOrDefault();
            if (process != null)
            {
                GuiLoggerProvider.Instance.Log("Current Process: " + process.ProcessName);
                GuiLoggerProvider.Instance.Log("Current Window handle: " + process.MainWindowHandle);
            }
            else
            {
                GuiLoggerProvider.Instance.Log("No process found");
            }

            ActionElements actionElements = new ActionElements();



            if (process != null && process.MainWindowHandle != null)
            {

                GuiLoggerProvider.Instance.Log("Loading..");
                var root = AutomationElement.FromHandle(process.MainWindowHandle);
                GuiLoggerProvider.Instance.Log("root?: " + root.Current.Name);

                // get tab bar
                    var tab = root.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Tab));

                // get tab
                var item = tab.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Profiles"));
                if (item == null)
                {
                    GuiLoggerProvider.Instance.Log("Tab item '" + "Profiles" + "' has been closed.");
                    return;
                }
                // select it
                ((SelectionItemPattern)item.GetCurrentPattern(SelectionItemPattern.Pattern)).Select();
                GuiLoggerProvider.Instance.Log("Tab selected");
            }
          
        }
        
        public async Task LoadWithDelay()
        {
            await Task.Delay(Settings.Default.UIAutomationDelay);
            GuiLoggerProvider.Instance.Log("Load UIAutomation with delay..." + Settings.Default.UIAutomationDelay);
            await SimFeedbackFacadeProvider.Instance.DispatcherHelper.BeginInvoke((Action)(() =>
            {
                SimFeedbackInvoker.Instance.SelectProfileTab();
                Instance.LoadElements();
            }));
        }
    }
}
