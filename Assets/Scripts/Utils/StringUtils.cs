using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class StringUtils
{
    public static string Decimals2(float number)
    {
        return String.Format("{0:0.00}", number);
    }
    public static string Price(double number)
    {
        return String.Format(((Math.Round(number) == number) ? "{0:0}" : "{0:0.00}"), number);
    }
}