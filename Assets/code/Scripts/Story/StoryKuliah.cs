using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryKuliah : MonoBehaviour
{
    [SerializeField] SceneTransitionStarter _sceneTransitionStarter;
    [SerializeField] BelanjaListGenerator _belanjaListGenerator;
    public void LoadNext()
    {
        
        if((Save.Data.CurrentDay+1) % 3 == 0) {
            _sceneTransitionStarter.StartTransition("SetelahKuliah");
        } else {
            if(Save.Data.HaveAnyCredit()) {
                _sceneTransitionStarter.StartTransition("Budgeting");
            } else {
                _belanjaListGenerator.GenerateBelanjaList();
                _sceneTransitionStarter.StartTransition("World");
            }
        }
    }
}
