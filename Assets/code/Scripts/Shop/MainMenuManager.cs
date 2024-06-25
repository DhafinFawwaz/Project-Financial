using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] SceneTransition _sceneTransition;
    
    public void StartGame() => _sceneTransition.StartSceneTransition("Prolog");

    public void ExitGame() => Application.Quit();


    
}
