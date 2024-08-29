using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSetter : MonoBehaviour
{
    [SerializeField] Vector3 _spawnPosition = Vector3.zero;
    public void Set()
    {
        Save.Data.Position = _spawnPosition;
    }
}
