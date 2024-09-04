using UnityEngine;

public class Main
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialization()
    {
        Singleton.Initialize();
        ResolutionManager.Initialize();
        ResolutionManager.SetResolution(PlayerPrefs.GetInt("Resolution", 3));
        Audio.SetSoundMixerVolume(0);
        Save.Initialize();
    }
}
