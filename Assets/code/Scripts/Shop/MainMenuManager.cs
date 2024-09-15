using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] SceneTransition _sceneTransition;
    [SerializeField] PopUp _popUp;
    public void StartGame() {
        if(Save.Data == null || (Save.Data.CurrentDay == 0 && Save.Data.DayState == DayState.JustGotHome))
            _sceneTransition.StartSceneTransition("Prolog");
        else 
            _popUp.Show();
    }

    public void ExitGame() => Application.Quit();


    public void NewGame() {
        Save.Data = new SaveData();
        _sceneTransition.StartSceneTransition("Prolog");
    }

    public void ContinueGame() {
        _sceneTransition.StartSceneTransition("Bedroom");
    }


    
}
