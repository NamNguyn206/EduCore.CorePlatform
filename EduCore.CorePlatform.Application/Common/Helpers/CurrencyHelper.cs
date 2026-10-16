using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EduCore.CorePlatform.Application.Common.Helpers;

public static class CurrencyHelper
{
    public static string Format(decimal value, string cultureCode = "vi-VN", bool hasDecimal = false)
    {
        var culture = new CultureInfo(cultureCode);

        return value.ToString(hasDecimal ? "C" : "C0", culture);
    }
}