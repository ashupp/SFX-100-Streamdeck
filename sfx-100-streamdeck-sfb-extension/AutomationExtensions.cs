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
