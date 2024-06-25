using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionStarter : MonoBehaviour
{
    [SerializeField] SceneTransition _sceneTransition;
    [SerializeField] float _delay = 0.5f;

    public void StartTransition(string sceneName)
    {
        StartCoroutine(DelayedTransition(sceneName));
    }

    IEnumerator DelayedTransition(string sceneName)
    {
        yield return new WaitForSeconds(_delay);
        _sceneTransition.StartSceneTransition(sceneName);
    }
}
