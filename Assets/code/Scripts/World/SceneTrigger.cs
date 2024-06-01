using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SceneTrigger : MonoBehaviour
{
    [SerializeField] string _playerTag = "Player";
    [SerializeField] string _sceneToLoad = "World";
    [SerializeField] SceneTransition _sceneTransition;
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(_playerTag))
        {
            Time.timeScale = 0;
            _sceneTransition.AddListenerBeforeIn(() => {
                Time.timeScale = 1;
            }).StartSceneTransition(_sceneToLoad);
        }
    }
}
