using System;
using System.Net.NetworkInformation;

using Scripts.Data;

using UnityEngine;

public static class Links
{
    public static string VersionURL = "https://ddragon.leagueoflegends.com/api/versions.json";

    public static string ChampionListURL(string version)
    {
        return $"https://ddragon.leagueoflegends.com/cdn/{version}/data/ko_KR/champion.json\r\n";
    }

    public static string ChampionPortraitURL(string version, string championId)
    {
        return $"https://ddragon.leagueoflegends.com/cdn/{version}/img/champion/{ChampionPortraitName(championId)}";
    }

    public static string ChampionLoadingURL(string championId)
    {
        return $"https://ddragon.leagueoflegends.com/cdn/img/champion/loading/{ChampionLoadingName(championId)}";
    }

    public static string ChampionSplashURL(string championId)
    {
        return $"https://ddragon.leagueoflegends.com/cdn/img/champion/splash/{ChampionSplashName(championId)}";
    }

    public static string ChampionPortraitName(string championId)
    {
        return $"{championId}.png";
    }

    public static string ChampionLoadingName(string championId)
    {
        return $"{championId}_0.jpg";
    }

    public static string ChampionSplashName(string championId)
    {
        return $"{championId}_0.jpg";
    }

    public static string SavePortraitPath = $"{Application.persistentDataPath}/Image/ChampionPortrait/";
    public static string SaveLoadingPath = $"{Application.persistentDataPath}/Image/ChampionLoading/";
    public static string SaveSplashPath = $"{Application.persistentDataPath}/Image/ChampionSplash/";

    public static string UserSettingPath = $"{Application.persistentDataPath}/UserSetting/";
}
