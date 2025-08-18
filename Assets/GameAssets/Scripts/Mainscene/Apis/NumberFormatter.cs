using System;
using System.Globalization;

public static class NumberFormatter
{
    public static string FormatBetAmount ( string input ,int decimalplaces)
    {
        if (double.TryParse(input , NumberStyles.Any , CultureInfo.InvariantCulture , out double value))
        {
            // Round to 1 decimal place
            value = Math.Round(value , decimalplaces);

            // If it's effectively a whole number after rounding, format without decimals
            if (value % 1 == 0)
                return value.ToString("F0" , CultureInfo.CurrentCulture);
            else
                return value.ToString("F1" , CultureInfo.CurrentCulture);
        }

        return input; // fallback if parsing fails
    }
}
