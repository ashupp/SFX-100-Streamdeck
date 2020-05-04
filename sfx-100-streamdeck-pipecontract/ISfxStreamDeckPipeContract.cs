using System.ServiceModel;

namespace sfx_100_streamdeck_pipecontract
{
    [ServiceContract]
    public interface ISfxStreamDeckPipeContract
    {
        [OperationContract]
        bool CheckConnection();

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
        bool IncrementOverallIntensity(int steps);

        [OperationContract]
        bool DecrementOverallIntensity(int steps);

        [OperationContract]
        void SetOverallIntensity(int value);

        [OperationContract]
        int GetOverallIntensity();

        [OperationContract]
        void ResetOverallIntensity(); 

        #region Controller Intensity

        [OperationContract]
        void ControllerIntensityIncrement(string controllerName, int steps);

        [OperationContract]
        void ControllerIntensityDecrement(string controllerName, int steps);

        [OperationContract]
        void ControllerIntensitySet(string controllerName, int value);

        [OperationContract]
        int ControllerIntensityGet(string controllerName);

        [OperationContract]
        void ControllerIntensityReset(string controllerName);
        #endregion

        #region Controller Smoothness

        [OperationContract]
        void ControllerSmoothnessIncrement(string controllerName, int steps);

        [OperationContract]
        void ControllerSmoothnessDecrement(string controllerName, int steps);

        [OperationContract]
        void ControllerSmoothnessSet(string controllerName, int value);

        [OperationContract]
        int ControllerSmoothnessGet(string controllerName);

        [OperationContract]
        bool ControllerSmoothnessIsEnabled(string controllerName);

        [OperationContract]
        void ControllerSmoothnessEnable(string controllerName);

        [OperationContract]
        void ControllerSmoothnessDisable(string controllerName);

        [OperationContract]
        void ControllerSmoothnessToggle(string controllerName);

        #endregion

        #region Controller Acceleration 

        [OperationContract]
        void ControllerAccelerationIncrement(string controllerName, int steps);

        [OperationContract]
        void ControllerAccelerationDecrement(string controllerName, int steps);

        [OperationContract]
        void ControllerAccelerationSet(string controllerName, int value);

        [OperationContract]
        int ControllerAccelerationGet(string controllerName);

        #endregion

        #region Controller Min Speed 

        [OperationContract]
        void ControllerMinSpeedIncrement(string controllerName, int steps);

        [OperationContract]
        void ControllerMinSpeedDecrement(string controllerName, int steps);

        [OperationContract]
        void ControllerMinSpeedSet(string controllerName, int value);

        [OperationContract]
        int ControllerMinSpeedGet(string controllerName);

        #endregion

        #region Controller Max Speed 

        [OperationContract]
        void ControllerMaxSpeedIncrement(string controllerName, int steps);

        [OperationContract]
        void ControllerMaxSpeedDecrement(string controllerName, int steps);

        [OperationContract]
        void ControllerMaxSpeedSet(string controllerName, int value);

        [OperationContract]
        int ControllerMaxSpeedGet(string controllerName);

        #endregion

        #region Effect Smoothing

        [OperationContract]
        void EffectSmoothingIncrement(string effectName, int steps);

        [OperationContract]
        void EffectSmoothingDecrement(string effectName, int steps);

        [OperationContract]
        void EffectSmoothingSet(string effectName, int value);

        [OperationContract]
        int EffectSmoothingGet(string effectName);

        #endregion

        #region Effect Intensity

        [OperationContract]
        void EffectIntensityIncrement(string effectName, int steps);

        [OperationContract]
        void EffectIntensityDecrement(string effectName, int steps);

        [OperationContract]
        void EffectIntensitySet(string effectName, int value);

        [OperationContract]
        int EffectIntensityGet(string effectName);

        #endregion

        #region Effect Linear
        [OperationContract]
        bool EffectLinearIsEnabled(string effectName);

        [OperationContract]
        void EffectLinearEnable(string effectName);

        [OperationContract]
        void EffectLinearDisable(string effectName);

        [OperationContract]
        void EffectLinearToggle(string effectName);
        #endregion

        #region Effect Enabled
        [OperationContract]
        bool EffectIsEnabled(string effectName);

        [OperationContract]
        void EffectEnable(string effectName);

        [OperationContract]
        void EffectDisable(string effectName);

        [OperationContract]
        void EffectToggle(string effectName);
        #endregion

        #region Effect Muted
        [OperationContract]
        bool EffectIsMuted(string effectName);

        [OperationContract]
        void EffectMuteEnable(string effectName);

        [OperationContract]
        void EffectMuteDisable(string effectName);

        [OperationContract]
        void EffectMuteToggle(string effectName);

        #endregion
    }
}
