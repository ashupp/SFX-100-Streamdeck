using System;

namespace sfx_100_streamdeck_sfb_extension
{
    class SimFeedbackFacadeProvider
    {
        #region Singleton
        private static volatile SimFeedbackFacadeProvider instance;
        private static object syncRoot = new Object();

        private SimFeedbackFacadeProvider() { }

        public static SimFeedbackFacadeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new SimFeedbackFacadeProvider();
                        }
                        return instance;
                    }
                }
                return instance;
            }
        }
        #endregion


        public SimFeedback.extension.SimFeedbackExtensionFacade SimFeedbackFacade;
    }
}