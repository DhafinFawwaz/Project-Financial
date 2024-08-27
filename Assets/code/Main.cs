using UnityEngine;

public class Main
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialization()
    {
        Singleton.Initialize();
        ResolutionManager.Initialize();
        ResolutionManager.SetResolution(PlayerPrefs.GetInt("Resolution", 3));
        Save.Initialize();
    }
}
