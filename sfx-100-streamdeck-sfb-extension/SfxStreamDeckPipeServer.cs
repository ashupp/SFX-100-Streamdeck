using System;
using System.ServiceModel;
using sfx_100_streamdeck_pipecontract;
using WpfApp2;

namespace sfx_100_streamdeck_sfb_extension
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    public class SfxStreamDeckPipeServer : ISfxStreamDeckPipeContract
    {
        public bool CheckConnection()
        {
            GuiLoggerProvider.Instance.Log("Incoming Command: CheckConnection");
            return true;
        }

        #region Running / TelemetryProvider States
        public bool IsRunning()
        {
            GuiLoggerProvider.Instance.Log("Incoming Command: IsRunning");
            var isRunning = SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.IsRunning();
            GuiLoggerProvider.Instance.Log("Returning: " + isRunning);
            return isRunning;

        }

        public bool IsTelemetryProviderConnected()
        {

            GuiLoggerProvider.Instance.Log("Incoming Command: IsTelemetryProviderConnected");
            var isTelemetryProviderConnected = SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.IsTelemetryProviderConnected();
            GuiLoggerProvider.Instance.Log("Returning: " + isTelemetryProviderConnected);
            return isTelemetryProviderConnected;
        }
        #endregion

        #region Start / Stop Motion
        public bool StartMotion()
        {
            try
            {
                GuiLoggerProvider.Instance.Log("Incoming Command: Start");
                SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool StopMotion()
        {
            try
            {
                GuiLoggerProvider.Instance.Log("Incoming Command: Stop");
                SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.Stop();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region All Effects Enable / Disable
        public bool EnableAllEffects()
        {
            try
            {
                GuiLoggerProvider.Instance.Log("Incoming Command: EnableAllEffects");
                SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.EnableAllEffects();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DisableAllEffects()
        {
            try
            {
                GuiLoggerProvider.Instance.Log("Incoming Command: DisableAllEffects");
                SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.DisableAllEffects();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region OverallIntensity
        public int IncrementOverallIntensity(int steps)
        {
            try
            {
                GuiLoggerProvider.Instance.Log("Incoming Command: IncrementOverallIntensity by " + steps);
                for (var i = steps - 1; i >= 0; i--)
                {
                    SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.IncrementOverallIntensity();
                }
                return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.OverallIntensity.Current.NativeWindowHandle);
            }
            catch
            {
                return -1;
            }
        }

        public int DecrementOverallIntensity(int steps)
        {
            try
            {
                GuiLoggerProvider.Instance.Log("Incoming Command: DecrementOverallIntensity by " + steps);
                for (var i = steps - 1; i >= 0; i--)
                {
                    SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.DecrementOverallIntensity();
                }
                return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.OverallIntensity.Current.NativeWindowHandle);
            }
            catch
            {
                return -1;
            }
        }

        public int SetOverallIntensity(int value)
        {
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr) SimFeedbackInvoker.Instance.actionElements.OverallIntensity.Current.NativeWindowHandle,value);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.OverallIntensity.Current.NativeWindowHandle);
        }

        public int GetOverallIntensity()
        {
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.OverallIntensity.Current.NativeWindowHandle);
        }

        public int ResetOverallIntensity()
        {
            SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.BtnResetOverallIntensity.Current.NativeWindowHandle);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.OverallIntensity.Current.NativeWindowHandle);
        }
        #endregion

        #region Intensity of Controller
        public int ControllerIntensityIncrement(string controllerName, int steps)
        {
            var hWnd = (IntPtr) SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Intensity.Current.NativeWindowHandle;
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue(hWnd);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue(hWnd, currVal + steps);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue(hWnd);
        }

        public int ControllerIntensityDecrement(string controllerName, int steps)
        {
            var hWnd = (IntPtr) SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Intensity.Current.NativeWindowHandle;
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue(hWnd);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue(hWnd, currVal - steps);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue(hWnd);
        }

        public int ControllerIntensitySet(string controllerName, int value)
        {
            GuiLoggerProvider.Instance.Log("Incoming Command: ControllerIntensitySet " + controllerName + " -- " + value);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Intensity.Current.NativeWindowHandle, value);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Intensity.Current.NativeWindowHandle);
        }

        public int ControllerIntensityGet(string controllerName)
        {
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr) SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Intensity.Current.NativeWindowHandle);
        }

        public int ControllerIntensityReset(string controllerName)
        {
            SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].IntensityResetButton.Current.NativeWindowHandle);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Intensity.Current.NativeWindowHandle);
        }
        #endregion

        #region Smoothing of Controller
        public int ControllerSmoothnessIncrement(string controllerName, int steps)
        {
            var currVal =SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Smoothness.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Smoothness.Current.NativeWindowHandle, currVal + steps);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Smoothness.Current.NativeWindowHandle);
        }

        public int ControllerSmoothnessDecrement(string controllerName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Smoothness.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Smoothness.Current.NativeWindowHandle, currVal - steps);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Smoothness.Current.NativeWindowHandle);
        }

        public int ControllerSmoothnessSet(string controllerName, int value)
        {
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Smoothness.Current.NativeWindowHandle, value);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Smoothness.Current.NativeWindowHandle);
        }

        public int ControllerSmoothnessGet(string controllerName)
        {
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Smoothness.Current.NativeWindowHandle);
        }

        public bool ControllerSmoothnessIsEnabled(string controllerName)
        {
            return AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].SmoothnessEnabled);
        }

        public bool ControllerSmoothnessEnable(string controllerName)
        {
            if (!AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].SmoothnessEnabled))
            {
                SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].SmoothnessEnabled.Current.NativeWindowHandle);
                return true;
            }
            return false;
        }

        public bool ControllerSmoothnessDisable(string controllerName)
        {
            if (AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].SmoothnessEnabled))
            {
                SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].SmoothnessEnabled.Current.NativeWindowHandle);
                return false;
            }
            return true;
        }

        public bool ControllerSmoothnessToggle(string controllerName)
        {
            SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].SmoothnessEnabled.Current.NativeWindowHandle);
            if (AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].SmoothnessEnabled))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Acceleration of Controller
        public int ControllerAccelerationIncrement(string controllerName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Acceleration.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Acceleration.Current.NativeWindowHandle, currVal + steps);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Acceleration.Current.NativeWindowHandle);
        }

        public int ControllerAccelerationDecrement(string controllerName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Acceleration.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Acceleration.Current.NativeWindowHandle, currVal - steps);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Acceleration.Current.NativeWindowHandle);
        }

        public int ControllerAccelerationSet(string controllerName, int value)
        {
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Acceleration.Current.NativeWindowHandle, value);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Acceleration.Current.NativeWindowHandle);
        }

        public int ControllerAccelerationGet(string controllerName)
        {
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Acceleration.Current.NativeWindowHandle);
        }
        #endregion

        #region Min Speed of Controller
        public int ControllerMinSpeedIncrement(string controllerName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MinSpeed.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MinSpeed.Current.NativeWindowHandle, currVal + steps);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MinSpeed.Current.NativeWindowHandle);
        }

        public int ControllerMinSpeedDecrement(string controllerName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MinSpeed.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MinSpeed.Current.NativeWindowHandle, currVal - steps);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MinSpeed.Current.NativeWindowHandle);
        }

        public int ControllerMinSpeedSet(string controllerName, int value)
        {
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MinSpeed.Current.NativeWindowHandle, value);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MinSpeed.Current.NativeWindowHandle);
        }

        public int ControllerMinSpeedGet(string controllerName)
        {
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MinSpeed.Current.NativeWindowHandle);
        }
        #endregion

        #region Max Speed of Controller
        public int ControllerMaxSpeedIncrement(string controllerName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MaxSpeed.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MaxSpeed.Current.NativeWindowHandle, currVal + steps);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MaxSpeed.Current.NativeWindowHandle);
        }

        public int ControllerMaxSpeedDecrement(string controllerName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MaxSpeed.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MaxSpeed.Current.NativeWindowHandle, currVal - steps);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MaxSpeed.Current.NativeWindowHandle);
        }

        public int ControllerMaxSpeedSet(string controllerName, int value)
        {
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MaxSpeed.Current.NativeWindowHandle, value);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MaxSpeed.Current.NativeWindowHandle);
        }

        public int ControllerMaxSpeedGet(string controllerName)
        {
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MaxSpeed.Current.NativeWindowHandle);
        }
        #endregion

        #region Slider Smothing of Effect
        public int EffectSmoothingIncrement(string effectName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Smoothing.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Smoothing.Current.NativeWindowHandle, currVal + steps);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Smoothing.Current.NativeWindowHandle);
        }

        public int EffectSmoothingDecrement(string effectName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Smoothing.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Smoothing.Current.NativeWindowHandle, currVal - steps);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Smoothing.Current.NativeWindowHandle);
        }

        public int EffectSmoothingSet(string effectName, int value)
        {
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Smoothing.Current.NativeWindowHandle, value);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Smoothing.Current.NativeWindowHandle);
        }

        public int EffectSmoothingGet(string effectName)
        {
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Smoothing.Current.NativeWindowHandle);
        }
        #endregion

        #region Slider Intensity of Effect
        public int EffectIntensityIncrement(string effectName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Intensity.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Intensity.Current.NativeWindowHandle, currVal + steps);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Intensity.Current.NativeWindowHandle);
        }

        public int EffectIntensityDecrement(string effectName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Intensity.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Intensity.Current.NativeWindowHandle, currVal - steps);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Intensity.Current.NativeWindowHandle);
        }

        public int EffectIntensitySet(string effectName, int value)
        {
            GuiLoggerProvider.Instance.Log("Incoming Command: EffectIntensitySet " + effectName + " -- " + value);
            if (SimFeedbackInvoker.Instance.actionElements.Effects.ContainsKey(effectName))
            {
                GuiLoggerProvider.Instance.Log("Incoming Command: EffectIntensitySet  - Effekt gefunden");
            }
            else
            {
                GuiLoggerProvider.Instance.Log("Incoming Command: EffectIntensitySet  - Effekt nicht gefunden");
                GuiLoggerProvider.Instance.Log("Incoming Command: Effect Keys: " + String.Join(",",SimFeedbackInvoker.Instance.actionElements.Effects.Keys));
                GuiLoggerProvider.Instance.Log("Incoming Command: Controller Keys: " + String.Join(",",SimFeedbackInvoker.Instance.actionElements.Controllers.Keys));
            }

            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Intensity.Current.NativeWindowHandle, value);
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Intensity.Current.NativeWindowHandle);
        }

        public int EffectIntensityGet(string effectName)
        {
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Intensity.Current.NativeWindowHandle);
        }
        #endregion

        #region Linear Property of Effect
        public bool EffectLinearIsEnabled(string effectName)
        {
            return AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].LinearInterpolated);
        }

        public bool EffectLinearEnable(string effectName)
        {
            if (!AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].LinearInterpolated))
            {
                SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].LinearInterpolated.Current.NativeWindowHandle);
                return true;
            }
            return false;
        }

        public bool EffectLinearDisable(string effectName)
        {
            if (AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].LinearInterpolated))
            {
                SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].LinearInterpolated.Current.NativeWindowHandle);
                return false;
            }

            return true;
        }

        public bool EffectLinearToggle(string effectName)
        {
            SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].LinearInterpolated.Current.NativeWindowHandle);
            if (AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].LinearInterpolated))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Enabled Property of Effect
        public bool EffectIsEnabled(string effectName)
        {
            return AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Enabled);
        }

        public bool EffectEnable(string effectName)
        {
            if (!AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Enabled))
            {
                SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Enabled.Current.NativeWindowHandle);
                return true;
            }

            return false;
        }

        public bool EffectDisable(string effectName)
        {
            if (AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Enabled))
            {
                SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Enabled.Current.NativeWindowHandle);
                return false;
            }
            return true;
        }

        public bool EffectToggle(string effectName)
        {
            SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Enabled.Current.NativeWindowHandle);
            if (AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Enabled))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Mute Property of Effect
        public bool EffectIsMuted(string effectName)
        {
            return AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Muted);
        }

        public bool EffectMuteEnable(string effectName)
        {
            if (!AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Muted))
            {
                SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Muted.Current.NativeWindowHandle);
                return true;
            }
            return false;
        }

        public bool EffectMuteDisable(string effectName)
        {
            if (AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Muted))
            {
                SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Muted.Current.NativeWindowHandle);
                return false;
            }
            return true;
        }

        public bool EffectMuteToggle(string effectName)
        {
            SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Muted.Current.NativeWindowHandle);
            if (AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Muted))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}