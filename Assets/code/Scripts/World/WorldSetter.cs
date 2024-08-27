using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSetter : MonoBehaviour
{
    [SerializeField] bool _movePlayerToSpawnPoint = false;
    [SerializeField] Vector3 _spawnPosition = Vector3.zero;
    public void Set()
    {
        if(_movePlayerToSpawnPoint) Save.Data.Position = _spawnPosition;
    }
}
