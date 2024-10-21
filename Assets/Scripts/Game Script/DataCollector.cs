using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollector : MonoBehaviour
{
    private static int score = 0, virusesKilled = 0, ventsClosed = 0, shopPoints = 0;
    public static int tempScore = 0, tempViruses = 0, tempVents = 0, tempPoints = 0;
    public static void UpdateScores()
    {
        score += tempScore;
        virusesKilled += tempViruses;
        ventsClosed += tempVents;
        shopPoints += tempPoints;
        ResetLevelScores();
    }
    public static int[] GetScores()
    {
        return new int[] { score, virusesKilled, ventsClosed, shopPoints };
    }
    public static void ResetLevelScores()
    {
        tempScore = 0;
        tempViruses = 0;
        tempVents = 0;
        tempPoints = 0;
    }
    public static void ResetScores()
    {
        score = 0;
        virusesKilled = 0;
        ventsClosed = 0;
        shopPoints = 0;
    }
}
