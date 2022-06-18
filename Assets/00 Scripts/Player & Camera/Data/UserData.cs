using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    private const string ks_user_SCORE = "user_score";
    private const string ks_user_COINS = "user_coins";
    private const string ks_user_LEVEL = "user_level";

    public static int ScoreNumber
    {
        get => SDKPlayerPrefs.GetInt(ks_user_SCORE, 0);
        set => SDKPlayerPrefs.SetInt(ks_user_SCORE, value);
    }

    public static int CoinsNumber
    {
        get => SDKPlayerPrefs.GetInt(ks_user_COINS, 0);
        set => SDKPlayerPrefs.SetInt(ks_user_COINS, value);
    }

    public static int LevelNumber
    {
        get => SDKPlayerPrefs.GetInt(ks_user_LEVEL, 1);
        set => SDKPlayerPrefs.SetInt(ks_user_LEVEL, value);
    }


}
