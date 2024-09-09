using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] SceneTransition _sceneTransition;
    
    public void StartGame() {
        if(Save.Data.CurrentDay == 0 && Save.Data.DayState == DayState.JustGotHome)
            _sceneTransition.StartSceneTransition("Prolog");
        else 
            _sceneTransition.StartSceneTransition("Bedroom");
        
    }

    public void ExitGame() => Application.Quit();


    public void NewGame() {
        Save.Data = new SaveData();
        _sceneTransition.StartSceneTransition("Bedroom");
    }


    
}
