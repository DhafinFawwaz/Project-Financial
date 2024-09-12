using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGun : MonoBehaviour
{
    [SerializeField] float _ratioToMouse = 0.1f;
    Vector3 _initialPos;

    void Awake()
    {
        _initialPos = transform.position;

        this.Invoke(() => {
            MouseCursor.Main.SetToTarget();
        }, 0.05f);
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        transform.position = _initialPos + (mousePos-_initialPos) * _ratioToMouse;
    }


    public void GameEnd()
    {
        MouseCursor.Main.SetToCursor();
    }
}
