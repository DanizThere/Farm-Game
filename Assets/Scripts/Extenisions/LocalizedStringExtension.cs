using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Localization;

public static class LocalizedStringExtension
{
    public static string GetLocalizedStringSafe(this LocalizedString localizedString)
    {
        if (localizedString == null || localizedString.IsEmpty) return "[Missing]";

        var result = localizedString.GetLocalizedString();
        return string.IsNullOrEmpty(result) ? "[Not localized]" : result;
    }
}
