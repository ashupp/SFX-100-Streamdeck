using System;
using System.Windows.Automation;

namespace sfx_100_streamdeck_sfb_extension
{
    public static class AutomationExtensions
    {
        public static string GetText(this AutomationElement element)
            => element.TryGetCurrentPattern(ValuePattern.Pattern, out object patternValue) ? ((ValuePattern)patternValue).Current.Value
                : element.TryGetCurrentPattern(TextPattern.Pattern, out object patternText) ? ((TextPattern)patternText).DocumentRange.GetText(-1).TrimEnd('\r') // often there is an extra '\r' hanging off the end.
                : element.Current.Name;

        // Does not notify...
        public static void SetSliderValue(AutomationElement element, double theValue)
        {
            element.SetFocus();
            var range = element.GetCurrentPattern(RangeValuePattern.Pattern) as RangeValuePattern;
            range.SetValue(theValue);
        }

        // This will bring the window to front if it is in normal or maximized state. Using Win32 Click instead
        public static void ChangeCheckBoxState(AutomationElement element)
        {
            TogglePattern targetInvokePattern = element.GetCurrentPattern(TogglePattern.Pattern) as TogglePattern;
            if (targetInvokePattern == null)
                return;
            targetInvokePattern.Toggle();
        }

        // Currently Unused
        public static void InvokeElement(AutomationElement element)
        {
            InvokePattern targetInvokePattern = element.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            if (targetInvokePattern == null)
                return;
            targetInvokePattern.Invoke();
        }

        public static bool IsElementToggledOn(AutomationElement element)
        {
            if (element == null)
            {
                // TODO: Invalid parameter error handling.
                return false;
            }

            Object objPattern;
            TogglePattern togPattern;
            if (true == element.TryGetCurrentPattern(TogglePattern.Pattern, out objPattern))
            {
                togPattern = objPattern as TogglePattern;
                if (togPattern.Current.ToggleState == ToggleState.On)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // TODO: Object doesn't support TogglePattern error handling.
            return false;
        }


    }
}
