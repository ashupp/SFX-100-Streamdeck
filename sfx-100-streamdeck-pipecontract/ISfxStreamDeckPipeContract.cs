using System.Collections.Generic;
using System.ServiceModel;

namespace sfx_100_streamdeck_pipecontract
{
    [ServiceContract]
    public interface ISfxStreamDeckPipeContract
    {
        [OperationContract]
        bool CheckConnection();

        [OperationContract]
        List<string> GetControllerNames();

        [OperationContract]
        List<string> GetEffectNames();

        [OperationContract]
        bool IsRunning();

        [OperationContract]
        bool IsTelemetryProviderConnected();

        [OperationContract]
        bool StartMotion();

        [OperationContract]
        bool StopMotion();

        [OperationContract]
        bool EnableAllEffects();

        [OperationContract]
        bool DisableAllEffects();

        [OperationContract]
        int IncrementOverallIntensity(int steps);

        [OperationContract]
        int DecrementOverallIntensity(int steps);

        [OperationContract]
        int SetOverallIntensity(int value);

        [OperationContract]
        int GetOverallIntensity();

        [OperationContract]
        int ResetOverallIntensity(); 

        #region Controller Intensity

        [OperationContract]
        int ControllerIntensityIncrement(string controllerName, int steps);

        [OperationContract]
        int ControllerIntensityDecrement(string controllerName, int steps);

        [OperationContract]
        int ControllerIntensitySet(string controllerName, int value);

        [OperationContract]
        int ControllerIntensityGet(string controllerName);

        [OperationContract]
        int ControllerIntensityReset(string controllerName);
        #endregion

        #region Controller Smoothness

        [OperationContract]
        int ControllerSmoothnessIncrement(string controllerName, int steps);

        [OperationContract]
        int ControllerSmoothnessDecrement(string controllerName, int steps);

        [OperationContract]
        int ControllerSmoothnessSet(string controllerName, int value);

        [OperationContract]
        int ControllerSmoothnessGet(string controllerName);

        [OperationContract]
        bool ControllerSmoothnessIsEnabled(string controllerName);

        [OperationContract]
        bool ControllerSmoothnessEnable(string controllerName);

        [OperationContract]
        bool ControllerSmoothnessDisable(string controllerName);

        [OperationContract]
        bool ControllerSmoothnessToggle(string controllerName);

        #endregion

        #region Controller Acceleration 

        [OperationContract]
        int ControllerAccelerationIncrement(string controllerName, int steps);

        [OperationContract]
        int ControllerAccelerationDecrement(string controllerName, int steps);

        [OperationContract]
        int ControllerAccelerationSet(string controllerName, int value);

        [OperationContract]
        int ControllerAccelerationGet(string controllerName);

        #endregion

        #region Controller Min Speed 

        [OperationContract]
        int ControllerMinSpeedIncrement(string controllerName, int steps);

        [OperationContract]
        int ControllerMinSpeedDecrement(string controllerName, int steps);

        [OperationContract]
        int ControllerMinSpeedSet(string controllerName, int value);

        [OperationContract]
        int ControllerMinSpeedGet(string controllerName);

        #endregion

        #region Controller Max Speed 

        [OperationContract]
        int ControllerMaxSpeedIncrement(string controllerName, int steps);

        [OperationContract]
        int ControllerMaxSpeedDecrement(string controllerName, int steps);

        [OperationContract]
        int ControllerMaxSpeedSet(string controllerName, int value);

        [OperationContract]
        int ControllerMaxSpeedGet(string controllerName);

        #endregion

        #region Effect Smoothing

        [OperationContract]
        int EffectSmoothingIncrement(string effectName, int steps);

        [OperationContract]
        int EffectSmoothingDecrement(string effectName, int steps);

        [OperationContract]
        int EffectSmoothingSet(string effectName, int value);

        [OperationContract]
        int EffectSmoothingGet(string effectName);

        #endregion

        #region Effect Intensity

        [OperationContract]
        int EffectIntensityIncrement(string effectName, int steps);

        [OperationContract]
        int EffectIntensityDecrement(string effectName, int steps);

        [OperationContract]
        int EffectIntensitySet(string effectName, int value);

        [OperationContract]
        int EffectIntensityGet(string effectName);

        #endregion

        #region Effect Linear
        [OperationContract]
        bool EffectLinearIsEnabled(string effectName);

        [OperationContract]
        bool EffectLinearEnable(string effectName);

        [OperationContract]
        bool EffectLinearDisable(string effectName);

        [OperationContract]
        bool EffectLinearToggle(string effectName);
        #endregion

        #region Effect Enabled
        [OperationContract]
        bool EffectIsEnabled(string effectName);

        [OperationContract]
        bool EffectEnable(string effectName);

        [OperationContract]
        bool EffectDisable(string effectName);

        [OperationContract]
        bool EffectToggle(string effectName);
        #endregion

        #region Effect Muted
        [OperationContract]
        bool EffectIsMuted(string effectName);

        [OperationContract]
        bool EffectMuteEnable(string effectName);

        [OperationContract]
        bool EffectMuteDisable(string effectName);

        [OperationContract]
        bool EffectMuteToggle(string effectName);

        #endregion
    }
}
