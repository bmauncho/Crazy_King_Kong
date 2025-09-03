using System;
using System.Globalization;

public static class NumberFormatter
{
    public static string FormatString ( string input , int decimalPlaces , bool forceFixedDecimals = false )
    {
        if (double.TryParse(input , NumberStyles.Any , CultureInfo.InvariantCulture , out double value))
        {
            value = Math.Round(value , decimalPlaces);

            if (forceFixedDecimals)
            {
                // Always show the exact number of decimal places, even trailing zeros
                string format = "F" + decimalPlaces;
                return value.ToString(format , CultureInfo.CurrentCulture);
            }
            else
            {
                // Show decimals only when needed
                if (value % 1 == 0)
                    return value.ToString("F0" , CultureInfo.CurrentCulture);
                else
                    return value.ToString("F" + decimalPlaces , CultureInfo.CurrentCulture);
            }
        }

        return input; // fallback if parsing fails
    }
}
