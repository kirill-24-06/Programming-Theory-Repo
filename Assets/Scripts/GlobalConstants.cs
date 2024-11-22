using UnityEngine;
public static class GlobalConstants
{
    //scenes
    public static readonly string MainMenuSceneName = "Menu";
    public static readonly string MainSceneName = "Main";

    //prefabs path
    public static readonly string LoadingScreenPrefabPath = "Prefabs/UI/LoadingScreen";

    public static readonly bool _encryptionRequired = true;
    //save paths
    public static readonly string JsonSavePath = "/savefileJson.json";
    public static readonly string BinarySavePath = Application.persistentDataPath + "/savefileBinary.dat";
}