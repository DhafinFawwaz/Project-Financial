using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SceneTrigger : MonoBehaviour
{
    [SerializeField] string _playerTag = "Player";
    [SerializeField] string _sceneToLoad = "World";
    [SerializeField] SceneTransition _sceneTransition;

    [SerializeField] bool _movePlayerToSpawnPoint = false;
    [SerializeField] Vector3 _spawnPoint = Vector3.zero;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(_playerTag))
        {
            Time.timeScale = 0;
            _sceneTransition.AddListenerBeforeIn(() => {
                Time.timeScale = 1;
            }).StartSceneTransition(_sceneToLoad);
            if(_movePlayerToSpawnPoint){
                PlayerCore.SetPlayerSpawnPosition(_spawnPoint);
            }
        }
    }
}
