using System;

namespace sfx_100_streamdeck_sfb_extension
{
    class SimFeedbackFacadeProvider
    {
        #region Singleton
        private static readonly Lazy<SimFeedbackFacadeProvider> lazy
            = new Lazy<SimFeedbackFacadeProvider>(() => new SimFeedbackFacadeProvider());

        public static SimFeedbackFacadeProvider Instance
            => lazy.Value;

        private SimFeedbackFacadeProvider() { }
        #endregion


        public SimFeedback.extension.SimFeedbackExtensionFacade SimFeedbackFacade;
    }
}