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
        public bool IncrementOverallIntensity(int steps)
        {
            try
            {
                GuiLoggerProvider.Instance.Log("Incoming Command: IncrementOverallIntensity by " + steps);
                for (var i = steps - 1; i >= 0; i--)
                    SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.IncrementOverallIntensity();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DecrementOverallIntensity(int steps)
        {
            try
            {
                GuiLoggerProvider.Instance.Log("Incoming Command: DecrementOverallIntensity by " + steps);
                for (var i = steps - 1; i >= 0; i--)
                    SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.DecrementOverallIntensity();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SetOverallIntensity(int value)
        {
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr) SimFeedbackInvoker.Instance.actionElements.OverallIntensity.Current.NativeWindowHandle,value);
        }

        public int GetOverallIntensity()
        {
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.OverallIntensity.Current.NativeWindowHandle);
        }

        public void ResetOverallIntensity()
        {
            SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.BtnResetOverallIntensity.Current.NativeWindowHandle);
        }
        #endregion

        #region Intensity of Controller
        public void ControllerIntensityIncrement(string controllerName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Intensity.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Intensity.Current.NativeWindowHandle, currVal + steps);
        }

        public void ControllerIntensityDecrement(string controllerName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Intensity.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Intensity.Current.NativeWindowHandle, currVal - steps);
        }

        public void ControllerIntensitySet(string controllerName, int value)
        {
            GuiLoggerProvider.Instance.Log("Incoming Command: ControllerIntensitySet " + controllerName + " -- " + value);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Intensity.Current.NativeWindowHandle, value);
        }

        public int ControllerIntensityGet(string controllerName)
        {
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr) SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Intensity.Current.NativeWindowHandle);
        }

        public void ControllerIntensityReset(string controllerName)
        {
            SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].IntensityResetButton.Current.NativeWindowHandle);
        }
        #endregion

        #region Smoothing of Controller
        public void ControllerSmoothnessIncrement(string controllerName, int steps)
        {
            var currVal =SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Smoothness.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Smoothness.Current.NativeWindowHandle, currVal + steps);
        }

        public void ControllerSmoothnessDecrement(string controllerName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Smoothness.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Smoothness.Current.NativeWindowHandle, currVal - steps);
        }

        public void ControllerSmoothnessSet(string controllerName, int value)
        {
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Smoothness.Current.NativeWindowHandle, value);
        }

        public int ControllerSmoothnessGet(string controllerName)
        {
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Smoothness.Current.NativeWindowHandle);
        }

        public bool ControllerSmoothnessIsEnabled(string controllerName)
        {
            return AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].SmoothnessEnabled);
        }

        public void ControllerSmoothnessEnable(string controllerName)
        {
            if (!AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].SmoothnessEnabled))
            {
                SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].SmoothnessEnabled.Current.NativeWindowHandle);
            }
        }

        public void ControllerSmoothnessDisable(string controllerName)
        {
            if (AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].SmoothnessEnabled))
            {
                SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].SmoothnessEnabled.Current.NativeWindowHandle);
            }
        }

        public void ControllerSmoothnessToggle(string controllerName)
        {
            SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].SmoothnessEnabled.Current.NativeWindowHandle);
        }
        #endregion

        #region Acceleration of Controller
        public void ControllerAccelerationIncrement(string controllerName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Acceleration.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Acceleration.Current.NativeWindowHandle, currVal + steps);
        }

        public void ControllerAccelerationDecrement(string controllerName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Acceleration.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Acceleration.Current.NativeWindowHandle, currVal - steps);
        }

        public void ControllerAccelerationSet(string controllerName, int value)
        {
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Acceleration.Current.NativeWindowHandle, value);
        }

        public int ControllerAccelerationGet(string controllerName)
        {
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].Acceleration.Current.NativeWindowHandle);
        }
        #endregion

        #region Min Speed of Controller
        public void ControllerMinSpeedIncrement(string controllerName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MinSpeed.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MinSpeed.Current.NativeWindowHandle, currVal + steps);
        }

        public void ControllerMinSpeedDecrement(string controllerName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MinSpeed.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MinSpeed.Current.NativeWindowHandle, currVal - steps);
        }

        public void ControllerMinSpeedSet(string controllerName, int value)
        {
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MinSpeed.Current.NativeWindowHandle, value);
        }

        public int ControllerMinSpeedGet(string controllerName)
        {
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MinSpeed.Current.NativeWindowHandle);
        }
        #endregion

        #region Max Speed of Controller
        public void ControllerMaxSpeedIncrement(string controllerName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MaxSpeed.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MaxSpeed.Current.NativeWindowHandle, currVal + steps);
        }

        public void ControllerMaxSpeedDecrement(string controllerName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MaxSpeed.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MaxSpeed.Current.NativeWindowHandle, currVal - steps);
        }

        public void ControllerMaxSpeedSet(string controllerName, int value)
        {
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MaxSpeed.Current.NativeWindowHandle, value);
        }

        public int ControllerMaxSpeedGet(string controllerName)
        {
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Controllers[controllerName].MaxSpeed.Current.NativeWindowHandle);
        }
        #endregion

        #region Slider Smothing of Effect
        public void EffectSmoothingIncrement(string effectName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Smoothing.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Smoothing.Current.NativeWindowHandle, currVal + steps);
        }

        public void EffectSmoothingDecrement(string effectName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Smoothing.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Smoothing.Current.NativeWindowHandle, currVal - steps);
        }

        public void EffectSmoothingSet(string effectName, int value)
        {
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Smoothing.Current.NativeWindowHandle, value);
        }

        public int EffectSmoothingGet(string effectName)
        {
            return SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Smoothing.Current.NativeWindowHandle);
        }
        #endregion

        #region Slider Intensity of Effect
        public void EffectIntensityIncrement(string effectName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Intensity.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Intensity.Current.NativeWindowHandle, currVal + steps);
        }

        public void EffectIntensityDecrement(string effectName, int steps)
        {
            var currVal = SimFeedbackInvoker.Instance.GetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Intensity.Current.NativeWindowHandle);
            SimFeedbackInvoker.Instance.SetCurrentSliderValue((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Intensity.Current.NativeWindowHandle, currVal - steps);
        }

        public void EffectIntensitySet(string effectName, int value)
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

        public void EffectLinearEnable(string effectName)
        {
            if (!AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].LinearInterpolated))
            {
                SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].LinearInterpolated.Current.NativeWindowHandle);
            }
        }

        public void EffectLinearDisable(string effectName)
        {
            if (AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].LinearInterpolated))
            {
                SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].LinearInterpolated.Current.NativeWindowHandle);
            }
        }

        public void EffectLinearToggle(string effectName)
        {
            SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].LinearInterpolated.Current.NativeWindowHandle);
        }
        #endregion

        #region Enabled Property of Effect
        public bool EffectIsEnabled(string effectName)
        {
            return AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Enabled);
        }

        public void EffectEnable(string effectName)
        {
            if (!AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Enabled))
            {
                SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Enabled.Current.NativeWindowHandle);
            }
        }

        public void EffectDisable(string effectName)
        {
            if (AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Enabled))
            {
                SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Enabled.Current.NativeWindowHandle);
            }
        }

        public void EffectToggle(string effectName)
        {

            SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Enabled.Current.NativeWindowHandle);
        }
        #endregion

        #region Mute Property of Effect
        public bool EffectIsMuted(string effectName)
        {
            return AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Muted);
        }

        public void EffectMuteEnable(string effectName)
        {
            if (!AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Muted))
            {
                SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Muted.Current.NativeWindowHandle);
            }
        }

        public void EffectMuteDisable(string effectName)
        {
            if (AutomationExtensions.IsElementToggledOn(SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Muted))
            {
                SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Muted.Current.NativeWindowHandle);
            }
        }

        public void EffectMuteToggle(string effectName)
        {
            SimFeedbackInvoker.Instance.ClickElement((IntPtr)SimFeedbackInvoker.Instance.actionElements.Effects[effectName].Muted.Current.NativeWindowHandle);
        }
        #endregion
    }
}