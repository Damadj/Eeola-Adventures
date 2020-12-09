using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class XPManager
{
    public static int CalculateXP(Enemy e)
    {
        var baseXP = Eeola.MyInstance.MyLevel * 5 + 45;
        var grayLevel = Eeola.MyInstance.MyLevel - 10;
        var totalXP = 0;

        if (e.MyLevel >= Eeola.MyInstance.MyLevel)
        {
            totalXP =  (int) Mathf.Floor(baseXP * (1 + 0.05f * (e.MyLevel - Eeola.MyInstance.MyLevel)));
        }
        else if (e.MyLevel > grayLevel)
        {
            totalXP = baseXP * (1 - (Eeola.MyInstance.MyLevel - e.MyLevel) / ZeroDifference());
        }

        return totalXP;
    }

    public static int CalculateXp(Quest quest)
    {
        if (Eeola.MyInstance.MyLevel <= quest.MyLevel + 5) return quest.MyXp;
        if (Eeola.MyInstance.MyLevel == quest.MyLevel + 6) return (int) Mathf.Floor(quest.MyXp * 0.8f / 5 * 5);
        if (Eeola.MyInstance.MyLevel == quest.MyLevel + 7) return (int) Mathf.Floor(quest.MyXp * 0.6f / 5 * 5);
        if (Eeola.MyInstance.MyLevel == quest.MyLevel + 8) return (int) Mathf.Floor(quest.MyXp * 0.4f / 5 * 5);
        if (Eeola.MyInstance.MyLevel == quest.MyLevel + 9) return (int) Mathf.Floor(quest.MyXp * 0.2f / 5 * 5);
        if (Eeola.MyInstance.MyLevel >= quest.MyLevel + 10) return (int) Mathf.Floor(quest.MyXp * 0.1f / 5 * 5);

        return 0;
    }
    private static int ZeroDifference()
    {
        if (Eeola.MyInstance.MyLevel <= 7) return 5;
        if (Eeola.MyInstance.MyLevel >= 8 && Eeola.MyInstance.MyLevel <= 9) return 6;
        if (Eeola.MyInstance.MyLevel >= 10 && Eeola.MyInstance.MyLevel <= 11) return 7;
        if (Eeola.MyInstance.MyLevel >= 12 && Eeola.MyInstance.MyLevel <= 15) return 8;
        if (Eeola.MyInstance.MyLevel >= 16 && Eeola.MyInstance.MyLevel <= 19) return 9;
        if (Eeola.MyInstance.MyLevel >= 20 && Eeola.MyInstance.MyLevel <= 29) return 11;
        if (Eeola.MyInstance.MyLevel >= 30 && Eeola.MyInstance.MyLevel <= 39) return 12;
        if (Eeola.MyInstance.MyLevel >= 40 && Eeola.MyInstance.MyLevel <= 44) return 13;
        if (Eeola.MyInstance.MyLevel >= 45 && Eeola.MyInstance.MyLevel <= 49) return 14;
        if (Eeola.MyInstance.MyLevel >= 50 && Eeola.MyInstance.MyLevel <= 54) return 15;
        if (Eeola.MyInstance.MyLevel >= 55 && Eeola.MyInstance.MyLevel <= 59) return 14;
        return 17;
    }

    private static int CalculateGrayLevel()
    {
        if (Eeola.MyInstance.MyLevel <= 5) return 0;
        else if (Eeola.MyInstance.MyLevel >= 6 && Eeola.MyInstance.MyLevel <= 49) return Eeola.MyInstance.MyLevel - (int) Mathf.Floor(Eeola.MyInstance.MyLevel / 10) - 5;
        else if (Eeola.MyInstance.MyLevel == 50) return Eeola.MyInstance.MyLevel - 10;
        else if (Eeola.MyInstance.MyLevel >= 51 && Eeola.MyInstance.MyLevel <= 59) return Eeola.MyInstance.MyLevel - (int) Mathf.Floor(Eeola.MyInstance.MyLevel / 5) - 1;
        return Eeola.MyInstance.MyLevel - 9;
    }
}
