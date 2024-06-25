using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSetter : MonoBehaviour
{
    [SerializeField] bool _changeLightColor = false;
    [SerializeField] Color _lightColor = Color.white;
    [SerializeField] bool _movePlayerToSpawnPoint = false;
    [SerializeField] Vector3 _spawnPosition = Vector3.zero;
    public void Set()
    {
        if(_changeLightColor) PlayerCore.SetLightColor(_lightColor);
        if(_movePlayerToSpawnPoint) PlayerCore.SetPlayerSpawnPosition(_spawnPosition);
    }
}
