using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System;
using System.Reflection;
using System.Linq;

public enum Resolution {

    [Description("640x480")]
    R640x480 = 0,
    [Description("720x480")]
    R720x480 = 1,
    [Description("720x576")]
    R720x576 = 2,
    [Description("800x600")]
    R800x600 = 3,
    [Description("1024x768")]
    R1024x768 = 4,
    [Description("1152x864")]
    R1152x864 = 5,
    [Description("1176x664")]
    R1176x664 = 6,
    [Description("1280x720")]
    R1280x720 = 7,
    [Description("1280x768")]
    R1280x768 = 8,
    [Description("1280x800")]
    R1280x800 = 9,
    [Description("1280x960")]
    R1280x960 = 10,
    [Description("1280x1024")]
    R1280x1024 = 11,
    [Description("1360x768")]
    R1360x768 = 12,
    [Description("1366x768")]
    R1366x768 = 13,
    [Description("1440x900")]
    R1440x900 = 14,
    [Description("1600x900")]
    R1600x900 = 15,
    [Description("1600x1024")]
    R1600x1024 = 16,
    [Description("1680x1050")]
    R1680x1050 = 17,
    [Description("1920x1080")]
    R1920x1080 = 18



}

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        return ((DescriptionAttribute)Attribute.GetCustomAttribute(
            value.GetType().GetFields(BindingFlags.Public | BindingFlags.Static)
                .Single(x => x.GetValue(null).Equals(value)),
            typeof(DescriptionAttribute))).Description ?? value.ToString();
    }
}

