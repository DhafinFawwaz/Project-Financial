using System;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] GameObject _flashLight;
    [SerializeField] LayerMask _enemyLayer;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) ToggleFlash();
        AdjustFlashRotationBasedOnMouse();
    }
    void FixedUpdate()
    {
        if(_flashLight.activeSelf) FlashEnemy();
    }
    void ToggleFlash()
    {
        _flashLight.SetActive(!_flashLight.activeSelf);
    }
    void AdjustFlashRotationBasedOnMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 direction = (mousePos - screenCenter).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _flashLight.transform.rotation = Quaternion.Euler(-9, -angle+90, 0);
    }

    float _flashLightRange => transform.localScale.z*1.4f;
    float _flashLightRadius => transform.localScale.x;

    void FlashEnemy()
    {  
        Vector3 direction = _flashLight.transform.forward;
        // use box cast all
        RaycastHit[] hits = Physics.BoxCastAll(_flashLight.transform.position, new Vector3(_flashLightRadius/2, _flashLightRadius/2, 0), direction, _flashLight.transform.rotation, _flashLightRange, _enemyLayer);
        if(hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                Shadow s = hits[i].collider.gameObject.GetComponent<Shadow>();
                s.StopMoving();
            }
        }
    }
}
