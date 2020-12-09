using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Quality {Common, Magic, Rare, Unique, Set}
public static class QualityColor
{
    private static Dictionary<Quality, string> colors = new Dictionary<Quality, string>()
    {
        {Quality.Common, "#d6d6d6"},
        {Quality.Magic, "#0026FF"},
        {Quality.Rare, "#FFE460"},
        {Quality.Unique, "#B200FF"},
        {Quality.Set, "#00B200"}
    };

    public static Dictionary<Quality,string> MyColors { get => colors; }
}
