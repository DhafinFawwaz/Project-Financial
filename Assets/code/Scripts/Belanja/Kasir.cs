using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Kasir : MonoBehaviour
{
    [SerializeField] BelanjaList _belanjaList;
    [SerializeField] PopUp _notCompletedPopUp;
    [SerializeField] SceneTransition _sceneTransition;
    [SerializeField] Interactable _interactable;
    public void Interact()
    {
        List<ItemData> items = _belanjaList.GetNotInCartItems();
        if(items.Count > 0)
        {
            InputManager.SetActiveMouseAndKey(false);
            _notCompletedPopUp.Show();
            EventSystem.current.SetSelectedGameObject(_notCompletedPopUp.transform.GetChild(0).gameObject);
            return;
        }
        _sceneTransition.StartSceneTransition("AfterBelanja");
    }

    public void Ok()
    {
        this.Invoke(() => {
            InputManager.SetActiveMouseAndKey(true);
        }, 0.1f);
    }

    void Update()
    {
        if(_interactable.IsShowingPrompt && Input.GetKeyDown(KeyCode.E) && !InputManager.CanGetKeyDown)
        {
            EventSystem.current.SetSelectedGameObject(null);
            _notCompletedPopUp.Hide();
            Ok();
        }
    }
}
