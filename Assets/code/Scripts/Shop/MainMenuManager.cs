using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] SceneTransition _sceneTransition;
    
    public void StartGame() => _sceneTransition.StartSceneTransition("House");

    public void ExitGame() => Application.Quit();


    
}
