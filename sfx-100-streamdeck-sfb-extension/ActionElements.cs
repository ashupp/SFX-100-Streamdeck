using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace WpfApp2
{
    class ActionElements
    {
        public Dictionary<string, ControllerActionElement> Controllers = new Dictionary<string, ControllerActionElement>();
        public Dictionary<string, EffectActionElement> Effects = new Dictionary<string, EffectActionElement>();

        public List<string> ControllerIndexAssignment = new List<string>();
        public List<string> EffectIndexAssignment = new List<string>();

        public AutomationElement OverallIntensity;
        public AutomationElement BtnResetOverallIntensity;
        public AutomationElement BtnEnableAllEffects;
        public AutomationElement BtnDisableAllEffects;
    }

    class EffectActionElement
    {
        public string Name;
        public AutomationElement Intensity;
        public AutomationElement Smoothing;
        public AutomationElement LinearInterpolated;
        public AutomationElement Enabled;
        public AutomationElement Muted;
    }

    class ControllerActionElement
    {
        public string Name;
        public AutomationElement Intensity;
        public AutomationElement IntensityResetButton;
        public AutomationElement SmoothnessEnabled;
        public AutomationElement Smoothness;
        public AutomationElement Acceleration;
        public AutomationElement MinSpeed;
        public AutomationElement MaxSpeed;
    }




}
